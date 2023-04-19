using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeycardUnlock : MonoBehaviour
{
    [SerializeField]
    private bool triggerpress;
    private bool triggerheldonenter = false;
    public bool useSlot = false;
    void Update()
    {
        //Get input from trigger press
        triggerpress = Input.GetKey("joystick button 14");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (triggerpress == true)
            {
                triggerheldonenter = true;
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && other.GetComponent<Pickups>().inventory.Contains("security_card") == true && triggerpress == true && triggerheldonenter == false)
        {
            useSlot = true;
        }
        if (triggerpress == false)
        {
            triggerheldonenter = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Pickup")
        {
            triggerheldonenter = false;
        }
    }
}
