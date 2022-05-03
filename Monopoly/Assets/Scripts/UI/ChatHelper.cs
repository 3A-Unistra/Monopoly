/*
 * ChatHelper.cs
 * UI helper script for the chat menu.
 * 
 * Date created : 03/05/2022
 * Author       : Finn RAYMENT <rayment@etu.unistra.fr>
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Monopoly.Runtime;

namespace Monopoly.UI
{
    
    [RequireComponent(typeof(Button))]
    public class ChatHelper : MonoBehaviour
    {

        private Button button;
        private Coroutine repeater;

        public Color highlightColor;
        public Color darkColor;

        private bool toggle;

        void Awake()
        {
            button = GetComponent<Button>();
            button.image.color = highlightColor;
        }

        public void Notify()
        {
            if (repeater == null)
                repeater = StartCoroutine(NotifyRepeat());
        }

        private IEnumerator NotifyRepeat()
        {
            toggle = false;
            while (true)
            {
                // repeat forever until told to do otherwise
                button.image.color = toggle ? darkColor : highlightColor;
                toggle = !toggle;
                yield return new WaitForSeconds(0.5f);
            }
        }

        public void Clear()
        {
            if (repeater != null)
            {
                StopCoroutine(repeater);
                repeater = null;
            }
            button.image.color = highlightColor;
        }

    }

}
