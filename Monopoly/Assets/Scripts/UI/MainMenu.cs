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
using Monopoly.Util;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using Monopoly.Runtime;
using Monopoly.Util;

namespace Monopoly.UI
{
    public class MainMenu : MonoBehaviour
    {
        public Button PlayOnlineButton;
        public TMP_Text PlayText;
        public Button OptionsButton;
        public TMP_Text OptionsText;
        public Button ConnectIPButton;
        public TMP_Text ConnectText;
        public Button QuitButton;
        public TMP_Text QuitText;
        public TMP_Text ErrorText;

        public GameObject LoginMenuPrefab;
        public GameObject ConnectMenuPrefab;
        public GameObject OptionsMenuPrefab;
        
        public static bool OptionsOpened = false;

        private ClientLobbyState connector;

        void Start()
        {
            OptionsButton.onClick.AddListener(OpenOptionsMenu);
            PlayOnlineButton.onClick.AddListener(PlayOnline);
            ConnectIPButton.onClick.AddListener(ConnectIP);
            QuitButton.onClick.AddListener(QuitGame);

            PlayText.text = StringLocaliser.GetString("play_online");
            ConnectText.text = StringLocaliser.GetString("connect_ip");
            OptionsText.text = StringLocaliser.GetString("options");
            QuitText.text = StringLocaliser.GetString("quit");

            UIDirector.IsMenuOpen = true;
        }

        void OnDestroy()
        {
            if (connector != null)
                Destroy(connector.gameObject);
        }

        public void OpenOptionsMenu()
        {
            if (!OptionsOpened)
            {
                UIDirector.IsMenuOpen = false;
                GameObject OptionsMenu = Instantiate(OptionsMenuPrefab,transform.parent);
                OptionsOpened = true;
                Destroy(this.gameObject);
            }
        }
        
        public void PlayOnline()
        {
            UIDirector.IsMenuOpen = false;
            GameObject loginMenu =
                Instantiate(LoginMenuPrefab, transform.parent);
            Destroy(this.gameObject);
        }
    
        public void ConnectIP()
        {
            UIDirector.IsMenuOpen = false;
            GameObject ConnectMenu =
                Instantiate(ConnectMenuPrefab, transform.parent);
            Destroy(this.gameObject);
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        public void DisplayError(string error)
        {
            ErrorText.text = StringLocaliser.GetString(error);
            ErrorText.gameObject.SetActive(true);
        }

    }

}