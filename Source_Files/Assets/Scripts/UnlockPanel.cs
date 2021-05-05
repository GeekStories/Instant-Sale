using UnityEngine;
using UnityEngine.UI;

public class UnlockPanel : MonoBehaviour{

    public GameManager gm;
    public int cost;

    public Sprite locked;
    public Sprite unlocked;

    public void Start(){
        InvokeRepeating("CheckMoney", 0, 0.5f);
    }

    public void CheckMoney(){
        if (gm.money >= cost){
            GetComponent<Image>().sprite = unlocked;
        }else{
            GetComponent<Image>().sprite = locked;
        }
    }

    public void Unlock(){
        if (gm.money >= cost){
            gm.AddMoney(-cost);
            gm.GameStats["TotalMoneySpent"] += cost;
            transform.parent.GetComponent<GridLayoutGroup>().cellSize = new Vector2(200, 300);
            transform.parent.GetComponent<GridLayoutGroup>().enabled = true;

            transform.parent.transform.GetChild(0).gameObject.SetActive(false);

            if (gm.sounds){
                gm.ambientSource.PlayOneShot(gm.buttonClick);
                gm.ambientSource.PlayOneShot(gm.buySellProperty);
            }
        }
    }
}
