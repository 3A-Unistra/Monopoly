/*
 * MiniCard.cs
 * 
 * Date created : 19/04/2022
 * Author       : Maxime MAIRE <maxime.maire2@etu.unistra.fr>
 *                Finn Rayment <rayment@etu.unistra.fr>
 */

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Monopoly.UI
{
    public class MiniCard : MonoBehaviour
    {
        public Button PreviewButton;
        public Button SelectButton;
        public TMP_Text Name;
        public TMP_Text Price;

        public bool Selected { get; private set; }
        [HideInInspector]
        public int Index;

        void Start()
        {
            Selected = false;
            SelectButton.onClick.AddListener(ToggleSelect);
        }

        private void ToggleSelect()
        {
            Selected = !Selected;
        }

    }
 
}
