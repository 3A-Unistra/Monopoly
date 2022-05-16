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
        public TMP_InputField UsernameInput;
        public GameObject ErrorTextField;
        public TMP_Text ErrorText;

        private ClientLobbyState connector = null;

        void Start()
        {
            MainMenuButton.onClick.AddListener(ReturnToMainMenu);
            ConnectButton.onClick.AddListener(Connect);

            MainMenuText.text = StringLocaliser.GetString("main_menu");
            ConnectText.text = StringLocaliser.GetString("connect");
            IPInput.placeholder.GetComponent<TextMeshProUGUI>().text =
                StringLocaliser.GetString("ip_address_input");
            PortInput.placeholder.GetComponent<TextMeshProUGUI>().text =
                StringLocaliser.GetString("port_input");
            UsernameInput.placeholder.GetComponent<TextMeshProUGUI>().text =
                StringLocaliser.GetString("username_input");

            ErrorTextField.SetActive(false);

            string defaultIP = PlayerPrefs.GetString("favourite_ip", "");
            string defaultPort = PlayerPrefs.GetString("favourite_port", "");
            string defaultUsername =
                PlayerPrefs.GetString("favourite_offlineusername", "");
            IPInput.text = defaultIP;
            PortInput.text = defaultPort;
            UsernameInput.text = defaultUsername;

            UIDirector.IsMenuOpen = true;
            UIDirector.IsUIBlockingNet = false;
        }
        
        public void Connect()
        {
            if (ClientLobbyState.current != null)
                return; // already doing one, ignore other presses

            string address = IPInput.text.Trim();
            if (address.Equals(""))
            {
                IPInput.text = "";
                DisplayError("connection_noip");
                return;
            }
            int port;
            string porttxt = PortInput.text.Trim();
            if (porttxt.Equals(""))
            {
                // using the default port of 80 instead
                porttxt = "80";
            }
            if (!int.TryParse(porttxt, out port) ||
                port < 0 || port >= 65536)
            {
                PortInput.text = "";
                DisplayError("connection_badport");
                return;
            }
            ConnectButton.interactable = false;

            GameObject clientLobbyObject = new GameObject("ClientLobbyState");
            ClientLobbyState state =
                clientLobbyObject.AddComponent<ClientLobbyState>();
            state.Canvas = transform.parent.gameObject;
            PlayerPrefs.SetString("favourite_ip", address);
            PlayerPrefs.SetString("favourite_port", port.ToString());
            string usernameSelect = UsernameInput.text.Trim();
            if (usernameSelect.Length > 32)
                usernameSelect = usernameSelect.Substring(0, 32);
            ClientLobbyState.desiredClientUsername = usernameSelect;
            // TODO: UPDATE TOKEN AND SHIT FROM LOCAL FILE
            state.StartCoroutine(
                state.Connect(address, port,
                              RuntimeInit.localUUID.ToString(), null,
                              this, ClientLobbyState.ConnectMode.BYIP));
        }
        
        public void ReturnToMainMenu()
        {
            // if we manage to return during a socket connection, then close the
            // lobby state and socket as we leave
            if (ClientLobbyState.current != null)
            {
                if (connector != null)
                {
                    connector.StopAllCoroutines();
                    Destroy(connector);
                }
                Destroy(ClientLobbyState.current.gameObject);
            }

            UIDirector.IsMenuOpen = false;
            GameObject MainMenu =
                Instantiate(RuntimeData.current.MainMenuPrefab, transform.parent);
            Destroy(this.gameObject);
        }

        public void DisplayError(string error)
        {
            ConnectButton.interactable = true;
            ErrorText.text = StringLocaliser.GetString(error);
            ErrorTextField.SetActive(true);
        }

    }
}

