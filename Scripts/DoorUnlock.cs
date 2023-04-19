using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorUnlock : MonoBehaviour
{
    private Rigidbody rb; 
    // Start is called before the first frame update
    void Start()
    {
        rb = this.gameObject.transform.parent.GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && other.GetComponent<Pickups>().inventory.Contains("silver_key") == true)
        {
            rb.isKinematic = false;
            Destroy(this.gameObject);
        }
    }
}
