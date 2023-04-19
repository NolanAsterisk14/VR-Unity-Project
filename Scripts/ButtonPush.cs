using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Input;

public class ButtonPush : MonoBehaviour, IMixedRealityPointerHandler, IMixedRealityFocusHandler
{
    private Transform buttonStart;
    [SerializeField]
    private GameObject cover;
    [SerializeField]
    private Quaternion startCoverRotation;
    [SerializeField]
    private Quaternion targetCoverRotation;
    [SerializeField]
    private Quaternion currentCoverRotation;
    private Vector3 startPos;
    private Vector3 movePos;
    [SerializeField]
    private float moveExtent = 0f;
    [SerializeField]
    private float rotateExtent = 0f;
    private bool transActive = false;
    [SerializeField]
    private bool coverOpened = false;
    private bool alarmActive;
    private GameObject keypad;
    private GameObject alarmObject;
    // Start is called before the first frame update
    void Start()
    {
        buttonStart = this.transform;
        startPos = buttonStart.localPosition;
        movePos = startPos + new Vector3(0, 0, 0.018f);
        cover = this.transform.parent.parent.Find("Cover").gameObject;
        startCoverRotation = cover.transform.localRotation;
        targetCoverRotation = cover.transform.localRotation * Quaternion.Euler(0, 0, 140f);
        keypad = this.transform.parent.gameObject;
        alarmObject = GameObject.Find("SpeakerAlarm");
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.localPosition = Vector3.Lerp(startPos, movePos, moveExtent);
        cover.transform.localRotation = Quaternion.RotateTowards(cover.transform.localRotation, targetCoverRotation, rotateExtent);
        currentCoverRotation = cover.transform.rotation;
        alarmActive = alarmObject.GetComponent<AlarmSystem>().alarmActive;
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
                    if (alarmActive == true)
                    {
                        keypad.GetComponent<SecurityKeypad>().addNums(this.gameObject.name);
                    }
                    break;
                case "Clear":
                    if (alarmActive == true)
                    {
                        keypad.GetComponent<SecurityKeypad>().clearNums();
                    }
                    break;
                case "Enter":
                    if (alarmActive == true)
                    {
                        keypad.GetComponent<SecurityKeypad>().checkNums();
                    }
                    break;
                case "Open":
                    if (coverOpened == false)
                    {
                        StartCoroutine(quaternionLerpTransition());
                    }
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
        for(float i = 0f; i <= 1f; i += Time.deltaTime * 4)
        {
            moveExtent = i;
            yield return null;
        }
        for(float i = 1f; i >= 0; i -= Time.deltaTime * 4)
        {
            moveExtent = i;
            yield return null;
        }
        transActive = false;
    }

    IEnumerator quaternionLerpTransition()
    {
        for(float i = 0f; i <= 1f; i += Time.deltaTime)
        {
            rotateExtent = i;
            yield return null;
        }
        coverOpened = true;
    }
}
