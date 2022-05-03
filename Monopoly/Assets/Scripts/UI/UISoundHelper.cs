/*
 * UISoundHelper.cs
 * Automatic sound manager for UI.
 * 
 * Date created : 03/05/2022
 * Author       : Finn Rayment <rayment@etu.unistra.fr>
 */

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using Monopoly.Runtime;

namespace Monopoly.UI
{

    public class UISoundHelper : MonoBehaviour
    {

        public Button button;
        public TMP_Dropdown dropdown;

        void Start()
        {
            if (button != null)
                button.onClick.AddListener(PlayButtonSound);
            if (dropdown != null)
            {
                UIPointerHelper pointerHelper =
                    dropdown.GetComponent<UIPointerHelper>();
                if (pointerHelper != null)
                    pointerHelper.onClick += delegate { PlayButtonSound(); };
                dropdown.onValueChanged.AddListener(
                    delegate { PlayButtonSound(); });
            }
            Destroy(this); // not needed anymore after we attach the listener
        }

        private static void PlayButtonSound()
        {
            RuntimeData.current.SoundHandler.PlayButtonBlip();
        }

    }

}
