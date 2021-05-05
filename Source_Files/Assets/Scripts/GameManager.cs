using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public Text moneyText;
    public Text rentText;
    public Text scoreText;
    public Text yearsText;
    public Text weeksPerSecondText;

    public int money;
    public int rent;
    public int score;
    public int cardsLeft;
    public int cardsLeftMax;
    public int year;
    public int week;
    public int networth;
    public int addMoneyAmnt;
    public int weeksPerSecond;
    public int maxWeeksPerSecond = 0;

    public int minCost, maxCost, rentMax = 150;

    int nextUpgrade = 5;

    public float sellBuffer = 0.25f;
    public float rentBonus;
    public float rentWeekInc;

    public GameObject[] buyPanels;

    public Transform CardPile;
    public GameObject card;
    public GameObject sellPanel;

    public Canvas tutorial;

    public Text weeksLeft;
    public Text cardsLeftText;

    public Button musicToggle;
    public Button soundToggle;
    public Button mainSubmit;

    public Sprite musicOn;
    public Sprite musicOff;
    public Sprite soundOn;
    public Sprite soundOff;

    public AudioClip[] backgroundMusic;
    public AudioClip[] upgradeSounds;
    public AudioClip[] placeCardSounds;

    public AudioClip pickUpCard;
    public AudioClip buySellProperty;
    public AudioClip buttonClick;

    public AudioSource ambientSource;
    public AudioSource source;

    public bool music = false;
    public bool sounds = false;

    bool repeatingWeek = false;

    public Dictionary<string, int> GameStats = new Dictionary<string, int>(){
        { "TotalPropertiesOwned", 0},
        { "TotalMoneySpent", 0},
        { "TotalPropertiesSold", 0},
        { "MoneySpentOnUpgrades", 0},
        { "TotalPassedProperties", 0},
        { "TotalUpgrades", 0}
    };

    public void Start() {
        source = GetComponent<AudioSource>();

        BackgroundMusic();

        money = 100000;
        networth = money;
        rent = 0;
        cardsLeftMax = 5;
        cardsLeft = cardsLeftMax;
        year = 0;

        NextWeek();

        InvokeRepeating("CalculateNetWorth", 0, 0.5f);
        StartCoroutine(RepeatingWeeks());
    }

    public void BackgroundMusic() {
        CancelInvoke("BackgroundMusic");

        int x = Random.Range(0, 2);

        if (music)
            source.PlayOneShot(backgroundMusic[x], 0.1f);

        InvokeRepeating("BackgroundMusic", backgroundMusic[x].length, backgroundMusic[x].length);
    }

    //Increase the amount of rent gained per year
    public void AddRent(int amnt) {
        rent += amnt;
        rentText.text = "+$" + rent.ToString("#,##0") + " p/w" + "\n" + " (+$" + (rent * rentBonus).ToString("F2") + " bonus) ";
    }

    public void CheckPile() {
        if (CardPile.childCount == 1) { //Are there any cards currently in the pile?
            Card c = CardPile.GetChild(0).GetComponent<Card>(); //Grab that card
            c.weeksLeft--;

            //Check if we have any weeks left before expiry
            if (c.weeksLeft > 0)
                weeksLeft.text = "Expires in " + c.weeksLeft.ToString() + " weeks";
            else{
                c.Destroy(); //No weeks left, destroy the card
                CreateCard();
            }
      
        } else //No card, so generate one!
            CreateCard();
    }

    public void CreateCard(){
        if (cardsLeft > 0){ //Do we have any left in the pile?
            GameObject x = Instantiate(card, CardPile); //Yes! Generate a new card!
            x.GetComponent<Card>().weeksLeft = Random.Range(3, 10);
            weeksLeft.text = "Expires in " + x.GetComponent<Card>().weeksLeft.ToString() + " weeks";

            cardsLeft--;
            cardsLeftText.text = cardsLeft.ToString() + "/" + cardsLeftMax.ToString();
        } else { 
            weeksLeft.text = "Out of cards!";
        }
    }

    //Add x to the players money
    public void AddMoney(int amount) {
        money += amount;
        moneyText.text = "$" + money.ToString("#,##0");
    }

    public void CalculateNetWorth() {
        int x = money;

        foreach (GameObject item in buyPanels) {
            if (item.transform.childCount != 0 && item.transform.GetChild(0).gameObject.activeInHierarchy == false && item.transform.childCount == 2) {
                Card c = item.GetComponentInChildren<Card>();
                x += Mathf.FloorToInt(c.cost * sellBuffer);
            }
        }

        networth = x;
        scoreText.text = "NetWorth" + "\n" + "$" + networth.ToString("#,##0");
    }

    //Move on to the next year in the game
    public void NextWeek() {
        if (sounds)
            ambientSource.PlayOneShot(buttonClick);

        if (week == 52) {
            year++;
            rentBonus += rentWeekInc;

            rentText.text = "+$" + rent.ToString("#,##0") + " p/w" + "\n" + " (+$" + (rent * rentBonus).ToString("F2") + " bonus) ";

            week = 0;
        } else {
            week++;
        }

        minCost = Random.Range(Random.Range(8500, 12500), Random.Range(15000, 35000));
        maxCost = Random.Range(Random.Range(36000, 50000), Random.Range(51000, 155000));

        //Every 5 years, you'll gain 1 extra card per week!
        if (year == nextUpgrade) {
            nextUpgrade += 5;
            cardsLeftMax++;
        }

        GameStats["NetWorth"] = networth;
        GameStats["Money"] = money;

        yearsText.text = "W" + week.ToString() + " : " + "Y" + year.ToString();

        AddMoney(Mathf.RoundToInt(rent * (rentBonus + 1)));

        cardsLeft = cardsLeftMax;

        CheckPile();
    }

    IEnumerator RepeatingWeeks(){
        if (weeksPerSecond > 0){
            yield return new WaitForSeconds((150 / 100 / weeksPerSecond));
            NextWeek();
        }

        yield return new WaitForSeconds((0.07f));
        StartCoroutine(RepeatingWeeks());
    }

    public void IncreaseWeeksPerSecond(){
        if(weeksPerSecond < maxWeeksPerSecond){
                weeksPerSecond++;
                weeksPerSecondText.text = weeksPerSecond.ToString();
        }
    }

    public void DecreaseWeeksPerSecond() {
        if (weeksPerSecond > 0) { 
            weeksPerSecond--;
            weeksPerSecondText.text = weeksPerSecond.ToString();
        }
    }

    public void PassCard() {
        if (CardPile.childCount > 0) {
            CardPile.GetChild(0).GetComponent<Card>().Destroy();

            CardPile.DetachChildren();
            GameStats["TotalPassedProperties"]++;

            if (cardsLeft > 0)
                CheckPile();           
            else
                weeksLeft.text = "Out of cards!";          
        }
    }

    public void OpenTutorial() {
        if (sounds)
            ambientSource.PlayOneShot(buttonClick);

        tutorial.gameObject.SetActive(!tutorial.gameObject.activeInHierarchy);
    }

    int HighestValueProperty() {
        int x = 0;

        foreach (GameObject item in buyPanels) {
            if (item.transform.childCount != 0 && item.transform.GetChild(0).name != "Unlock") {
                Card c = item.GetComponentInChildren<Card>();

                if (c.cost > x)
                    x = c.cost;
            }
        }

        return x;
    }

    IEnumerator SubmitScoreDelay() {
        yield return new WaitForSeconds(2.0f);

        mainSubmit.interactable = true;
    }

    public void ToggleSound() {
        sounds = !sounds;

        if (sounds) 
            soundToggle.GetComponent<Image>().sprite = soundOn;
        else 
            soundToggle.GetComponent<Image>().sprite = soundOff;
        
        if (sounds)
            ambientSource.PlayOneShot(buttonClick);
    }

    public void ToggleMusic() {
        music = !music;

        if (music) {
            musicToggle.GetComponent<Image>().sprite = musicOn;
            BackgroundMusic();
        } else {
            musicToggle.GetComponent<Image>().sprite = musicOff;
            source.Stop();
        }

        if (sounds)
            ambientSource.PlayOneShot(buttonClick);
    }

    public void ResetGame() {
        //Clearing properties and resetting buy panels
        foreach (GameObject panel in buyPanels) {
            if (panel.transform.childCount == 2)
                panel.transform.GetChild(1).GetComponent<Card>().Destroy();

            GameObject lockObj = panel.transform.GetChild(0).gameObject;

            panel.GetComponent<GridLayoutGroup>().enabled = false;

            if (!lockObj.activeInHierarchy)
                lockObj.SetActive(true);            
        }
        Debug.Log("PANELS ARE RESET");

        //Reset the first buy panel
        GameObject buyPanel1 = buyPanels[0];

        if (buyPanel1.transform.childCount == 2)
            buyPanel1.transform.GetChild(1).GetComponent<Card>().Destroy();

        buyPanel1.GetComponent<GridLayoutGroup>().enabled = true;
        buyPanel1.transform.GetChild(0).gameObject.SetActive(false);
        Debug.Log("FIRST PANEL IS RESET");

        //Clear Sell panel
        if (sellPanel.transform.childCount == 1)
            sellPanel.transform.GetChild(0).GetComponent<Card>().Destroy();

        Debug.Log("SELL PANEL IS RESET");

        //Resetting variables
        AddMoney(-money);
        AddMoney(100000);
        Debug.Log("MONEY SET TO 0");

        AddRent(-rent);
        rentBonus = 0;
        Debug.Log("RENT SET TO 0");
        Debug.Log("RENT BONUS SET TO 0");

        networth = 0;
        scoreText.text = "Net Worth" + "\n" + "$" + networth.ToString("#,##0");
        Debug.Log("NETWORTH SET TO 0");

        //Reset the weeks per second slider
        weeksPerSecond = 0;
        Debug.Log("WEEKS PER SECOND SET TO 0");

        //Reset the card stack
        cardsLeftMax = 5;
        cardsLeft = cardsLeftMax;
        cardsLeftText.text = cardsLeft + "/" + cardsLeftMax;
        Debug.Log("CARDS AMNT INDICATOR RESET");

        //Reset the years and weeks
        year = 0;
        week = 0;
        yearsText.text = "W" + week + " : Y" + year;
        Debug.Log("WEEKS/YEARS RESET");

        //Remove the card from the card pile
        if (CardPile.transform.childCount == 1){
            CardPile.transform.GetChild(0).GetComponent<Card>().Destroy();
            CardPile.DetachChildren();
        }
        Debug.Log("CLEARED CARD SLOT");

        //Generate a new card
        NextWeek();
    }
}
