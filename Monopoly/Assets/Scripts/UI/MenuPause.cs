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
using UnityEngine.SceneManagement;

namespace Monopoly.UI
{
    
    [RequireComponent(typeof(RectTransform))]
    public class MenuPause : MonoBehaviour
    {
        //public GameObject OptionsMenu;
        public GameObject PrefabOptions;
        public Button ResumeButton;
        public Button OptionsButton;
        public Button DisconnectButton;
        public Button QuitButton;
        
        public static bool OptionsOpenedFromPauseMenu = false;
        void Start()
        {
            ResumeButton.onClick.AddListener(ResumeGame);
            OptionsButton.onClick.AddListener(OpenOptions);
            
            
            RectTransform rt = GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0.5f, 0.5f);
            rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.pivot = new Vector2(0.5f, 0.5f);
            rt.localPosition = Vector3.zero;
            #if UNITY_WEBGL
                        DisconnectButton.onClick.AddListener(QuitGame);
                        QuitButton.gameObject.SetActive(false);
            #else
                        DisconnectButton.onClick.AddListener(DisconnectFromTheGame);
                        QuitButton.onClick.AddListener(QuitGame);
            #endif
        }


        private void ResumeGame()
        {
            Destroy(this.gameObject);
            PauseHelper.MenuOpened = false;
        }
        private void OpenOptions()
        {
            OptionsOpenedFromPauseMenu = true;
            GameObject optionsMenu = Instantiate(PrefabOptions,transform.parent);
            Destroy(this.gameObject);
        }

        private void DisconnectFromTheGame()
        {
            SceneManager.LoadScene("Scenes/MainMenu");
        }
        
        public void QuitGame()
        {
            Application.Quit();
        }
    }
}

