using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler{

    public Transform parentToReturnTo = null;
    GameManager gm;

    private void Start(){
        gm = GetComponent<Card>().gm;
    }

    public void OnBeginDrag(PointerEventData eventData){

        if (gm.sounds){
            gm.ambientSource.PlayOneShot(gm.pickUpCard);
        }

        parentToReturnTo = transform.parent;
        transform.SetParent(transform.parent.parent);

        if (parentToReturnTo.name == "Buy Panel"){
            gm.AddRent(-Mathf.FloorToInt(GetComponent<Card>().rent * float.Parse(parentToReturnTo.tag)));
        }

        if(parentToReturnTo.name == "Sell Panel"){
            parentToReturnTo.GetComponent<SellPanel>().ClearOffer();
        }

        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData){

        if (Input.GetMouseButton(1)){
            return;
        }

        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData){
        transform.SetParent(parentToReturnTo);

        if(GetComponent<Card>().purchased && parentToReturnTo.name == "Buy Panel"){
            gm.AddRent(Mathf.FloorToInt(GetComponent<Card>().rent * float.Parse(parentToReturnTo.tag)));
        }

        if(parentToReturnTo.name == "Sell Panel"){
            StartCoroutine(parentToReturnTo.GetComponent<SellPanel>().GenerateOffer());
        }

        GetComponent<CanvasGroup>().blocksRaycasts = true;

        if (gm.sounds){
            int x = Random.Range(0, gm.placeCardSounds.Length);
            gm.ambientSource.PlayOneShot(gm.placeCardSounds[x]);
        }
    }
}
