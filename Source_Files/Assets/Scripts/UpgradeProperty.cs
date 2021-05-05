using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeProperty : MonoBehaviour{

    public GameManager gm;
    public GameObject mySlot;
    public Text text;

    public Sprite close;
    public Sprite info;

    public Image button;

    Card card;

    int cost, rent;

    private void Start(){
        gm = mySlot.GetComponent<DropZone>().gm;
    }

    public void OpenCloseUpgradePanel(){
        if (!mySlot.transform.GetChild(0).gameObject.activeInHierarchy && mySlot.transform.childCount > 1){
            gameObject.SetActive(!gameObject.activeInHierarchy);
        }

        if (gameObject.activeInHierarchy){
            card = mySlot.transform.GetChild(1).GetComponent<Card>();
            UpdateText();
            button.sprite = close;
        }else{
            button.sprite = info;
        }
    }

    public void ConfirmUpgrade(){
        if (card != null && gm.money >= cost){
            float x = card.rent;

            //If the property is located in a bonus slot then calculate that
            x *= float.Parse(card.GetComponent<Draggable>().parentToReturnTo.transform.tag);

            gm.AddRent(-Mathf.FloorToInt(x)); //Just deduct the rent
            
            card.rent += rent;

            x = card.rent;
            //If the property is located in a bonus slot then calculate that
            x *= float.Parse(card.GetComponent<Draggable>().parentToReturnTo.transform.tag);

            gm.AddRent(Mathf.FloorToInt(x)); //Just add the rent
         
            //Increase the value of the property
            card.cost += Random.Range(cost / 2, cost);

            //Update the property cards text
            card.cardText.text = card.Calculate(card.cost, card.rent);

            //Deduct the money
            gm.AddMoney(-cost);

            gm.GameStats["TotalUpgrades"]++;
            UpdateText();
        }else{
            text.text = "Not enough money!";
        }
    }

    void UpdateText(){
        cost = Mathf.FloorToInt(card.cost * 0.28f);
        rent = Mathf.FloorToInt(card.rent * 0.15f);

        switch (Mathf.FloorToInt(Mathf.Log10(cost))){
            case 3: //[1],000
                text.text = "Cost: " + "$" + cost.ToString()[0] + "K" +
                                " Rent: " + "+$" + rent.ToString();
                break;
            case 4: //[10],000
                text.text = "Cost: " + "$" + cost.ToString()[0] + cost.ToString()[1] + "K" +
                                " Rent: " + "+$" + rent.ToString();
                break;
            case 5: //[100],000
                text.text = "Cost: " + "$" + cost.ToString()[0] + cost.ToString()[1] + cost.ToString()[2] + "K" +
                                " Rent: " + "+$" + rent.ToString();
                break;
            case 6: //[1],000,000
                text.text = "Cost: " + "$" + cost.ToString()[0] + "M" +
                                " Rent: " + "+$" + rent.ToString();
                break;
            case 7: //[10],000,000
                text.text = "Cost: " + "$" + cost.ToString()[0] + cost.ToString()[1] + "M" +
                                " Rent: " + "+$" + rent.ToString();
                break;
            default: //[100],000,000
                text.text = "Cost: " + "$" + cost.ToString()[0] + cost.ToString()[1] + cost.ToString()[2] + "M" +
                                " Rent: " + "+$" + rent.ToString();
                break;
        }
    }

    int Round() {
        return cost % 1000 >= 500 ? cost + 1000 - cost % 1000 : cost - cost % 1000;
    }
}
