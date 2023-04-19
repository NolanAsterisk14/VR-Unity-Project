using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkerPhone : MonoBehaviour
{
    private bool fileObtained;
    private bool soundStarted;
    private GameObject hand;
    private AudioSource ringSource;
    // Start is called before the first frame update
    void Start()
    {
        ringSource = this.gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hand == null)
        {
            hand = GameObject.Find("VR_left_hand_final(Clone)");
        }
        fileObtained = hand.GetComponent<Pickups>().inventory.Contains("File_Classified");
        if (fileObtained == true && soundStarted == false)
        {
            StartSound();
        }
    }

    void StartSound()
    {
        ringSource.Play();
        soundStarted = true;
    }

}
