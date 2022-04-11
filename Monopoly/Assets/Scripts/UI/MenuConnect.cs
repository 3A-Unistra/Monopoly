using System.Collections;
using System.Collections.Generic;
using Monopoly.Util;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Monopoly.UI
{
    public class MenuConnect : MonoBehaviour
    {
        public Button MainMenuButton;
        public TMP_Text MainMenuText;
        public Button ConnectButton;
        public TMP_Text ConnectText;
        public TMP_InputField IPInput;
        public GameObject MainMenuPrefab;
        public GameObject LobbyMenuPrefab;
        void Start()
        {
            MainMenuButton.onClick.AddListener(ReturnToMainMenu);
            ConnectButton.onClick.AddListener(Connect);

            MainMenuText.text = StringLocaliser.GetString("main menu");
            ConnectText.text = StringLocaliser.GetString("connect");
            IPInput.placeholder.GetComponent<TextMeshProUGUI>().text = StringLocaliser.GetString("ip address");
        }
        
        public void Connect()
        {
            GameObject lobbyMenu = Instantiate(LobbyMenuPrefab,transform.parent);
            Destroy(this.gameObject);
        }
        
        public void ReturnToMainMenu()
        {
            GameObject MainMenu = Instantiate(MainMenuPrefab, transform.parent);
            Destroy(this.gameObject);
        }
    }
}

