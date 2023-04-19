using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bookcase_Animation : MonoBehaviour
{
    Animator anim;
    private GameObject lamp;
    private GameObject keyhole;
    // Start is called before the first frame update
    void Start()
    {
        anim = GameObject.Find("Secret_Bookcase").GetComponent<Animator>();
        lamp = GameObject.Find("secret_wall_light_lever");
        keyhole = GameObject.Find("Secret_Keyhole");
    }

    void Update()
    {
        if (keyhole.GetComponent<KeyholeChecker>().keyInserted == true && lamp.GetComponent<LampChecker>().lampPull == true)
        {
            anim.SetTrigger("OpenBookcase");
            Destroy(this);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            anim.SetTrigger("OpenDoor");
        }
    }
}
