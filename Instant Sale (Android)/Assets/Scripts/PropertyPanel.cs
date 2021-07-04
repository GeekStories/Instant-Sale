using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PropertyPanel : MonoBehaviour{

    public GameManager gameManager;
    public Text upgradeText, currentOfferText, tenantText;
    public Button upgradeButton, sellButton, findNewTenantButton;
    public GameObject ManagersBoxPanel;
    public Text propertyDetailsText;

    Card card;
    int cost, rentUpgradeAmount, currentOffer, activeSlot;

    public void OpenPropertyPanel(string name){
        gameObject.SetActive(!gameObject.activeInHierarchy);

        if (gameObject.activeInHierarchy){
            GameObject propertySlot = GameObject.Find("PropertySlot_" + name);
            activeSlot = int.Parse(name);

            card = (propertySlot.transform.childCount > 1) ? propertySlot.GetComponentInChildren<Card>() : null;
        }

        if(!card){
            upgradeText.text = "No property in current slot!";
            upgradeButton.interactable = false;

            currentOfferText.text = "No property available!";
            sellButton.interactable = false;

            tenantText.text = "No property available!";
            findNewTenantButton.interactable = false;

            return;
        } else if(!card.tenants){
             findNewTenantButton.interactable = true; 
        }

        ManagersBoxPanel.GetComponent<ToggleGroup>().allowSwitchOff = true;

        foreach(Transform manager in ManagersBoxPanel.transform){
            manager.transform.GetChild(1).GetComponent<Toggle>().isOn = false;

            if(manager.name == card.assignedManager){
                manager.transform.GetChild(1).GetComponent<Toggle>().isOn = true;
            }
        }

        ManagersBoxPanel.GetComponent<ToggleGroup>().allowSwitchOff = false;

        UpgradeText();
        TenantsText();
        SellPropertyText();
        UpdatePropertyDetailsText();
    }

    public void AssignManager(GameObject manager){
        //Selected Manager is currently assigned to this property, un-assign them.
        if(manager.name == card.assignedManager){
            card.ChangeBonus("manager_bonus", 1.00f);
            manager.transform.GetChild(2).gameObject.SetActive(false);
            card.assignedManager = "";
            UpdatePropertyDetailsText();
            return;
        }

        //Assign the new, selected manager
        card.assignedManager = manager.name;
        card.ChangeBonus("manager_bonus", (manager.GetComponent<Manager>().bonusAmount));
        manager.transform.GetChild(2).gameObject.SetActive(true);

        UpdatePropertyDetailsText();
    }

    public void UpdatePropertyDetailsText(){
        string tenants = (!card.tenants) ? "No" : "Yes";
        string man = (card.assignedManager == "") ? "No" : "Yes";
        string slot = (card.bonuses["panel_bonus"] == 0.00f) ? "None" : "%" + ((card.bonuses["panel_bonus"] - 1) * 100).ToString("F2");
        string managerBonus = (card.assignedManager == "") ? "None" : "%" + ((GameObject.Find(card.assignedManager).GetComponent<Manager>().bonusAmount - 1 )* 100).ToString("F2");
        propertyDetailsText.text = "Tenants: " + tenants + "\nManager: " + man + "\n\nValue\n$" + card.cost.ToString("#,##0") + "\n\nBase Rent\n$" + card.baseRent.ToString("#,##0") + "\n\nActual Rent\n$" + card.rent.ToString() + "\n\n-=Bonuses=-\n\nSlot: " + slot + "\nManager: " + managerBonus + "\n\n-=Current Tennants=-\n\nTerm: " + card.tenantTermRemaining + "/" + card.tenantTerm + "\nRisk: Low\n\nPower Usage\n" + card.powerUse.ToString("#,##0") + " KWH";
    }

    public void PurchaseUpgrade(){
        if (gameManager.money >= cost){
            //Deduct the money
            gameManager.AddMoney(-cost, "Upgrade Purchase");

            //Apply the upgrade
            card.baseRent += rentUpgradeAmount;
            
            //Increase the value of the property
            card.cost += Mathf.FloorToInt(card.cost * 0.28f);
            
            //Update the cards text
            card.UpdateRent();

            UpgradeText();
            SellPropertyText();
            UpdatePropertyDetailsText();

            gameManager.GameStats["MoneySpentOnUpgrades"] += cost;
            gameManager.GameStats["TotalUpgrades"]++;

            //check for high score
            if(card.rent > gameManager.GameStats["HighestRental"]) gameManager.GameStats["HighestRental"] = card.rent;
            if(card.cost > gameManager.GameStats["MostExpensiveProperty"]){
                gameManager.GameStats["MostExpensiveProperty"] = card.cost;
            }
        }else{
            upgradeText.text = "Not enough money!";
        }
    }
 
    public void SellProperty(){
        gameManager.AddMoney(currentOffer, "Property Sale");

        card.transform.parent.GetComponent<BuyPanel>().openPropertySlotButton.GetComponent<Image>().sprite = gameManager.normal;
        card.transform.parent.GetComponent<BuyPanel>().openPropertySlotButton.GetComponent<Image>().color = Color.white;

        card.Destroy();

        gameManager.GameStats["TotalPropertiesSold"]++;
        OpenPropertyPanel("none");
    }
  
    void UpgradeText(){
        rentUpgradeAmount = Mathf.FloorToInt(card.baseRent * 0.05f);
        cost = Mathf.FloorToInt(card.cost * card.upgradeMultiplier);

        upgradeText.text = "Upgrade Cost: $" + cost.ToString("#,##0") + "\n($"+card.baseRent+") -> $" + (card.baseRent + rentUpgradeAmount).ToString("#,##0");

        upgradeButton.interactable = true;
    }
  
    void SellPropertyText(){
        currentOffer = ValueProperty();
        currentOfferText.text = "Property Value: "+ card.cost.ToString("#,##0") +"\n\n Current Offer: $" + currentOffer.ToString("#,##0");
        sellButton.interactable = true;
    }
  
    int ValueProperty(){
        int baseValue = card.cost;
        int value = Mathf.FloorToInt(baseValue * gameManager.supplyDemandIndex);
        return value;
    }

    public void FindTenant(){
        card.tenants = true;
        card.tenantTerm = Random.Range(1, 7) * 3;
        card.tenantTermRemaining = card.tenantTerm;
        card.tenantMoveInWeek = gameManager.week;

        gameManager.GameStats["TotalNumberTenants"] += Random.Range(1, 5);

        findNewTenantButton.interactable = false;

        card.transform.parent.GetComponent<BuyPanel>().openPropertySlotButton.GetComponent<Image>().sprite = gameManager.normal;
        card.transform.parent.GetComponent<BuyPanel>().openPropertySlotButton.GetComponent<Image>().color = Color.white;

        TenantsText();
        UpdatePropertyDetailsText();

        gameManager.tenancyTexts[activeSlot - 1].text = card.tenantTerm.ToString();
    }

    void TenantsText(){
        tenantText.text = "Current Tenants: " + card.tenants + "\n\nLease Term: "+ card.tenantTermRemaining + "/" + card.tenantTerm + " Months \n\n Monthly Power:" + card.powerUse;
    }
}
