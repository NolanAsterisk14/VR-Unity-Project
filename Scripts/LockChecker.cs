using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockChecker : MonoBehaviour
{
    private bool isUnlocked;
    [SerializeField]
    private List<string> combination = new List<string>();
    private GameObject lockBody;
    private FixedJoint bodyJoint;
    private GameObject lockLoop;
    private ConfigurableJoint loopJoint;
    private GameObject boxTop;
    private ConfigurableJoint lid;
    private GameObject blueprint;
    private MonoBehaviour blueprintHandler;

    
    void Start()
    {
        isUnlocked = false;
        lockBody = GameObject.Find("Combination_Lock_Body");
        lockLoop = GameObject.Find("Lock_Top_Loop");
        boxTop = GameObject.Find("Toolbox_Lid");
        bodyJoint = lockBody.GetComponent<FixedJoint>();
        loopJoint = lockLoop.GetComponent<ConfigurableJoint>();
        lid = boxTop.GetComponent<ConfigurableJoint>();
        blueprint = GameObject.Find("Blueprint");
        blueprintHandler = blueprint.GetComponent("ManipulationHandler") as MonoBehaviour;
        blueprintHandler.enabled = false;
    }

    
    void Update()
    {
        if (combination.Contains("ComboTrigger1") && combination.Contains("ComboTrigger2") && combination.Contains("ComboTrigger3") && combination.Contains("ComboTrigger4"))
        {
            StartCoroutine(Unlock());
        }
        if (isUnlocked == true)
        {
            blueprintHandler.enabled = true;
            blueprint.tag = "Scannable";
            blueprint.transform.SetParent(blueprint.transform.parent.parent);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        combination.Add(other.name);
    }

    void OnTriggerExit(Collider other)
    {
        combination.Remove(other.name);
    }

    IEnumerator Unlock()
    {
        isUnlocked = true;
        Destroy(bodyJoint);
        Destroy(loopJoint, 1);
        yield return new WaitForSeconds(1);
        lid.angularXMotion = ConfigurableJointMotion.Free;
        GameObject.Find("Toolbox_Lid").layer = 9;
        Destroy(this);
        yield return null;
        
    }
}
