using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Monopoly.UI
{
    public class MenuConnect : MonoBehaviour
    {
        public Button MainMenuButton;
        public Button ConnectButton;
        public GameObject MainMenuPrefab;
        public GameObject LobbyMenuPrefab;
        void Start()
        {
            MainMenuButton.onClick.AddListener(ReturnToMainMenu);
            ConnectButton.onClick.AddListener(Connect);
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

