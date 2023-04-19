using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampChecker : MonoBehaviour
{
    public bool lampPull;
    

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "lever_trigger")
        {
            lampPull = true;
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.name == "lever_trigger")
        {
            lampPull = false;
        }
    }
}
