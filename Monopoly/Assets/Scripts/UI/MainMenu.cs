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

        public GameObject LobbyMenuPrefab;
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
            /*Dictionary<string, string> data = JsonLoader.LoadJsonAsset
                <Dictionary<string, string>>("Data/data");
            string address;
            int port;
            if (data == null ||
                !data.ContainsKey("default_ip") ||
                !data.ContainsKey("default_port") ||
                !int.TryParse(data["default_port"], out port))
            {
                DisplayError("connection_baddata");
                return;
            }
            address = data["default_ip"];
            GameObject clientLobbyObject = new GameObject("ClientLobbyState");
            ClientLobbyState state =
                clientLobbyObject.AddComponent<ClientLobbyState>();
            state.Canvas = transform.parent.gameObject;
            connector = state;
            // TODO: UPDATE TOKEN
            state.StartCoroutine(
                state.ConnectWithPort(
                    address, port, "283e3f3e-3411-44c5-9bc5-037358c47100",
                    this, ClientLobbyState.ConnectMode.ONLINE));*/

            // FIXME: uncomment everything above for the actual release, this is
            // just temporary so we can access the board scene!!!
            UIDirector.IsMenuOpen = false;
            GameObject lobbyMenu =
                Instantiate(LobbyMenuPrefab, transform.parent);
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