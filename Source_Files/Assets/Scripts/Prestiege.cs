using UnityEngine;
using UnityEngine.UI;

public class Prestiege : MonoBehaviour {

    public Text upgradesInfo;
    public Text unclaimedPointsInfo;
    public Text pointsInfo;
    public Text rentInfo;
    public Text bonusInfo;
    public Text sellPriceInfo;
    public Text autoWeeksInfo;

    int points, unclaimedPoints, rentCost, bonusCost, sellPriceCost, autoWeeksCost;

    float unclaimedPointsBuffer = 0.000000f;
    GameManager gm;

    private void Start() {
        gm = GetComponent<GameManager>();

        unclaimedPointsBuffer = gm.year / 1000000f;
        unclaimedPoints = Mathf.FloorToInt((gm.money * unclaimedPointsBuffer) * 100);

        rentCost = 1000;
        bonusCost = 1000;
        sellPriceCost = 1000;
        autoWeeksCost = 1000;

        UpdateUpgradesText();
    }

    private void Update(){
        unclaimedPointsBuffer = gm.year / 1000000f;
        unclaimedPoints = Mathf.FloorToInt(((gm.money + gm.networth) * unclaimedPointsBuffer) * 100);
        unclaimedPointsInfo.text = "Claim " + unclaimedPoints.ToString("#,##0");
    }

    void UpdateUpgradesText() {
        pointsInfo.text = points.ToString();

        upgradesInfo.text = "Rent: $" + gm.rentMax + "\n" +
                            "Bonus: " + gm.rentWeekInc + "\n" +
                            "Sell Price x " + gm.sellBuffer.ToString("0.00") + "\n" +
                            "Auto Weeks x " + gm.maxWeeksPerSecond;

        rentInfo.text = rentCost.ToString();
        bonusInfo.text = bonusCost.ToString();
        sellPriceInfo.text = sellPriceCost.ToString();
        autoWeeksInfo.text = autoWeeksCost.ToString();
    }

    public void ClaimPoints(){
        if (unclaimedPoints == 0)
            return;

        points += unclaimedPoints;
        unclaimedPoints = 0;

        //Reset the game
        gm.ResetGame();

        UpdateUpgradesText();
    }

    public void Upgrade(int x){
        switch (x){
            case 1: //Rent
                if (points < rentCost)
                    break;

                points -= rentCost;
                rentCost += Random.Range(500, 1000) + Random.Range(0, gm.year);

                gm.rentMax += 15;
                break;
            case 2: //Bonus
                if (points < bonusCost)
                    break;

                points -= bonusCost;
                bonusCost += Random.Range(500, 1000) + Random.Range(0, gm.year);

                gm.rentWeekInc += 0.0001f;
                break;
            case 3: //Sell Price
                if (points < sellPriceCost)
                    break;
                
                if(gm.sellBuffer < 0.01f){
                    gm.sellBuffer = 0.01f;
                    sellPriceInfo.GetComponentInParent<Button>().interactable = false;
                    break;
                }
                
                points -= sellPriceCost;
                sellPriceCost += Random.Range(500, 1000) + Random.Range(0, gm.year);

                gm.sellBuffer += 0.01f;
                break;
            case 4: //Auto Weeks
                if (points < autoWeeksCost)
                    break;

                points -= autoWeeksCost;
                autoWeeksCost += Random.Range(500, 1000) + Random.Range(0, gm.year);

                gm.maxWeeksPerSecond++;
                break;
        }

        UpdateUpgradesText();
    }
}
