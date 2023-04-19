using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Microsoft.MixedReality.Toolkit.Input;

public class ButtonPushSafe : MonoBehaviour, IMixedRealityPointerHandler, IMixedRealityFocusHandler
{
    //For reference, this script copies a lot of code from the 'ButtonPush' script.
    private Transform buttonStart;
    private Vector3 startPos;
    private Vector3 movePos;
    [SerializeField]
    private float moveExtent = 0f;
    private bool transActive = false;
    private GameObject keypad;

    void Start()
    {
        buttonStart = this.transform;
        startPos = buttonStart.localPosition;
        movePos = startPos + new Vector3(0, 0, 0.018f);
        keypad = this.transform.parent.gameObject;
    }

    void Update()
    {
        this.transform.localPosition = Vector3.Lerp(startPos, movePos, moveExtent);
    }

    public void OnPointerClicked(MixedRealityPointerEventData eventData)
    {
        if (transActive == false)
        {
            StartCoroutine(lerpTransition());
            switch (this.gameObject.name)
            {
                case "0":
                case "1":
                case "2":
                case "3":
                case "4":
                case "5":
                case "6":
                case "7":
                case "8":
                case "9":
                    keypad.GetComponent<SafeKeypad>().addNums(this.gameObject.name);
                    break;
                case "Clear":
                    keypad.GetComponent<SafeKeypad>().clearNums();
                    break;
                case "Enter":
                    keypad.GetComponent<SafeKeypad>().checkNums();
                    break;
            }

        }
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

    IEnumerator lerpTransition()
    {
        transActive = true;
        for (float i = 0f; i <= 1f; i += Time.deltaTime * 4)
        {
            moveExtent = i;
            yield return null;
        }
        for (float i = 1f; i >= 0; i -= Time.deltaTime * 4)
        {
            moveExtent = i;
            yield return null;
        }
        transActive = false;
    }

}
