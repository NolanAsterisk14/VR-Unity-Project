using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock_Door_Animation : MonoBehaviour
{
    [SerializeField]
    Animator anim;
    GameObject clock;
    [SerializeField]
    private float time;
    // Start is called before the first frame update
    void Start()
    {
        clock = GameObject.Find("Grandfather_Clock_Animation");
        anim = clock.GetComponent<Animator>();
        GameObject.Find("CodeBook").GetComponent<BoxCollider>().enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "AnimTrigger")
        {
            time = 5f;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "AnimTrigger")
        {
            time -= Time.deltaTime;
            if (time <= 0)
            {
                anim.SetTrigger("ClockOpen");
                GameObject.Find("CodeBook").GetComponent<BoxCollider>().enabled = true;
                Destroy(this);
            }


        }
    }
   
}
