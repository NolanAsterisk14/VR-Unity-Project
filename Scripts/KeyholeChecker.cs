using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyholeChecker : MonoBehaviour
{
    public bool keyInserted;
    [SerializeField]
    private bool triggerPress;
    private bool triggerHeldOnEnter;
    private GameObject bookKey;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        keyInserted = false;
        triggerHeldOnEnter = false;
        if (this.gameObject.name == "Secret_Keyhole")
        {
            bookKey = this.transform.Find("book_key_turn").gameObject;
            bookKey.SetActive(false);
        }
        if (this.gameObject.name == "Cabinet_Keyhole")
        {
            bookKey = this.transform.Find("10_key_turn").gameObject;
            bookKey.SetActive(false);
        }
        anim = bookKey.GetComponent<Animator>();
    }

    void Update()
    {
        triggerPress = Input.GetKey("joystick button 14");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Player Has Entered Collision");
            if (triggerPress == true)
            {
                triggerHeldOnEnter = true;
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && this.gameObject.name == "Secret_Keyhole")
        {
            if (triggerHeldOnEnter == false && triggerPress == true && other.GetComponent<Pickups>().inventory.Contains("book_key") == true)
            {
                Debug.Log("You inserted the Key!");
                keyInserted = true;
                bookKey.SetActive(true);
                StartCoroutine(Delay());
            }
        }
            if (other.tag == "Player" && this.gameObject.name == "Cabinet_Keyhole")
            {
                Debug.Log("Player is staying inside collision");
                if (triggerHeldOnEnter == false && triggerPress == true && other.GetComponent<Pickups>().inventory.Contains("10_key") == true)
                {
                    Debug.Log("You inserted the Key!");

                    bookKey.SetActive(true);
                    StartCoroutine(Delay());
                }
            }
            if (other.tag == "Player" && this.gameObject.name == "Door_Keyhole")
            {
                if (triggerHeldOnEnter == false && triggerPress == true && other.GetComponent<Pickups>().inventory.Contains("silver_key") == true)
                {
                    Debug.Log("You inserted the Key!");

                    bookKey.SetActive(true);
                    StartCoroutine(Delay());
                }
            }
        if (triggerPress == false)
            {
                triggerHeldOnEnter = false;
            }
        
    }

    void OnTriggerExit(Collider other)
    {
        triggerHeldOnEnter = false;
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(2f);
        anim.SetTrigger("TurnKey");
        if (this.gameObject.name == "Cabinet_Keyhole")
                {
                    yield return new WaitForSeconds(3f);
                    GameObject parent1 = this.transform.parent.gameObject;
                    GameObject parent2 = parent1.transform.parent.gameObject;
                    GameObject parent3 = parent2.transform.parent.gameObject;
                    parent3.transform.Find("CabinetTrigger").gameObject.SetActive(true);
                }

    }
}
