using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dresser_TopDraw_Animation : MonoBehaviour
{
    Animator anim;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Player" && other.GetComponent<Pickups>().inventory.Contains("silver_key"))
        {
            anim.SetTrigger("TopDrawOpen");
        }
    }

    void Stop()
    {
        anim.enabled = false;
    }
}
