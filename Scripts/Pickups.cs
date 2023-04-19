using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pickups : MonoBehaviour
{
    [SerializeField]
    private bool triggerpress;
    private bool triggerheldonenter = false;
    [SerializeField]
    public List<string> inventory = new List<string> ();
    [SerializeField]
    private GameObject itemObject;
    [SerializeField]
    private GameObject smartDevice;
    private Text text;
    private MonoBehaviour outline;

    void Update()
    {
        //Get input from trigger press
         triggerpress = Input.GetKey("joystick button 14");
    }

    void OnTriggerEnter(Collider other)
    {
        //putting instance reference search here since onenable was causing issues
        smartDevice = GameObject.Find("SmartDevice(Clone)");
        itemObject = smartDevice.transform.Find("Canvas/Background/ItemsList/ContentViewport/ItemContent").gameObject;
        text = itemObject.GetComponent<Text>();
        //if object is collectable, make sure trigger isn't already pressed
        if (other.tag == "Pickup")
        {
            outline = other.GetComponent("MeshOutline") as MonoBehaviour;
            outline.enabled = true;
            if (triggerpress == true)
            {
                triggerheldonenter = true;
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        //if object is collectable, and trigger isn't already pressed, pressing the trigger will add it to inventory and destroy it
        if (other.tag == "Pickup" && triggerpress == true && triggerheldonenter == false)
        {
            if(inventory.Contains(other.name) == false)
            {
                string temp = other.name;
                temp.Replace('_', ' ');
                text.text += "• " + temp + Environment.NewLine;
                inventory.Add(other.name);
            }
                Destroy(other.gameObject);
            
        }
        if (triggerpress == false)
        {
            triggerheldonenter = false;
        }
    }

    void OnTriggerExit (Collider other)
    {
        //reset flag for trigger being held on exit
        if (other.tag == "Pickup")
        {
            outline.enabled = false;
            triggerheldonenter = false;
        }
    }
}
