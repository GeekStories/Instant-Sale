using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour{

    public int cost;
    public int rent;

    public Sprite[] houses;

    public bool purchased;

    [HideInInspector] public int rentBuffer = 6;

    public int upgradeCost;

    int rentMin = 50;

   // [HideInInspector]
    public int weeksLeft;

    public Text cardText;

    public GameManager gm;
    Image spriteImage;

    private void Start(){
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();

        cardText = transform.GetChild(1).GetComponent<Text>();
        spriteImage = transform.GetChild(2).GetComponent<Image>();

        spriteImage.sprite = houses[Random.Range(0, houses.Length-1)];

        cost = Random.Range(gm.minCost, gm.maxCost);
        cost = cost % 1000 >= 500 ? cost + 1000 - cost % 1000 : cost - cost % 1000;

        rent = Random.Range(rentMin, gm.rentMax);

        cardText.text = Calculate(cost, rent);
    }

    public void Destroy(){
        Destroy(gameObject);
    }

    public string Calculate(int amnt, int rent){
        string x = "";

        //VALUE
        switch (Mathf.FloorToInt(Mathf.Log10(amnt))){
            case 3: //[1],000
                x = "Value: " + "$" + amnt.ToString()[0] + "K" +
                                " Rent: " + "+$" + rent.ToString();
                break;
            case 4: //[10],000
                x = "Value: " + "$" + amnt.ToString()[0] + amnt.ToString()[1] + "K" +
                                " Rent: " + "+$" + rent.ToString();
                break;
            case 5: //[100],000
                x = "Value: " + "$" + amnt.ToString()[0] + amnt.ToString()[1] + amnt.ToString()[2] + "K" +
                                " Rent: " + "+$" + rent.ToString();
                break;
            case 6: //[1],000,000
                x = "Value: " + "$" + amnt.ToString()[0] + "M" +
                                " Rent: " + "+$" + rent.ToString();
                break;
            case 7: //[10],000,000
                x = "Value: " + "$" + amnt.ToString()[0] + amnt.ToString()[1] + "M" +
                                " Rent: " + "+$" + rent.ToString();
                break;
            default: //[100],000,000
                x = "Value: " + "$" + amnt.ToString()[0] + amnt.ToString()[1] + amnt.ToString()[2] + "M" +
                                " Rent: " + "+$" + rent.ToString();
                break;
        }

        return x;
    }
}
