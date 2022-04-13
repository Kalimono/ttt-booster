using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SliderClick : MonoBehaviour, IPointerDownHandler {

    public void OnPointerDown (PointerEventData eventData) {
        Debug.Log (this.gameObject.name + " Was Clicked.");
    }
}
