using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;// Required when using Event data.

public class mouseHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler// required interface when using the OnPointerEnter method.
{
    [SerializeField] GameObject pic1,pic2;
    //Do this when the cursor enters the rect area of this selectable UI object.
    public void OnPointerEnter(PointerEventData eventData)
    {
        pic1.SetActive(false);
        pic2.SetActive(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        pic2.SetActive(false);
        pic1.SetActive(true);
    }

}