using System.Collections;
using System.Collections.Generic;
using UnityEngine;





public class Cabinet_Animation : MonoBehaviour
{
    Animator anim;
    private GameObject silverKey;

    // Start is called before the first frame update
    void Start()
    {
        anim = GameObject.Find("Cabinet_Animation").GetComponent<Animator>();
        silverKey = GameObject.Find("silver_key");
        silverKey.GetComponent<SphereCollider>().enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
       
        if (other.tag == "Player" && other.GetComponent<Pickups>().inventory.Contains("10_key"))
        {
            anim.SetTrigger("OpenCabinet");
            silverKey.GetComponent<SphereCollider>().enabled = true;
        }
    }
}
