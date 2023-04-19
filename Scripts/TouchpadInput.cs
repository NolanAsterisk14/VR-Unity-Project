using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;







    public class TouchpadInput : MonoBehaviour
    {
        [SerializeField]
        private GameObject scrollContent; //Content to scroll
        [SerializeField]
        private Transform scrollPos; //Transform of content
        [SerializeField]
        private Vector2 touchPos; //Vector of inputs from touchpad
        [SerializeField]
        private float touchStart; //Storage variable for the start of scroll position
        [SerializeField]
        private float triggerPress;
        [SerializeField]
        private float gripButton;
        
        
        void OnEnable()
        {

            //Get the transform for the group of content to be scrolled. Make sure this is set in editor!
            scrollPos = scrollContent.GetComponent<Transform>();
            
        }

        void Update()
        {
            //Get the input from the touchpad
            touchPos.y = Input.GetAxis("AXIS_18");
            touchPos.x = Input.GetAxis("AXIS_17");
            triggerPress = Input.GetAxis("AXIS_10");
            gripButton = Input.GetAxis("AXIS_12");
          /*  
            if (touchPos.y != 0)
        {

            scrollPos.transform.Translate(0f, touchPos.y, 0f * Time.deltaTime);
        }

            */

        }


    }
