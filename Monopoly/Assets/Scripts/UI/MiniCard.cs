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

using Monopoly.Classes;

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

        void Awake()
        {
            Selected = false;
            SelectButton.image.sprite = deselectSprite;
            SelectButton.onClick.AddListener(delegate { ToggleSelect(true); });
            PreviewButton.onClick.AddListener(delegate { previewCallback(); });
            if (!editable)
                SelectButton.enabled = false;
        }

        void Start()
        {
            if (PropertySquare.IsPropertyIndex(Index))
                Name.color = PropertySquare.GetColorIndex(Index);
            else
                Name.color = Color.white;
        }

        public void ToggleSelect(bool callback)
        {
            if (callback)
            {
                // we are the ones updatig the button and need the server to
                // agree before we update it, so don't update the sprite nor
                // the value for now
                selectCallback(Index, Selected);
            }
            else
            {
                // we are being told to update this button
                Selected = !Selected;
                SelectButton.image.sprite =
                    Selected ? selectSprite : deselectSprite;
            }
        }

    }
 
}
