using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SellPanel : MonoBehaviour{

    public GameManager gm;
    public Text sellText;

    public IEnumerator GenerateOffer(){
        sellText.text = "Generating Offer...";
        yield return new WaitForSeconds(0.25f);
        Card c = transform.GetChild(0).gameObject.GetComponent<Card>();

        if (c.purchased){
            int x = Mathf.FloorToInt(c.cost * gm.sellBuffer);

            switch (Mathf.FloorToInt(Mathf.Log10(x))){
                case 3: //[1],000
                    sellText.text = "+" + "$" + x.ToString()[0] + "K";
                    break;
                case 4: //[10],000
                    sellText.text = "+" + "$" + x.ToString()[0] + x.ToString()[1] + "K";
                    break;
                case 5: //[100],000
                    sellText.text = "+" + "$" + x.ToString()[0] + x.ToString()[1] + x.ToString()[2] + "K";
                    break;
                case 6: //[1],000,000
                    sellText.text = "+" + "$" + x.ToString()[0] + "M";
                    break;
                case 7: //[10],000,000
                    sellText.text = "+" + "$" + x.ToString()[0] + x.ToString()[1] + "M";
                    break;
                default: //[100],000,000
                    sellText.text = "+" + "$" + x.ToString()[0] + x.ToString()[1] + x.ToString()[2] + "M";
                    break;
            }
        }
    }

    public void ClearOffer(){
        sellText.text = "Place a property to generate an offer!";
    }

    public void SellProperty(){
        if (gm.sounds){
            gm.ambientSource.PlayOneShot(gm.buttonClick);
        }

        if (transform.childCount == 0){
            sellText.text = "No card! Place a card you own in the sell box!";
            return;
        }

        Card c = transform.GetChild(0).gameObject.GetComponent<Card>();

        gm.AddMoney(Mathf.FloorToInt(c.cost * gm.sellBuffer));
        gm.GameStats["TotalPropertiesSold"]++;

        c.Destroy();
        sellText.text = "+$0K";

        if (gm.sounds){
            gm.ambientSource.PlayOneShot(gm.buySellProperty);
        }
    }
}
