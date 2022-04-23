/*
 * MenuSelectCard.cs
 * This file contain the event listeners of the SelectCard's Menu buttons.
 * 
 * Date created : 19/04/2022
 * Author       : Maxime MAIRE <maxime.maire2@etu.unistra.fr
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Monopoly.UI
{

    public class MenuSelectCard : MonoBehaviour
    {

        public Button CancelButton;
        public Button ValidButton;

        void Start()
        {
            CancelButton.onClick.AddListener(CancelSelection);
            ValidButton.onClick.AddListener(ValidSelection);
        }

        private void CancelSelection()
        {
            Destroy(this.gameObject);
        }

        private void ValidSelection()
        {
            Destroy(this.gameObject);
        }

    }
}