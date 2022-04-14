using System.Collections;
using System.Collections.Generic;
using Monopoly.Util;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using Monopoly.Net.Packets;
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

        public static MenuLobby current;

        public static List<LobbyElement> lobbyElements;
        private List<LobbyJoin> lobbyJoinElements;

        public class LobbyElement
        {
            public string Name { get; set; }
            public string Token { get; set; }
            public LobbyElement(string name, string token)
            {
                this.Name = name;
                this.Token = token;
            }
        }

        static MenuLobby()
        {
            current = null;
            lobbyElements = new List<LobbyElement>();
        }

        void Start()
        {
            MainMenuButton.onClick.AddListener(ReturnToMainMenu);
            SearchButton.onClick.AddListener(SearchToken);
            CreateButton.onClick.AddListener(CreateLobby);

            TokenField.placeholder.GetComponent<TextMeshProUGUI>().text = StringLocaliser.GetString("private_token");
            SearchText.text = StringLocaliser.GetString("search");
            CreateText.text = StringLocaliser.GetString("create_lobby");
            MainMenuText.text = StringLocaliser.GetString("main_menu");

            lobbyJoinElements = new List<LobbyJoin>();

            UIDirector.IsMenuOpen = true;

            current = this;

            foreach (LobbyElement e in lobbyElements)
            {
                Debug.Log("looping");
                CreateLobbyButton(e.Name, e.Token, false);
            }
        }

        void OnDestroy()
        {
            if (current == this)
                current = null;
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
            ClientLobbyState.current.DoCreateGame(
                ClientLobbyState.current.clientUUID, 2, "", "hello", false, 1500, true, false, 60, 100, false);
        }

        public void CreateLobbyButton(string name, string token, bool isnew)
        {
            Debug.Log("creating");
            GameObject lobbyElement = Instantiate(LobbyElementPrefab, LobbyList.transform);
            LobbyJoin lobbyScript = lobbyElement.GetComponent<LobbyJoin>();
            lobbyScript.ParentMenu = gameObject;
            lobbyScript.LobbyName.text = name;
            lobbyScript.Token = token;
            lobbyJoinElements.Add(lobbyScript);
            if (isnew)
            {
                Debug.Log("adding new");
                lobbyElements.Add(new LobbyElement(name, token));
            }
        }
        
    }
}

