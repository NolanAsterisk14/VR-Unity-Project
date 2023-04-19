using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SafeKeypad : MonoBehaviour
{
    //Just for reference, this script copies a lot of code from the 'SecurityKeypad' script.
    [SerializeField]
    private List<GameObject> keys = new List<GameObject>(); //Assign these in the inspector!
    [SerializeField]
    private string typedNums;
    [SerializeField]
    public bool unlocked = false;
    [SerializeField]
    private bool code1 = false;
    [SerializeField]
    private bool code2 = false;
    [SerializeField]
    private bool keycardInserted = false;
    private bool transActive = false;
    [SerializeField]
    private bool doorOpened = false;
    private Vector3 startPos;
    private Vector3 movePos;
    private Color startColor;
    private Color endColor;
    private float fadeExtent = 0f;
    private float moveExtent = 0f;
    private float rotateExtent = 0f;
    [SerializeField]
    private GameObject door;
    [SerializeField]
    private Quaternion startDoorRotation;
    [SerializeField]
    private Quaternion targetDoorRotation;
    [SerializeField]
    private Quaternion currentDoorRotation;
    [SerializeField]
    private Material redLight;
    [SerializeField]
    private Material greenLight;
    [SerializeField]
    private Button code1Button; //Assign everything here and below in the inspector!
    [SerializeField]
    private Button code2Button;
    [SerializeField]
    private Button keycardText;
    [SerializeField]
    private Button numField;
    [SerializeField]
    private Button incorrectText;
    [SerializeField]
    private GameObject keycardSlot;
    [SerializeField]
    private GameObject keycardMover;

    void Start()
    {
        door = this.transform.parent.gameObject;
        startDoorRotation = door.transform.localRotation;
        targetDoorRotation = door.transform.localRotation * Quaternion.Euler(0, -120f, 0);
        startPos = keycardMover.transform.localPosition;
        movePos = startPos - new Vector3(0, 0.16f, 0);
        startColor = new Color(0.5395603f, 0.9150943f, 0.6168762f);
        endColor = new Color(0.2669989f, 0.4528302f, 0.3041652f);
        redLight = Resources.Load<Material>("RedLightSafe");
        greenLight = Resources.Load<Material>("GreenLightSafe");
        redLight.EnableKeyword("_EMISSION");
        greenLight.DisableKeyword("_EMISSION");
    }

    void Update()
    {
        door.transform.localRotation = Quaternion.RotateTowards(door.transform.rotation, targetDoorRotation, rotateExtent);
        currentDoorRotation = door.transform.localRotation;
        keycardMover.transform.localPosition = Vector3.Lerp(startPos, movePos, moveExtent);
        keycardInserted = keycardSlot.GetComponent<KeycardUnlock>().useSlot;
        keycardText.transform.GetChild(0).GetComponent<Text>().color = Color.Lerp(startColor, endColor, fadeExtent);
    }

    public void addNums(string keyName)
    {
        typedNums += keyName;
        numField.transform.GetChild(0).GetComponent<Text>().text = typedNums;
    }

    public void clearNums()
    {
        typedNums = null;
        numField.transform.GetChild(0).GetComponent<Text>().text = typedNums;
    }

    public void checkNums()
    {
        if (typedNums == "322484")
        {
            code1 = true;
            code1Button.GetComponent<Image>().color = new Color(0.094f, 0.925f, 0.039f);
        }
        if (typedNums == "0233")
        {
            code2 = true;
            code2Button.GetComponent<Image>().color = new Color(0.094f, 0.925f, 0.039f);
        }
        if (code1 == true && code2 == true)
        {
            StartCoroutine(keycardState());
        }
        if (typedNums != "322484" && typedNums != "0233")
        {
            StartCoroutine(wrongCode());
        }
    }

    IEnumerator wrongCode()
    {
        numField.gameObject.SetActive(false);
        incorrectText.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(0.5f);
        incorrectText.gameObject.SetActive(false);
        yield return new WaitForSecondsRealtime(0.5f);
        incorrectText.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(0.5f);
        incorrectText.gameObject.SetActive(false);
        numField.gameObject.SetActive(true);
    }

    IEnumerator keycardState()
    {
        numField.gameObject.SetActive(false);
        keycardText.gameObject.SetActive(true);
        while (keycardInserted == false)
        {
            for (float i = 0; i <= 1; i += Time.deltaTime)
            {
                fadeExtent = i;
                if (keycardInserted == true)
                {
                    goto StartAnim;
                }
                yield return null;
            }
            for (float i = 1; i >= 0; i -= Time.deltaTime)
            {
                fadeExtent = i;
                if (keycardInserted == true)
                {
                    goto StartAnim;
                }
                yield return null;
            }
            yield return null;
        }
        StartAnim:
        if (keycardInserted == true)
        {
            keycardText.gameObject.SetActive(false);
            keycardMover.SetActive(true);
            for (float i = 0f; i <=1f; i += Time.deltaTime * 3)
            {
                moveExtent = i;
                yield return null;
            }
            unlocked = true;
            this.transform.parent.parent.transform.Find("File_Classified").gameObject.GetComponent<Collider>().enabled = true;
            redLight.DisableKeyword("_EMISSION");
            greenLight.EnableKeyword("_EMISSION");
            yield return new WaitForSecondsRealtime(2f);
            StartCoroutine(quaternionLerpTransition());
        }
    }

    IEnumerator quaternionLerpTransition()
    {
        for (float i = 0f; i <= 1f; i += Time.deltaTime)
        {
            rotateExtent = i;
            yield return null;
        }
        doorOpened = true;
    }
}
