/*
 * OnOff.cs
 * This file contain the animation of the on/off switch 
 * 
 * Date created : 25/02/2022
 * Author       : Rayan MARMAR <rayan.marmar@etu.unistra.fr>
 *                Finn RAYMENT <rayment@etu.unistra.fr>
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Monopoly.Util;
using UnityEngine.UI;

namespace Monopoly.UI
{
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(Button))]
    public class OnOff : MonoBehaviour
    {
        public Button Front;
        public bool switchOn;
        private bool animating;
        private Vector3 leftPos, rightPos;

        public void Start()
        {
            RectTransform rectBack = GetComponent<RectTransform>();
            float backWidth = rectBack.rect.width;
            leftPos = new Vector3(transform.localPosition.x - backWidth / 2, transform.localPosition.y,
                transform.localPosition.z);
            rightPos = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
            Front.onClick.AddListener(OnClick);
            GetComponent<Button>().onClick.AddListener(OnClick);
            //switchOn = true;
            if (switchOn)
            {
                Front.transform.localPosition = rightPos;
                GetComponent<Image>().color = Color.green;
            }
            else
            {
                Front.transform.localPosition = leftPos;
                GetComponent<Image>().color = Color.white;
            }
            animating = false;
        }
        private void SwitchAnimation()
        {
            Vector3 toPos;
            if (!switchOn)
                toPos = rightPos;
            else
                toPos = leftPos;
            Front.transform.localPosition = Vector3.Lerp(Front.transform.localPosition, toPos, Time.deltaTime * 8f);
            if (MathUtil.CompareVector3(Front.transform.localPosition, toPos) == 0)
            {
                animating = false;
                switchOn = !switchOn;
            }
        }

        private void OnClick()
        {
           if (!animating)
           {
               animating = true;
               if (!switchOn)
                   GetComponent<Image>().color = Color.green;
               else
                   GetComponent<Image>().color = Color.white;
           }
        }
        
        void Update()
        {
            if (animating)
                SwitchAnimation();
        }
        
    }
}

