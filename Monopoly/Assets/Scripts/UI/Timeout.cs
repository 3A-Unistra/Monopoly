/*
 * Timeout.cs
 * Timeout UI element.
 * 
 * Date created : 25/04/2022
 * Author       : Finn Rayment <rayment@etu.unistra.fr>
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Monopoly.UI
{

    [RequireComponent(typeof(Slider))]
    public class Timeout : MonoBehaviour
    {

        public Slider slider;
        public Image fill;

        public Color colorFull;
        public Color colorEmpty;

        private int timeoutSeconds;
        private float animationTime;
        private bool canAnimate = false;

        void Start()
        {
            slider.value = 1;
            fill.color = colorFull;
            animationTime = 0.0f;
        }

        void Update()
        {
            if (canAnimate && animationTime < timeoutSeconds)
            {
                float delta = Time.deltaTime;
                animationTime += delta;
                fill.color = Color.Lerp(colorFull, colorEmpty,
                                        animationTime / timeoutSeconds);
                slider.value -= delta / timeoutSeconds;
                if (slider.value <= 0)
                    Pause();
            }
        }

        public void SetTime(int time)
        {
            timeoutSeconds = time;
        }

        public void SetRemainingTime(int time)
        {
            animationTime =
                Mathf.Clamp(timeoutSeconds - time, 0, timeoutSeconds);
        }

        public void Restart()
        {
            slider.value = 1;
            fill.color = colorFull;
            animationTime = 0.0f;
            canAnimate = true;
            gameObject.SetActive(true);
        }

        public void Resume()
        {
            canAnimate = true;
        }

        public void Pause()
        {
            canAnimate = false;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

    }

}
