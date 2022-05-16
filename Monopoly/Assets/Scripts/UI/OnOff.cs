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
        public Button Back;
        public Button Front;
        public bool switchOn;
        private bool animating, tempSwitchOn;
        private Vector3 leftPos, rightPos;
        
        private RectTransform backRect;
        private RectTransform frontRect;

        public new bool enabled
        {
            get
            {
                return this._enabled;
            }
            set
            {
                Back.interactable = value;
                Front.interactable = value;
                this._enabled = value;
            }
        }

        private bool _enabled = true;

        public void Start()
        {
            backRect = GetComponent<RectTransform>();
            frontRect = Front.GetComponent<RectTransform>();
            float backWidth = backRect.rect.width;
            leftPos = new Vector3(-backWidth/2, 0, 0);
            rightPos = new Vector3(0, 0, 0);
            Front.onClick.AddListener(OnClick);
            GetComponent<Button>().onClick.AddListener(OnClick);
            if (switchOn)
            {
                frontRect.localPosition = rightPos;
                GetComponent<Image>().color = Color.green;
            }
            else
            {
                frontRect.localPosition = leftPos;
                GetComponent<Image>().color = Color.white;
            }
            tempSwitchOn = switchOn;
            animating = false;
        }

        private void SwitchAnimation()
        {
            Vector3 toPos;
            if (!tempSwitchOn)
                toPos = rightPos;
            else
                toPos = leftPos;
            frontRect.localPosition = Vector3.Lerp(frontRect.localPosition, toPos, Time.deltaTime * 8f);
            if (MathUtil.CompareVector3(frontRect.localPosition, toPos) == 0)
            {
                animating = false;
                tempSwitchOn = switchOn;
            }
        }

        private void OnClick()
        {
            tempSwitchOn = switchOn;
            switchOn = !switchOn;
            animating = true;
            if (!tempSwitchOn)
                GetComponent<Image>().color = Color.green;
            else
                GetComponent<Image>().color = Color.white;
        }
        
        void Update()
        {
            if (animating)
                SwitchAnimation();
        }
        
    }
}

