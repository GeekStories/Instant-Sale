using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievmentsManager : MonoBehaviour {

    public Image[] navButtonImages;
    public Button[] claimButton;
    public GameObject[] panels;
    public Text[] text;

    int[] rewards = {
        50000, //Rent
        25000, //Owned Properties
        15250, //Sold Properties
        5000, //Passed Properties
        16500 //Upgrades
    };

    int[] requirements = {
        1000, //Rent
        3, //Owned Properties
        5, //Sold Properties
        15, //Passed Properties
        15 //Upgrades
    };

    GameObject activePanel;
    GameManager gm;

    private void Start(){
        //Grab the game manager
        gm = GetComponent<GameManager>();
        activePanel = panels[0];
    }

    public void ChangePanel(int p){
        if (activePanel == panels[p]){
            UpdatePanel(p);
            return;
        }else{
            activePanel.SetActive(false);
        }

        activePanel = panels[p];
        activePanel.SetActive(true);

        UpdatePanel(p);
    }

    void UpdatePanel(int p){
        switch (p){
            case 0: //Rent
                if (gm.rent >= requirements[0])
                    claimButton[0].interactable = true;

                text[0].text = "\n" + "$" + gm.rent.ToString("#,##0") + " / $" + requirements[0].ToString("#,##0") + "\n" +
                               "Reward: $" + rewards[0].ToString("#,##0");
                break;
            case 1: //Owned Properties
                if (gm.GameStats["TotalPropertiesOwned"] >= requirements[1])
                    claimButton[1].interactable = true;

                text[1].text = "\n" + gm.GameStats["TotalPropertiesOwned"].ToString() + " / " + requirements[1] + "\n" +
                               "Reward: $" + rewards[1].ToString("#,##0");
                break;
            case 2: //Sold Properties
                if (gm.GameStats["TotalPropertiesSold"] >= requirements[2])
                    claimButton[2].interactable = true;

                text[2].text = "\n" + gm.GameStats["TotalPropertiesSold"].ToString() + " / " + requirements[2] + "\n" +
                               "Reward: $" + rewards[2].ToString("#,##0");
                break;
            case 3: //Passed Properties
                if (gm.GameStats["TotalPassedProperties"] >= requirements[3])
                    claimButton[3].interactable = true;

                text[3].text = "\n" + gm.GameStats["TotalPassedProperties"].ToString() + " / " + requirements[3] + "\n" +
                               "Reward: $" + rewards[3].ToString("#,##0");
                break;
            case 4: //Upgrades
                if (gm.GameStats["TotalUpgrades"] >= requirements[4])
                    claimButton[4].interactable = true;

                text[4].text = "\n" + gm.GameStats["TotalUpgrades"].ToString() + " / " + requirements[4] + "\n" +
                               "Reward: $" + rewards[4].ToString("#,##0");
                break;
            default:
                break;
        }
    }
    
    public void ClaimReward(int p){
        switch (p){
            case 0: //RENT
                gm.AddMoney(rewards[0]);
                requirements[0] += Random.Range(1000, 3500);
                rewards[0] += Random.Range(1000, 2500);
                break;
            case 1: //OWNED PROPERTIES
                gm.AddMoney(rewards[1]);
                requirements[1] += Random.Range(2, 5);
                rewards[1] += Random.Range(5000, 15000);
                break;
            case 2: //SOLD PROPERTIES
                gm.AddMoney(rewards[2]);
                requirements[2] += Random.Range(2, 4);
                rewards[2] += Random.Range(1000, 3500);
                break;
            case 3: //PASSED PROPERTIES
                gm.AddMoney(rewards[3]);
                requirements[3] += Random.Range(5, 11);
                rewards[3] += Random.Range(2750, 4000);
                break;
            case 4: //UPGRADES
                gm.AddMoney(rewards[4]);
                requirements[4] += Random.Range(8, 35);
                rewards[4] += Random.Range(1500, 4560);
                break;
            default:
                break;
        }

        UpdatePanel(p);
    }
}
