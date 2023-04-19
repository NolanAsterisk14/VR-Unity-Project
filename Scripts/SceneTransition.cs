using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Microsoft.MixedReality.Toolkit.Input;

public class SceneTransition : MonoBehaviour
{
    private Button startButton;
    private GameObject[] allObjects;
    private GameObject alarmObject;
    [SerializeField]
    private GameObject outtroScreen; //Initialize this in the inspector!
    [SerializeField]
    private GameObject creditsScreen; //Initialize this in the inspector!
    private bool triggerpress1;
    private bool triggerpress2;
    private bool keycardObtained;
    private bool loadMainMenu;

    void Start()
    {
        allObjects = GameObject.FindGameObjectsWithTag("StartScene");
        if (this.gameObject.name == ("SceneOneButton")) //Check for main menu, add listener for button, get objects to disable
        {
            startButton = this.gameObject.GetComponent<Button>();
            startButton.onClick.AddListener(loadScene);
        }
        if (this.gameObject.name == "ExitTrigger")
        {
            alarmObject = GameObject.Find("SpeakerAlarm");
        }
    }

    void Update() //Watch for input from triggers
    {
        triggerpress1 = Input.GetKey("joystick button 15");
        triggerpress2 = Input.GetKey("joystick button 14");
        if (this.gameObject.name == "Typex")
        {
            keycardObtained = this.gameObject.GetComponent<InfoBehaviour>().keycardObtained;
        }
        if (this.gameObject.name == "ExitTrigger" && alarmObject.GetComponent<AlarmSystem>().playerExit == true)
        {
            StartCoroutine(loadEndState());
        }
        if (this.gameObject.name == "ExitTrigger")
        {
            loadMainMenu = this.gameObject.GetComponent<InfoBehaviour>().loadMenu;
        }
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && this.gameObject.name == "Typex") //This is the check for the Typex machine, which if true will load level 2
        {
            if (other.GetComponent<Pickups>().inventory.Contains("CodeBook") == true && keycardObtained == true)
            {
                StartCoroutine(loadSecondSceneAsync());
            }
        }
    }
    void loadScene() //Function for listener on main menu, loads level 1
    {
        if (this.gameObject.name == ("SceneOneButton"))
        {
            StartCoroutine(loadFirstSceneAsync());
        }
    }

    IEnumerator loadFirstSceneAsync() //Coroutine for loading level one. 
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("House", LoadSceneMode.Additive); //Loads level in background, while intro text activates in menu scene. Pause until player presses triggers
        asyncLoad.allowSceneActivation = false;
        yield return new WaitForSeconds(2f);
        while (triggerpress1 == false && triggerpress2 == false)
        {
            yield return null;
        }
        asyncLoad.allowSceneActivation = true;
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        if (asyncLoad.isDone) //When player presses triggers, create variable and assign empty gameobject's position to act as new start position for player. 
        {                     //Then, set UI canvas as a child of playspace. Move playspace, set new scene as active, and deactivate old objects.
            GameObject canvas = GameObject.Find("UICanvas");
            Transform startMarker = GameObject.Find("PlayerStartPos").GetComponent<Transform>();
            Vector3 playerAdjust = startMarker.localPosition;
            GameObject playSpace = GameObject.Find("MixedRealityPlayspace");
            playSpace.transform.localPosition = new Vector3(playerAdjust.x, playerAdjust.y, playerAdjust.z);
            playSpace.transform.rotation = startMarker.rotation;
            //canvas.transform.rotation = startMarker.rotation;     (I've commented these out because they don't work right, but I might want to test with them later)
            //canvas.transform.localPosition = new Vector3(playerAdjust.x, playerAdjust.y, playerAdjust.z);
            canvas.transform.SetParent(playSpace.transform, false);
            canvas.transform.localPosition = new Vector3(0, 0, 0);
            SceneManager.SetActiveScene(SceneManager.GetSceneByName("House"));
            foreach (GameObject a in allObjects) //The reason we don't deactivate the old scene entirely is it acts as the permanent scene containing the playspace
            {
                a.SetActive(false);
            }
        }
    }

    IEnumerator loadSecondSceneAsync() //Similar process to above, except we can now simply disable the old scene as none of the objects from that scene are needed
    {
        yield return new WaitForSecondsRealtime(4f);
        Debug.Log("Loading Second Scene");
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Act2_Office", LoadSceneMode.Additive);
        asyncLoad.allowSceneActivation = false;
        while (triggerpress1 == false && triggerpress2 == false)
        {
            yield return null;
        }
        asyncLoad.allowSceneActivation = true;
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        if (asyncLoad.isDone)
        {
            GameObject canvas = GameObject.Find("UICanvasOffice");
            Transform startMarker = GameObject.Find("PlayerStartPos2").GetComponent<Transform>();
            Vector3 playerAdjust = startMarker.localPosition;
            GameObject playSpace = GameObject.Find("MixedRealityPlayspace");
            playSpace.transform.localPosition = new Vector3(playerAdjust.x, playerAdjust.y, playerAdjust.z);
            playSpace.transform.rotation = startMarker.rotation;
            canvas.transform.SetParent(playSpace.transform, false);
            canvas.transform.localPosition = new Vector3(0, 0, 0);
            AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync("House");
            GameObject.Find("OfficeTransition").SetActive(false);
            SceneManager.SetActiveScene(SceneManager.GetSceneByName("Act2_Office"));
        }
    }

    IEnumerator loadEndState()
    {
        while (loadMainMenu == false)
        {
            yield return null;
        }
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("MainMenu"));
        foreach (GameObject a in allObjects)
        {
            a.SetActive(true);
        }
        Transform startMarker = GameObject.Find("PlayerStartPos3").GetComponent<Transform>();
        Vector3 playerAdjust = startMarker.localPosition;
        GameObject playSpace = GameObject.Find("MixedRealityPlayspace");
        playSpace.transform.localPosition = new Vector3(playerAdjust.x, playerAdjust.y, playerAdjust.z);
        playSpace.transform.rotation = startMarker.rotation;
        outtroScreen.SetActive(true);
        yield return new WaitForSecondsRealtime(2f);
        while (triggerpress1 == false && triggerpress2 == false)
        {
            yield return null;
        }
        creditsScreen.SetActive(true);
        yield return new WaitForSecondsRealtime(2f);
        while (triggerpress1 == false && triggerpress2 == false)
        {
            yield return null;
        }
        outtroScreen.SetActive(false);
        creditsScreen.SetActive(false);
    }
}