using System.Collections;
using System.Collections.Generic;
using Monopoly.Util;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using Monopoly.Runtime;

namespace Monopoly.UI
{
    public class MenuConnect : MonoBehaviour
    {
        public Button MainMenuButton;
        public TMP_Text MainMenuText;
        public Button ConnectButton;
        public TMP_Text ConnectText;
        public TMP_InputField IPInput;
        public GameObject ErrorTextField;
        public TMP_Text ErrorText;
        public GameObject MainMenuPrefab;
        public GameObject LobbyMenuPrefab;

        private ClientLobbyState connector = null;

        void Start()
        {
            MainMenuButton.onClick.AddListener(ReturnToMainMenu);
            ConnectButton.onClick.AddListener(Connect);

            MainMenuText.text = StringLocaliser.GetString("main_menu");
            ConnectText.text = StringLocaliser.GetString("connect");
            IPInput.placeholder.GetComponent<TextMeshProUGUI>().text = StringLocaliser.GetString("ip_address");

            ErrorTextField.SetActive(false);

            UIDirector.IsMenuOpen = true;
        }
        
        public void Connect()
        {
            string loc = IPInput.text.Trim();
            if (loc.Equals(""))
            {
                IPInput.text = "";
                DisplayError("connection_noip");
                return;
            }
            GameObject clientLobbyObject = new GameObject("ClientLobbyState");
            ClientLobbyState state =
                clientLobbyObject.AddComponent<ClientLobbyState>();
            state.Canvas = transform.parent.gameObject;
            state.MainMenuPrefab = MainMenuPrefab;
            connector = state;
            // TODO: UPDATE TOKEN
            state.StartCoroutine(
                state.Connect(loc, "283e3f3e-3411-44c5-9bc5-037358c47100",
                              this, ClientLobbyState.ConnectMode.BYIP));
        }
        
        public void ReturnToMainMenu()
        {
            // if we manage to return during a socket connection, then close the
            // socket as we leave
            if (connector != null)
            {
                connector.StopAllCoroutines();
                Destroy(connector);
            }

            UIDirector.IsMenuOpen = false;
            GameObject MainMenu = Instantiate(MainMenuPrefab, transform.parent);
            Destroy(this.gameObject);
        }

        public void DisplayError(string error)
        {
            ErrorText.text = StringLocaliser.GetString(error);
            ErrorTextField.SetActive(true);
        }

    }
}

