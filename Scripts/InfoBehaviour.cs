using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoBehaviour : MonoBehaviour
{
    [SerializeField]
    private GameObject playerCamera;
    private GameObject alarmObject;
    private GameObject player;
    private Transform cameraTransform;
    private Vector3 cameraVector;
    [SerializeField]
    private GameObject hint1; //You must initialize this using the inspector!
    [SerializeField]
    private GameObject hint2; //You must initialize this using the inspector!
    [SerializeField]
    private GameObject keycard; //You must initialize this using the inpsector!
    private Text t1;
    private Text t2;
    private Image hb1;
    private Image hb2;
    [SerializeField]
    private bool watchPlayer;
    private bool transActive1;
    private bool transActive2;
    private bool computerHintUsed;
    private bool alarmHintUsed;
    private bool alarmActive;
    private bool keycardTriggered;
    public bool keycardObtained;
    public bool playerExit;
    public bool loadMenu;
    void Start()
    {
        player = GameObject.Find("VR_left_hand_final(Clone)");
        t1 = hint1.transform.Find("Text1").GetComponent<Text>(); //find visual components within hint target
        hb1 = hint1.GetComponent<Image>();
        t1.color = new Color(t1.color.r, t1.color.g, t1.color.b, 0f); //set visual components completely transparent
        hb1.color = new Color(hb1.color.r, hb1.color.g, hb1.color.b, 0f);
        if (this.gameObject.name == "Typex" || this.gameObject.name == "ExitTrigger")
        {
            t2 = hint2.transform.Find("Text1").GetComponent<Text>(); //find visual components within hint target
            hb2 = hint2.GetComponent<Image>();
            t2.color = new Color(t2.color.r, t2.color.g, t2.color.b, 0f); //set visual components completely transparent
            hb2.color = new Color(hb2.color.r, hb2.color.g, hb2.color.b, 0f);
        }
        if (this.gameObject.name == "Typex")
        {
            Image hb = GameObject.Find("OfficeTransition").GetComponent<Image>();
            Text t = GameObject.Find("OfficeTransition").transform.Find("Text1").GetComponent<Text>();
            t.color = new Color(t.color.r, t.color.g, t.color.b, 0f);
            hb.color = new Color(hb.color.r, hb.color.g, hb.color.b, 0f);
        }
        if (this.gameObject.name == "AlarmHintTrigger")
        {
            alarmObject = GameObject.Find("SpeakerAlarm");
        }
        playerCamera = GameObject.Find("Main Camera"); //find camera reference for LookAt target
        cameraTransform = playerCamera.GetComponent<Transform>();
        if (playerCamera == null)
        {
            Debug.Log("Player not found");
        }
    }

    void Update()
    {
        if (this.gameObject.name == "Typex")
        {
            keycardObtained = player.GetComponent<Pickups>().inventory.Contains("security_card");
        }
        cameraVector = cameraTransform.localPosition;
        if (watchPlayer == true)
        {
            hint1.GetComponent<Transform>().transform.LookAt(cameraTransform, Vector3.up); //while rotation flag is true, rotation will look at player
            if (this.gameObject.name == "Typex" || this.gameObject.name == "ExitTrigger")
            {
                hint2.GetComponent<Transform>().transform.LookAt(cameraTransform, Vector3.up);
            }
        }
        if (this.gameObject.name == "AlarmHintTrigger" && alarmHintUsed == false)
        {
            if (alarmObject.GetComponent<AlarmSystem>().alarmActive == true)
            {
                hint1.transform.localPosition = new Vector3(cameraVector.x, cameraVector.y - 0.2f, cameraVector.z + 0.2f); //set position for component to appear relative to the camera
                watchPlayer = true; //set rotation flag true, begin transitioning, and destroy both objects afterwards
                StartCoroutine(Transition());
                alarmHintUsed = true;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (this.gameObject.name == "DoorTrigger" && transActive1 == false)
        {
            if (other.tag == "Player" && other.GetComponent<Pickups>().inventory.Contains("silver key") == false)
            {
                hint1.transform.localPosition = new Vector3(cameraVector.x, cameraVector.y - 0.2f, cameraVector.z + 0.2f); //set position for component to appear relative to the camera
                watchPlayer = true; //set rotation flag true, begin transitioning, and destroy both objects afterwards
                StartCoroutine(Transition());
            }
        }
        if (this.gameObject.name == "Typex" && transActive1 == false && transActive2 == false)
        {
            if (other.tag == "Player" && other.GetComponent<Pickups>().inventory.Contains("CodeBook") == true && keycardTriggered == false)
            {
                keycard.SetActive(true);
                hint1.transform.localPosition = new Vector3(cameraVector.x, cameraVector.y - 0.2f, cameraVector.z + 0.2f); //set position for component to appear relative to the camera
                watchPlayer = true; //set rotation flag true, begin transitioning, and destroy both objects afterwards
                StartCoroutine(Transition());
                keycardTriggered = true;
            }
            if (other.tag == "Player" && other.GetComponent<Pickups>().inventory.Contains("CodeBook") == true && keycardObtained == true)
            {
                hint1 = GameObject.Find("OfficeTransition");
                t1 = hint1.transform.Find("Text1").GetComponent<Text>(); //find visual components within hint target
                hb1 = hint1.GetComponent<Image>();
                t1.color = new Color(t1.color.r, t1.color.g, t1.color.b, 0f); //set visual components completely transparent
                hb1.color = new Color(hb1.color.r, hb1.color.g, hb1.color.b, 0f);
                hint1.transform.localPosition = new Vector3(cameraVector.x, cameraVector.y - 0.2f, cameraVector.z + 0.2f); //set position for component to appear relative to the camera
                watchPlayer = true; //set rotation flag true, begin transitioning, and destroy both objects afterwards
                StartCoroutine(Transition());

            }
            if (other.tag == "Player" && other.GetComponent<Pickups>().inventory.Contains("CodeBook") == false)
            {
                hint2.transform.localPosition = new Vector3(cameraVector.x, cameraVector.y - 0.2f, cameraVector.z + 0.2f); //set position for component to appear relative to the camera
                watchPlayer = true; //set rotation flag true, begin transitioning, and destroy both objects afterwards
                StartCoroutine(Transition2());
            }
        }
        if (this.gameObject.name == "ExitTrigger" && transActive1 == false && transActive2 == false)
        {
            if (other.tag == "Player" && GameObject.Find("SpeakerAlarm").GetComponent<AlarmSystem>().alarmActive == true)
            {
                hint1.transform.localPosition = new Vector3(cameraVector.x, cameraVector.y - 0.2f, cameraVector.z + 0.2f); //set position for component to appear relative to the camera
                watchPlayer = true; //set rotation flag true, begin transitioning, and destroy both objects afterwards
                StartCoroutine(Transition());
            }
            if (other.tag == "Player" && GameObject.Find("SpeakerAlarm").GetComponent<AlarmSystem>().alarmActive == false)
            {
                hint2.transform.localPosition = new Vector3(cameraVector.x, cameraVector.y - 0.2f, cameraVector.z + 0.2f); //set position for component to appear relative to the camera
                watchPlayer = true; //set rotation flag true, begin transitioning, and destroy both objects afterwards
                StartCoroutine(Transition2());
                playerExit = true;
            }
        }
        if (this.gameObject.name == "ComputerHintTrigger" && transActive1 == false)
        {
            if (computerHintUsed == false)
            {
                hint1.transform.localPosition = new Vector3(cameraVector.x, cameraVector.y - 0.2f, cameraVector.z + 0.2f); //set position for component to appear relative to the camera
                watchPlayer = true; //set rotation flag true, begin transitioning, and destroy both objects afterwards
                StartCoroutine(Transition());
                computerHintUsed = true;
            }
        }
        
    }
    
    IEnumerator Transition()
    {
        transActive1 = true;
        for (float i = 0f; i < 1f; i += Time.deltaTime) //fade in visual components over 1 second
        {
            t1.color = new Color(t1.color.r, t1.color.g, t1.color.b, i);
            hb1.color = new Color(hb1.color.r, hb1.color.g, hb1.color.b, i);
            yield return null;
        }
        yield return new WaitForSeconds(2);
        for (float i = 1f; i > 0f; i -= Time.deltaTime) //fade out visual components over 1 second
        {
            t1.color = new Color(t1.color.r, t1.color.g, t1.color.b, i);
            hb1.color = new Color(hb1.color.r, hb1.color.g, hb1.color.b, i);
            yield return null;
        }
        hb1.color = new Color(hb1.color.r, hb1.color.g, hb1.color.b, 0);
        watchPlayer = false;
        transActive1 = false;
        if (this.gameObject.name == "Typex" && keycardObtained == true)
        {
            hint1 = GameObject.Find("OfficeTransition");
            StartCoroutine(Level2Transition());
        }
    }

    IEnumerator Transition2()
    {
        transActive2 = true;
        for (float i = 0f; i < 1f; i += Time.deltaTime) //fade in visual components over 1 second
        {
            t2.color = new Color(t2.color.r, t2.color.g, t2.color.b, i);
            hb2.color = new Color(hb2.color.r, hb2.color.g, hb2.color.b, i);
            yield return null;
        }
        yield return new WaitForSeconds(4);
        for (float i = 1f; i > 0f; i -= Time.deltaTime) //fade out visual components over 1 second
        {
            t2.color = new Color(t2.color.r, t2.color.g, t2.color.b, i);
            hb2.color = new Color(hb2.color.r, hb2.color.g, hb2.color.b, i);
            yield return null;
        }
        hb2.color = new Color(hb2.color.r, hb2.color.g, hb2.color.b, 0);
        watchPlayer = false;
        transActive2 = false;
        if (playerExit == true)
        {
            loadMenu = true;
        }
    }

    IEnumerator Level2Transition()
    {
        watchPlayer = true;
        transActive1 = true;
        for (float i = 0f; i < 1f; i += Time.deltaTime) //fade in visual components over 1 second
        {
            t1.color = new Color(t1.color.r, t1.color.g, t1.color.b, i);
            hb1.color = new Color(hb1.color.r, hb1.color.g, hb1.color.b, i);
            yield return null;
        }
        yield return null;
    }

    
}