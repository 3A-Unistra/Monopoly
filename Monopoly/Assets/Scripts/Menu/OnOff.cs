using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Monopoly.Util;
using UnityEngine.UI;

namespace Monopoly.Menu
{
    public class OnOff : MonoBehaviour
    {
        public Button Back;
        public Button Front;
        private bool switchOn;
        private bool animating;

        Vector3 leftPos, rightPos;

        public void Start()
        {
            Transform backTransform = Back.transform;
            RectTransform rectBack = (RectTransform) backTransform;
            float backWidth = rectBack.rect.width;
            leftPos = new Vector3(backTransform.position.x - backWidth/2, backTransform.position.y, backTransform.position.z);
            rightPos = new Vector3(backTransform.position.x, backTransform.position.y, backTransform.position.z);
            Front.transform.position = leftPos;
            Front.onClick.AddListener(OnFrontClick);
            if (Front.transform.position == rightPos)
                switchOn = true;
            else
                switchOn = false;
            animating = false;

        }
        public void MonFonctionAnimation()
        {
            Vector3 toPos;
            if (!switchOn)
                toPos = rightPos;
            else
                toPos = leftPos;
            Front.transform.position = Vector3.Lerp(Front.transform.position, toPos, Time.deltaTime * 8f);
            if (MathUtil.CompareVector3(Front.transform.position, toPos) == 0)
            {
                animating = false;
                switchOn = !switchOn;
            }
        }

        void OnFrontClick()
        {
            animating = true;
            if (!switchOn)
                Back.GetComponent<Image>().color = Color.green;
            else
                Back.GetComponent<Image>().color = Color.white;
        }
        
        void Update()
        {
            if (animating)
                MonFonctionAnimation();
        }
        
    }
}

