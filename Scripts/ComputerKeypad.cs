using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComputerKeypad : MonoBehaviour
{
    [SerializeField]
    private Image background;
    [SerializeField]
    private Image loadScreen;
    [SerializeField]
    private Image desktop;
    [SerializeField]
    private List<Button> keys = new List<Button>();
    [SerializeField]
    private Button OS;
    [SerializeField]
    private Button numField;
    [SerializeField]
    private Button incorrectText;
    [SerializeField]
    private Button welcomeMessage;
    [SerializeField]
    private Button loadingMessage;
    [SerializeField]
    private string typedNums;
    [SerializeField]
    private bool unlocked = false;
    [SerializeField]
    private string computerName;

    void OnEnable()
    {
        foreach (Button key in keys)
        {
            key.GetComponent<Button>().onClick.AddListener(() => KeypadEval(keys.IndexOf(key)));
        }
        computerName = this.gameObject.transform.parent.parent.name;
        Text t1 = welcomeMessage.transform.GetChild(0).GetComponent<Text>();
        Image tb1 = welcomeMessage.GetComponent<Image>();
        Text t2 = loadingMessage.transform.GetChild(0).GetComponent<Text>();
        Image tb2 = loadingMessage.GetComponent<Image>();
        Image bg = background.GetComponent<Image>();
    }

   void KeypadEval(int keyNum)
    {
        if (keyNum >= 0 && keyNum <= 9)
        {
            typedNums += keyNum.ToString();
            numField.transform.GetChild(0).GetComponent<Text>().text = typedNums;
        }
        if (keyNum == 10)
        {
            switch (computerName)
            {
                case "ComputerWalker":
                    if (typedNums == "070432")
                    {
                        UnlockComputer();
                    }
                    else
                    {
                        Debug.Log("Incorrect Number Entered");
                        StartCoroutine(ElementPause());
                    }
                    break;
                case "ComputerHunt":
                    if (typedNums == "080392")
                    {
                        UnlockComputer();
                    }
                    else
                    {
                        StartCoroutine(ElementPause());
                    }
                    break;
                case "ComputerGrey":
                    if (typedNums == "123456")
                    {
                        UnlockComputer();
                    }
                    else
                    {
                        StartCoroutine(ElementPause());
                    }
                    break;
                default:
                    Debug.Log("Whoopsie, you didn't name the computer for the keypad script correctly. Check the script!");
                    break;
            }
        }
        if (keyNum == 11)
        {
            typedNums = null;
            numField.transform.GetChild(0).GetComponent<Text>().text = typedNums;
        }
    }

    void UnlockComputer()
    {
        foreach (Button key in keys)
        {
            key.gameObject.SetActive(false);
        }
        numField.gameObject.SetActive(false);
        StartCoroutine(ElementFade());
        this.gameObject.transform.parent.parent.tag = "Scannable";
    }

    IEnumerator ElementFade()
    {
        Text t1 = welcomeMessage.transform.GetChild(0).GetComponent<Text>();
        Image tb1 = welcomeMessage.GetComponent<Image>();
        Text t2 = loadingMessage.transform.GetChild(0).GetComponent<Text>();
        Image tb2 = loadingMessage.GetComponent<Image>();
        Image bg = loadScreen.GetComponent<Image>();
        Image ls = background.GetComponent<Image>();
        t1.color = new Color(t1.color.r, t1.color.g, t1.color.b, 0);
        tb1.color = new Color(tb1.color.r, tb1.color.g, tb1.color.b, 0);
        t2.color = new Color(t2.color.r, t2.color.g, t2.color.b, 0);
        tb2.color = new Color(tb2.color.r, tb2.color.g, tb2.color.b, 0);
        welcomeMessage.gameObject.SetActive(true);
        loadingMessage.gameObject.SetActive(true);
        for (float i = 1f; i >= 0f; i -= Time.deltaTime) //fade out visual components over 1 second
        {
            ls.color = new Color(ls.color.r, ls.color.g, ls.color.b, i);
            yield return null;
        }
        for (float i = 0f; i <= 1f; i += Time.deltaTime) //fade in visual components over 1 second
        {
            t1.color = new Color(t1.color.r, t1.color.g, t1.color.b, i);
            tb1.color = new Color(tb1.color.r, tb1.color.g, tb1.color.b, i);
            yield return null;
        }
        yield return new WaitForSecondsRealtime(1f);
        for (float i = 0f; i <= 1f; i += Time.deltaTime) //fade in visual components over 1 second
        {
            t2.color = new Color(t2.color.r, t2.color.g, t2.color.b, i);
            tb2.color = new Color(tb2.color.r, tb2.color.g, tb2.color.b, i);
            yield return null;
        }
        yield return new WaitForSecondsRealtime(1f);
        OS.gameObject.SetActive(false);
        for (float i = 1f; i >= 0f; i -= Time.deltaTime) //fade out visual components over 1 second
        {
            t1.color = new Color(t1.color.r, t1.color.g, t1.color.b, i);
            tb1.color = new Color(tb1.color.r, tb1.color.g, tb1.color.b, i);
            t2.color = new Color(t2.color.r, t2.color.g, t2.color.b, i);
            tb2.color = new Color(tb2.color.r, tb2.color.g, tb2.color.b, i);
            bg.color = new Color(bg.color.r, bg.color.g, bg.color.b, i);
            yield return null;
        }
        t1.color = new Color(t1.color.r, t1.color.g, t1.color.b, 0);
        tb1.color = new Color(tb1.color.r, tb1.color.g, tb1.color.b, 0);
        t2.color = new Color(t2.color.r, t2.color.g, t2.color.b, 0);
        tb2.color = new Color(tb2.color.r, tb2.color.g, tb2.color.b, 0);
        bg.color = new Color(bg.color.r, bg.color.g, bg.color.b, 0);
    }
    
    IEnumerator ElementPause()
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
