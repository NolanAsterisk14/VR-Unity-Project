using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmSystem : MonoBehaviour
{
    [SerializeField]
    public bool alarmActive;
    private bool alarmTriggered;
    private bool rotateLights;
    private bool panelUnlocked1;
    private bool panelUnlocked2;
    private bool panelUnlocked3;
    public bool gameOver;
    public bool playerExit;
    [SerializeField]
    private float lightSpeed = 1f;
    private GameObject phone;
    private GameObject securityPanel1;
    private GameObject securityPanel2;
    private GameObject securityPanel3;
    private GameObject alarmObject;
    [SerializeField]
    private GameObject exitArea; //Initialize this using the inspector!
    private GameObject exitTrigger;
    private List<GameObject> alarmLights = new List<GameObject>();
    private List<GameObject> lights = new List<GameObject>();
    [SerializeField]
    private GameObject flourescentLights; //Initialize this using the inspector!
    private Material alarmEmission;
    private Material flourescentEmission;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            alarmLights.Add(GameObject.Find("AlarmLight" + i));
        }
        foreach (GameObject light in alarmLights)
        {
            lights.Add(light.transform.Find("light1").gameObject);
            lights.Add(light.transform.Find("light2").gameObject);
        }
        alarmEmission = Resources.Load<Material>("RedLightAlarm");
        flourescentEmission = Resources.Load<Material>("LightEmission");
        alarmEmission.DisableKeyword("_EMISSION");
        flourescentEmission.EnableKeyword("_EMISSION");
        phone = GameObject.Find("walker_s_phone").transform.Find("phone_1").gameObject;
        securityPanel1 = GameObject.Find("Security_PanelHunt").transform.Find("Keys").gameObject;
        securityPanel2 = GameObject.Find("Security_PanelGrey").transform.Find("Keys").gameObject;
        securityPanel3 = GameObject.Find("Security_PanelEvans").transform.Find("Keys").gameObject;
        alarmObject = this.gameObject;
        exitTrigger = exitArea.transform.Find("ExitTrigger").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        panelUnlocked1 = securityPanel1.GetComponent<SecurityKeypad>().unlocked;
        panelUnlocked2 = securityPanel2.GetComponent<SecurityKeypad>().unlocked;
        panelUnlocked3 = securityPanel3.GetComponent<SecurityKeypad>().unlocked;
        playerExit = exitTrigger.GetComponent<InfoBehaviour>().playerExit;
        if (alarmTriggered == false)
        {
            alarmActive = phone.GetComponent<Phonecall>().alarmStarted;
        }
        if (alarmTriggered == true)
        {
            if (panelUnlocked1 == true && panelUnlocked2 == true && panelUnlocked3 == true)
            {
                alarmActive = false;
            }
        }
        if (alarmActive == true && alarmTriggered == false)
        {
            AlarmState();
        }
    }

    void AlarmState()
    {
        alarmTriggered = true;
        exitArea.SetActive(true);
        StartCoroutine(AlarmLights());
        StartCoroutine(AlarmTimer());
    }

    IEnumerator AlarmLights()
    {
        foreach (GameObject light in lights)
        {
            light.SetActive(true);
        }
        rotateLights = true;
        alarmEmission.EnableKeyword("_EMISSION");
        flourescentEmission.DisableKeyword("_EMISSION");
        flourescentLights.SetActive(false);
        StartCoroutine(RotateLights());
        while (alarmActive == true)
        {
            yield return new WaitForSecondsRealtime(2f);
            for (float i = 1f; i > 0f; i -= Time.deltaTime)
            {
                foreach (GameObject light in lights)
                {
                    light.GetComponent<Light>().intensity = i * 10f;
                }
                yield return null;
            }
            yield return new WaitForSecondsRealtime(2f);
            for (float i = 0f; i < 1f; i += Time.deltaTime)
            {
                foreach (GameObject light in lights)
                {
                    light.GetComponent<Light>().intensity = i * 10f;
                }
                yield return null;
            }
            yield return null;
        }
        alarmObject.GetComponent<AudioSource>().Stop();
        foreach (GameObject light in lights)
        {
            light.SetActive(false);
        }
        rotateLights = false;
        alarmEmission.DisableKeyword("_EMISSION");
        flourescentEmission.EnableKeyword("_EMISSION");
        flourescentLights.SetActive(true);
        GameObject.Find("SpeakerSystemInactive").GetComponent<AudioSource>().Play();
        StartCoroutine(ExitState());
    }

    IEnumerator RotateLights()
    {
        while (rotateLights == true)
        {
            foreach (GameObject light in lights)
            {
                light.transform.Rotate(Vector3.right * (lightSpeed * Time.deltaTime));
            }
            yield return null;
        }
    }

    IEnumerator AlarmTimer()
    {
        while (alarmActive == true)
        {
            float time = 0f;
            if (time <= 300f)
            {
                time += Time.deltaTime;
            }

            if (time > 300f)
            {
                gameOver = true;
                StartCoroutine(LossState());
                break;
            }

            if (playerExit == true)
            {
                break;
            }
            yield return null;
        }
        yield return null;
    }

    IEnumerator LossState()
    {
        exitArea.SetActive(false);
        GameObject LoseCanvas = GameObject.Find("GameOverCanvas");
        GameObject Camera = GameObject.Find("Main Camera");
        Vector3 CV = Camera.transform.localPosition;
        LoseCanvas.transform.localPosition = new Vector3(CV.x, CV.y - 0.2f, CV.z + 0.5f);
        yield return null;
    }

    IEnumerator ExitState()
    {
        GameObject exitLight = exitArea.transform.Find("ExitHighlight").gameObject;
        exitLight.SetActive(true);
        while (playerExit == false)
        {
            yield return new WaitForSecondsRealtime(1f);
            for (float i = 1f; i > 0f; i -= Time.deltaTime)
            {
                exitLight.GetComponent<Light>().intensity = i;
                yield return null;
            }
            yield return new WaitForSecondsRealtime(1f);
            for (float i = 0f; i < 1f; i += Time.deltaTime)
            {
                exitLight.GetComponent<Light>().intensity = i;
                yield return null;
            }
            yield return null;
        }
        exitLight.SetActive(false);
    }
}
