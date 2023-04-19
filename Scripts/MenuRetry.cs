using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuRetry : MonoBehaviour
{
    private Button retryButton;
    // Start is called before the first frame update
    void Start()
    {
        retryButton = this.gameObject.GetComponent<Button>();
        retryButton.onClick.AddListener(restartLevel);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void restartLevel()
    {
        GameObject.Find("SmartDevice(Clone)").GetComponent<Pickups>().inventory.Remove("File_Classified");
        StartCoroutine(ReloadLevel());
    }

    IEnumerator ReloadLevel()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Act2_Office", LoadSceneMode.Additive);
        yield return null;
    }
}
