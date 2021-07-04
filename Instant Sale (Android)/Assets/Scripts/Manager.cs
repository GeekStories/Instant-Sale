using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour{
    public GameManager gameManager;

    public int weeklyPay;
    public int hireCost;

    public bool hired = false;
    public string description = "Manager Details\n";

    public float bonusAmount;

    public Image checkmarkImage;

    public void SelectMe(){
       // if(!transform.GetChild(0).GetComponent<Toggle>().isOn) return; //Only trigger when the  manager was toggled ON
        if(transform.parent.parent.parent.parent.tag == "PropertyManagerBox"){
            transform.parent.parent.parent.parent.GetComponent<PropertyPanel>().AssignManager(gameObject);
        }else{
            gameManager.SelectManager(gameObject);
        }
    }
}
