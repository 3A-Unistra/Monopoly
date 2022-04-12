using System.Collections;
using System.Collections.Generic;
using Monopoly.Util;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using Monopoly.Runtime;

namespace Monopoly.UI
{
    public class MenuLobby : MonoBehaviour
    {
        public TMP_InputField TokenField;
        public Button SearchButton;
        public TMP_Text SearchText;
        public Button CreateButton;
        public TMP_Text CreateText;
        public Button MainMenuButton;
        public TMP_Text MainMenuText;
        public GameObject LobbyList;
            
        public GameObject LobbyElementPrefab;
        public GameObject MainMenuPrefab;
        public GameObject CreateMenuPrefab;

        void Start()
        {
            MainMenuButton.onClick.AddListener(ReturnToMainMenu);
            SearchButton.onClick.AddListener(SearchToken);
            CreateButton.onClick.AddListener(CreateLobby);
            CreateLobbyButton();

            TokenField.placeholder.GetComponent<TextMeshProUGUI>().text = StringLocaliser.GetString("private_token");
            SearchText.text = StringLocaliser.GetString("search");
            CreateText.text = StringLocaliser.GetString("create_lobby");
            MainMenuText.text = StringLocaliser.GetString("main_menu");

            UIDirector.IsMenuOpen = true;
        }

        public void SearchToken()
        {
            string txt = TokenField.text;
            TokenField.text = "";
        }
        
        public void ReturnToMainMenu()
        {
            if (ClientLobbyState.current != null)
            {
                Destroy(ClientLobbyState.current.gameObject);
            }
            UIDirector.IsMenuOpen = false;
            GameObject MainMenu = Instantiate(MainMenuPrefab, transform.parent);
            Destroy(this.gameObject);
        }

        public void CreateLobby()
        {
            UIDirector.IsMenuOpen = false;
            GameObject CreateMenu = Instantiate(CreateMenuPrefab, transform.parent);
            CreateMenu.GetComponent<MenuCreate>().IsHost = true;
            Destroy(this.gameObject);
        }

        // TODO: Populate
        public void CreateLobbyButton()
        {
            GameObject lobbyElement = Instantiate(LobbyElementPrefab, LobbyList.transform);
            lobbyElement.GetComponent<LobbyJoin>().ParentMenu = gameObject;
        }
        
    }
}

