using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler{

    public GameManager gm;

    public void OnPointerEnter(PointerEventData eventData){
        //Add some cool border effects on the card when the pointer hovers over the card
    }

    public void OnPointerExit(PointerEventData eventData){
        //Clear the cool border effects on the card when the pointer leaves the card
    }

    public void OnDrop(PointerEventData eventData){
        if (gm.sounds){
            int x = Random.Range(0, gm.placeCardSounds.Length);
            gm.ambientSource.PlayOneShot(gm.placeCardSounds[x]);
        }

        Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
        Card c = d.GetComponent<Card>();

        if (transform.name == "Sell Panel"){
            if (transform.childCount == 1 || !c.purchased){
                return;
            }
        }

        if (d != null){
            if (transform.name == "Buy Panel"){
                if (transform.GetChild(0).gameObject.activeInHierarchy == true){ //Dropped on a locked property slot
                    return;
                }

                if (c.purchased){ //Check the player owns the property
                    if (transform.childCount == 2 && d.parentToReturnTo != transform){ //Check there isn't already a property in the slot
                        //Move the card in the destination slot to the original slot
                        Draggable card2 = transform.GetChild(1).GetComponent<Draggable>(); 
                        
                        gm.AddRent(-Mathf.FloorToInt(card2.GetComponent<Card>().rent * float.Parse(transform.tag)));
                        gm.AddRent(Mathf.FloorToInt(card2.GetComponent<Card>().rent * float.Parse(d.parentToReturnTo.tag)));

                        card2.parentToReturnTo = d.parentToReturnTo;
                        card2.transform.SetParent(card2.parentToReturnTo);
                    }
                }else{
                    if (gm.money < c.cost){
                        return;
                    }

                    c.purchased = true;

                    gm.AddMoney(-c.cost);

                    gm.GameStats["TotalPropertiesOwned"]++;
                    gm.GameStats["TotalMoneySpent"] += c.cost; 
                    
                    gm.CheckPile();

                    if (gm.sounds){
                        gm.ambientSource.PlayOneShot(gm.buySellProperty, 0.2f);
                    }       
                }
            } else if(transform.name == "Sell Panel"){
                StartCoroutine(transform.GetComponent<SellPanel>().GenerateOffer());
            }

            d.parentToReturnTo = transform;
        }
    }
}
