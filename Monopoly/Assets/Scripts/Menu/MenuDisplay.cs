/*
 * MenuDisplay.cs
 * This file contain the event listeners of the display buttons and menus.
 * 
 * Date created : 28/02/2022
 * Author       : Rayan MARMAR <rayan.marmar@etu.unistra.fr>
 *                Maxime MAIRE <maxime.maire2@etu.unistra.fr
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Monopoly.Menu
{
    public class MenuDisplay : MonoBehaviour
    {
        public GameObject PauseMenu;
        public GameObject OptionsMenu;
        public GameObject OptionsButton;
        public GameObject PauseButton;
        public TMP_Dropdown ResolutionDropdown;
        public TMP_Dropdown QualityDropdown;
        public Slider MusicSlider;
        public Slider SoundSlider;
    
        void Start()
        {
            PauseMenu.SetActive(false);
            OptionsMenu.SetActive(false);
        }
    
        void Update()
        {
        }
    
        public void MenuPauseActive()
        {
            PauseMenu.SetActive(true);
            PauseButton.SetActive(false);
            OptionsButton.SetActive(false);
        }
        
        public void MenuOptionsActive()
        {
            OptionsMenu.SetActive(true);
            PauseButton.SetActive(false);
            OptionsButton.SetActive(false);
            MenuOptions.Resolution = ResolutionDropdown.value;
            MenuOptions.Quality = QualityDropdown.value;
            MenuOptions.Music = MusicSlider.value;
            MenuOptions.Sound = SoundSlider.value;
        }
    }
}

