/*
 * MiniCard.cs
 * 
 * Date created : 19/04/2022
 * Author       : Maxime MAIRE <maxime.maire2@etu.unistra.fr>
 *                Finn Rayment <rayment@etu.unistra.fr>
 */

using System;
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

        public Sprite selectSprite;
        public Sprite deselectSprite;

        [HideInInspector]
        public Action previewCallback;
        [HideInInspector]
        public Action<int, bool> selectCallback;

        [HideInInspector]
        public bool editable = false;

        void Start()
        {
            Selected = false;
            SelectButton.image.sprite = deselectSprite;
            SelectButton.onClick.AddListener(delegate { ToggleSelect(true); });
            PreviewButton.onClick.AddListener(delegate { previewCallback(); });
            if (!editable)
                SelectButton.enabled = false;
        }

        public void ToggleSelect(bool callback)
        {
            Selected = !Selected;
            SelectButton.image.sprite =
                Selected ? selectSprite : deselectSprite;
            if (callback)
                selectCallback(Index, Selected);
        }

    }
 
}
