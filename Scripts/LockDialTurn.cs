using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Input;


public class LockDialTurn : MonoBehaviour, IMixedRealityPointerHandler, IMixedRealityFocusHandler
{
    

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClicked(MixedRealityPointerEventData eventData)
    {
        this.gameObject.transform.Rotate(0.0f, 36.0f, 0.0f, Space.Self);
        return;
    }
    public void OnPointerUp(MixedRealityPointerEventData eventData)
    {
        return;
    }
    public void OnPointerDown(MixedRealityPointerEventData eventData)
    {
        return;
    }
    public void OnPointerDragged(MixedRealityPointerEventData eventData)
    {
        return;
    }
    public void OnFocusEnter(FocusEventData eventData)
    {
        return;
    }
    public void OnFocusExit(FocusEventData eventData)
    {
        return;
    }
}

