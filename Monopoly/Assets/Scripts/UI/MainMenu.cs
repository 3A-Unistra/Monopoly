/*
 * MainMenu.cs
 * This file contain the event listeners of the Main Menu buttons.
 * 
 * Date created : 05/04/2022
 * Author       : Rayan MARMAR <rayan.marmar@etu.unistra.fr>
 *                Maxime MAIRE <maxime.maire2@etu.unistra.fr
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



namespace Monopoly.UI
{
    public class MainMenu : MonoBehaviour
    {
        public Button PlayOnlineButton;
        public Button OptionsButton;
        public Button ConnectIPButton;
        public Button QuitButton;
        public GameObject LobbyMenuPrefab;
        public GameObject ConnectMenuPrefab;
        public GameObject OptionsMenuPrefab;
        
        public static bool OptionsOpened = false;
        void Start()
        {
            OptionsButton.onClick.AddListener(OpenOptionsMenu);
            PlayOnlineButton.onClick.AddListener(PlayOnline);
            ConnectIPButton.onClick.AddListener(ConnectIP);
            QuitButton.onClick.AddListener(QuitGame);
        }
    
        public void OpenOptionsMenu()
        {
            if (!OptionsOpened)
            {
                GameObject OptionsMenu = Instantiate(OptionsMenuPrefab,transform.parent);
                OptionsOpened = true;
            }
            
        }
        
        public void PlayOnline()
        {
            GameObject lobbyMenu = Instantiate(LobbyMenuPrefab,transform.parent);
            Destroy(this.gameObject);
        }
    
        public void ConnectIP()
        {
            GameObject ConnectMenu = Instantiate(ConnectMenuPrefab, transform.parent);
            Destroy(this.gameObject);
        }
        
        public void QuitGame()
        {
            Application.Quit();
        }
    }
}