using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandAnimator : MonoBehaviour
{
    private Animator anim;
    [SerializeField]
    private float gripPress;
    
    void Start()
    {
        anim = this.gameObject.GetComponent<Animator>();
        
    }


    void Update()
    {
        gripPress = Input.GetAxis("AXIS_11");
        if (gripPress == 1)
        {
            anim.SetTrigger("GripPress");
        }
        if (gripPress == 0)
        {
            anim.ResetTrigger("GripPress");
        }
    }
}
