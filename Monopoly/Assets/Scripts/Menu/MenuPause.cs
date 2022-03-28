/*
 * MenuPause.cs
 * This file contain the event listeners of the pause's Menu buttons.
 * 
 * Date created : 28/02/2022
 * Author       : Rayan MARMAR <rayan.marmar@etu.unistra.fr>
 *                Maxime MAIRE <maxime.maire2@etu.unistra.fr
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Monopoly.Menu
{
    public class MenuPause : MonoBehaviour
    {
        public GameObject PauseMenu;
        public GameObject MenuOptions;
        public GameObject PauseButton;
        public GameObject OptionsButton;

        public void MenuDisable()
        {
            PauseMenu.SetActive(false);
            PauseButton.SetActive(true);
            OptionsButton.SetActive(true);
        }
    
        public void OptionActive()
        {
            MenuOptions.SetActive(true);
            PauseMenu.SetActive(false);
        }
    
        public void DisconnectToTheGame()
        {
            ;
        }
    
        public void QuitGame()
        {
            Application.Quit();
        }
    }
}

