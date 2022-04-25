/*
 * MenuAuction.cs
 * This file contain the event listeners of the auction Menu buttons.
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

    public class MenuAuction : MonoBehaviour
    {

        public Button ResumeButton;

        void Start()
        {
            ResumeButton.onClick.AddListener(ResumeGame);
            UIDirector.IsGameMenuOpen = true;
        }

        private void ResumeGame()
        {
            UIDirector.IsGameMenuOpen = false;
            Destroy(this.gameObject);
        }

    }
}
