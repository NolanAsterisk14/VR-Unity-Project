using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SecurityKeypad : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> keys = new List<GameObject>(); //Assign these in the inspector!
    [SerializeField]
    private string typedNums;
    [SerializeField]
    public bool unlocked = false;
    [SerializeField]
    private Button numField; //Assign this in the inspector!
    [SerializeField]
    private Button incorrectText; //Assign this in the inspector!
    [SerializeField]
    private Material redLightEvans;
    private Material redLightGrey;
    private Material redLightHunt;
    [SerializeField]
    private Material greenLightEvans;
    private Material greenLightGrey;
    private Material greenLightHunt;
    
    void Start()
    {
        redLightEvans = Resources.Load<Material>("RedLightEvans");
        redLightGrey = Resources.Load<Material>("RedLightGrey");
        redLightHunt = Resources.Load<Material>("RedLightHunt");
        greenLightEvans = Resources.Load<Material>("GreenLightEvans");
        greenLightGrey = Resources.Load<Material>("GreenLightGrey");
        greenLightHunt = Resources.Load<Material>("GreenLightHunt");
        redLightEvans.EnableKeyword("_EMISSION");
        redLightGrey.EnableKeyword("_EMISSION");
        redLightHunt.EnableKeyword("_EMISSION");
        greenLightEvans.DisableKeyword("_EMISSION");
        greenLightGrey.DisableKeyword("_EMISSION");
        greenLightHunt.DisableKeyword("_EMISSION");
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
        switch (this.transform.parent.gameObject.name)
        {
            case "Security_PanelEvans":
                if (typedNums == "6120")
                {
                    unlocked = true;
                    redLightEvans.DisableKeyword("_EMISSION");
                    greenLightEvans.EnableKeyword("_EMISSION");
                }
                else
                {
                    StartCoroutine(wrongCode());
                }
                break;
            case "Security_PanelGrey":
                if (typedNums == "0451")
                {
                    unlocked = true;
                    redLightGrey.DisableKeyword("_EMISSION");
                    greenLightGrey.EnableKeyword("_EMISSION");
                }
                else
                {
                    StartCoroutine(wrongCode());
                }
                break;
            case "Security_PanelHunt":
                if (typedNums == "8392")
                {
                    unlocked = true;
                    redLightHunt.DisableKeyword("_EMISSION");
                    greenLightHunt.EnableKeyword("_EMISSION");
                }
                else
                {
                    StartCoroutine(wrongCode());
                }
                break;
            default:
                StartCoroutine(wrongCode());
                Debug.Log("You didn't assign a name to the security panel!!");
                break;
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
}
