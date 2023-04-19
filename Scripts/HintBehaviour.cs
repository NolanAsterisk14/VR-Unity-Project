using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HintBehaviour : MonoBehaviour
{
    [SerializeField]
    private GameObject playerCamera;
    private Transform cameraTransform;
    private Vector3 cameraVector;
    private Vector3 cameraVectorAdjust;
    [SerializeField]
    private GameObject hint; //you must initialize this value using the inspector!
    private Image hb;
    private Text ht1;
    private Text ht2;
    [SerializeField]
    private bool watchPlayer;
    private bool transActive;

    void Start()
    { 
        hb = hint.GetComponent<Image>(); //find visual components within hint object
        ht1 = hint.transform.Find("Text1").GetComponent<Text>();
        ht2 = hint.transform.Find("Text2").GetComponent<Text>();
        hb.color = new Color(hb.color.r, hb.color.g, hb.color.b, 0f); //set visual components completely transparent
        ht1.color = new Color(ht1.color.r, ht1.color.g, ht1.color.b, 0f);
        ht2.color = new Color(ht2.color.r, ht2.color.g, ht2.color.b, 0f);
        playerCamera = GameObject.Find("Main Camera"); //find camera reference for LookAt target
        cameraTransform = playerCamera.GetComponent<Transform>();
        if (playerCamera == null)
        {
            Debug.Log("Player not found");
        }
    }

    void Update()
    {
        cameraVector = cameraTransform.localPosition;
        if (watchPlayer == true)
        {
            hint.GetComponent<Transform>().transform.LookAt(cameraTransform, Vector3.up); //while rotation flag is true, rotation will look at player
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && transActive == false)
        {
            hint.transform.localPosition = new Vector3(cameraVector.x, cameraVector.y - 0.2f, cameraVector.z + 0.2f); //set position for component to appear relative to the camera
            watchPlayer = true; //set rotation flag true, begin transitioning, and destroy both objects afterwards
            StartCoroutine(Transition());
            Destroy(hint, 9);
            Destroy(this.gameObject, 9);
        }
    }
    
    IEnumerator Transition()
    {
        transActive = true;
        for(float i = 0f; i <= 1f; i += Time.deltaTime) //fade in visual components over 1 second
        {
            hb.color = new Color(hb.color.r, hb.color.g, hb.color.b, i); 
            ht1.color = new Color(ht1.color.r, ht1.color.g, ht1.color.b, i);
            ht2.color = new Color(ht2.color.r, ht2.color.g, ht2.color.b, i);
            yield return null;
        }
        yield return new WaitForSeconds(6);
        for (float i = 1f; i >= 0f; i -= Time.deltaTime) //fade out visual components over 1 second
        {
            hb.color = new Color(hb.color.r, hb.color.g, hb.color.b, i); 
            ht1.color = new Color(ht1.color.r, ht1.color.g, ht1.color.b, i);
            ht2.color = new Color(ht2.color.r, ht2.color.g, ht2.color.b, i);
            yield return null;
        }
        transActive = false;
    }
}