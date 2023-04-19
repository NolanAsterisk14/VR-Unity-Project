using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Input;

public class Phonecall : MonoBehaviour, IMixedRealityPointerHandler
{
    private bool callStarted;
    public bool alarmStarted;
    public void OnPointerClicked(MixedRealityPointerEventData eventData)
    {
        return;
    }
    public void OnPointerUp(MixedRealityPointerEventData eventData)
    {
        return;
    }
    public void OnPointerDown(MixedRealityPointerEventData eventData)
    {
        if (callStarted == false)
        {
            StartCoroutine(PhoneState());
        }
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

    IEnumerator PhoneState()
    {
        yield return new WaitForSecondsRealtime(2f);
        this.gameObject.GetComponent<AudioSource>().Play();
        yield return new WaitForSecondsRealtime(5f);
        GameObject.Find("SpeakerSystemActive").GetComponent<AudioSource>().Play();
        yield return new WaitForSecondsRealtime(5f);
        GameObject.Find("SpeakerAlarm").GetComponent<AudioSource>().Play();
        alarmStarted = true;
        yield return null;
    }
}
