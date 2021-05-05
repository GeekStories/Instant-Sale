using UnityEngine;
using UnityEngine.UI;

public class UpgradePanel : MonoBehaviour{

    public GameManager gm;

    public Text showText;

    Card c;

    int upgradeCost = 0;
    int addedRent = 0;

    public void FillUI(Card t){
        c = t;

        addedRent = Random.Range(11, 23) + (c.cost / 1000) - c.rentBuffer;
        //upgradeCost = Mathf.RoundToInt(c.cost * c.upgradeMultiplier);

        switch (Mathf.FloorToInt(Mathf.Log10(upgradeCost))){
            case 3: //[1],000
                showText.text = "Cost: " + "$" + upgradeCost.ToString()[0] + "K" +
                                " Rent: " + "+$" + addedRent.ToString();
                break;
            case 4: //[10],000
                showText.text = "Cost: " + "$" + upgradeCost.ToString()[0] + upgradeCost.ToString()[1] + "K" +
                                " Rent: " + "+$" + addedRent.ToString();
                break;
            case 5: //[100],000
                showText.text = "Cost: " + "$" + upgradeCost.ToString()[0] + upgradeCost.ToString()[1] + upgradeCost.ToString()[2] + "K" +
                                " Rent: " + "+$" + addedRent.ToString();
                break;
            case 6: //[1],000,000
                showText.text = "Cost: " + "$" + upgradeCost.ToString()[0] + "M" +
                                " Rent: " + "+$" + addedRent.ToString();
                break;
            case 7: //[10],000,000
                showText.text = "Cost: " + "$" + upgradeCost.ToString()[0] + upgradeCost.ToString()[1] + "M" +
                                " Rent: " + "+$" + addedRent.ToString();
                break;
            default: //[100],000,000
                showText.text = "Cost: " + "$" + upgradeCost.ToString()[0] + upgradeCost.ToString()[1] + upgradeCost.ToString()[2] + "M" +
                                " Rent: " + "+$" + addedRent.ToString();
                break;
        }
    }

    public void Upgrade(){
        if (gm.sounds)
            gm.ambientSource.PlayOneShot(gm.buttonClick);

        if (transform.childCount == 0) {
            showText.text = "No card! Place a card in the upgrade box!";
            return;
        }else{
            //Upgrade the tile
            if (gm.money >= upgradeCost){

                if (gm.sounds){
                    int x = Random.Range(0, gm.upgradeSounds.Length);
                    gm.ambientSource.PlayOneShot(gm.upgradeSounds[x], 0.2f);
                }

                c.rent += addedRent;
                c.cost += Mathf.FloorToInt(upgradeCost * 0.25f);
                c.transform.GetChild(1).GetComponent<Text>().text = c.Calculate(c.cost, c.rent);

                c.rentBuffer += Mathf.FloorToInt(addedRent * 0.25f);

                gm.AddMoney(-upgradeCost);

                //c.upgradeMultiplier += 0.005f;
                c.cost += Mathf.FloorToInt(upgradeCost * 0.25f);

                gm.GameStats["TotalMoneySpent"] += upgradeCost;
                gm.GameStats["MoneySpentOnUpgrades"] += upgradeCost;
                gm.GameStats["TotalUpgrades"]++;

                upgradeCost = 0;
                addedRent = 0;

                FillUI(c);
            }
        }
    }
}
