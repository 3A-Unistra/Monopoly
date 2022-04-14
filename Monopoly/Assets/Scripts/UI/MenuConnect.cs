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
        public TMP_InputField PortInput;
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
            IPInput.placeholder.GetComponent<TextMeshProUGUI>().text = StringLocaliser.GetString("ip_address_input");
            PortInput.placeholder.GetComponent<TextMeshProUGUI>().text = StringLocaliser.GetString("port_input");

            ErrorTextField.SetActive(false);

            string defaultIP = PlayerPrefs.GetString("favourite_ip", "");
            string defaultPort = PlayerPrefs.GetString("favourite_port", "");
            IPInput.text = defaultIP;
            PortInput.text = defaultPort;

            UIDirector.IsMenuOpen = true;
        }
        
        public void Connect()
        {
            string address = IPInput.text.Trim();
            if (address.Equals(""))
            {
                IPInput.text = "";
                DisplayError("connection_noip");
                return;
            }
            int port;
            string porttxt = PortInput.text.Trim();
            if (porttxt.Equals("") || !int.TryParse(porttxt, out port) ||
                port < 0 || port >= 65536)
            {
                PortInput.text = "";
                DisplayError("connection_badport");
                return;
            }
            
            GameObject clientLobbyObject = new GameObject("ClientLobbyState");
            ClientLobbyState state =
                clientLobbyObject.AddComponent<ClientLobbyState>();
            state.Canvas = transform.parent.gameObject;
            state.MainMenuPrefab = MainMenuPrefab;
            PlayerPrefs.SetString("favourite_ip", address);
            PlayerPrefs.SetString("favourite_port", port.ToString());
            connector = state;
            // TODO: UPDATE TOKEN AND SHIT FROM LOCAL FILE
            state.StartCoroutine(
                state.Connect(address, port,
                              RuntimeInit.localUUID.ToString(), null,
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

