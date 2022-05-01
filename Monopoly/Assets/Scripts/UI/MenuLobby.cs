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

        //public static List<LobbyElement> lobbyElements;
        private List<LobbyJoin> lobbyJoinElements;

        public class LobbyElement
        {
            public string Name { get; set; }
            public string Token { get; set; }
            // TODO: UPDATE ON PACKET RECV
            public int Num { get; set; }
            public int Max { get; set; }
            public LobbyElement(string name, string token, int num, int max)
            {
                this.Name = name;
                this.Token = token;
                this.Num = num;
                this.Max = max;
            }
        }

        static MenuLobby()
        {
            current = null;
            //lobbyElements = new List<LobbyElement>();
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

            current = this;

            UIDirector.IsMenuOpen = true;
            UIDirector.IsUIBlockingNet = false;

            //foreach (LobbyElement e in lobbyElements)
            //    CreateLobbyButton(e.Name, e.Token, e.Num, e.Max, false);
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
            string name = string.Format(
                StringLocaliser.GetString("default_lobby"),
                ClientLobbyState.clientUsername);
            ClientLobbyState.current.DoCreateGame(ClientLobbyState.clientUUID,
                                                  8,
                                                  "",
                                                  name,
                                                  false,
                                                  1500,
                                                  true,
                                                  false,
                                                  60,
                                                  100,
                                                  false);
        }

        private static void SetLobbyPlayerQty(LobbyJoin l)
        {
            l.NumberPlayersText.text = string.Format("{0}/{1}", l.Num, l.Max);
        }

        public void CreateLobbyButton(string name, string token,
                                      int num, int max, bool isnew)
        {
            GameObject lobbyElement = Instantiate(LobbyElementPrefab, LobbyList.transform);
            LobbyJoin lobbyScript = lobbyElement.GetComponent<LobbyJoin>();
            lobbyScript.ParentMenu = gameObject;
            lobbyScript.LobbyName.text = name;
            lobbyScript.Token = token;
            lobbyScript.Num = num;
            lobbyScript.Max = max;
            SetLobbyPlayerQty(lobbyScript);
            lobbyJoinElements.Add(lobbyScript);
            //if (isnew)
            //    lobbyElements.Add(new LobbyElement(name, token, num, max));
        }

        public void UpdateLobby(PacketBroadcastUpdateLobby packet)
        {
            LobbyJoin actual = null;
            //if (MenuLobby.current != null)
            //{
            foreach (LobbyJoin lj in lobbyJoinElements)
            {
                if (lj.Token.Equals(packet.LobbyToken))
                {
                    actual = lj;
                    break;
                }
            }
            if (actual == null)
                return;
            //}
            //for (int i = 0; i < lobbyElements.Count; ++i)
            //for (int i = 0; i < lobbyElements.Count; ++i)
            //{
            //    LobbyElement e = lobbyElements[i];
            //    if (!e.Token.Equals(packet.LobbyToken))
            //        continue;
            switch (packet.Reason)
            {
            case PacketBroadcastUpdateLobby.UpdateReason.PLAYER_LEFT:
                //--e.Num;
                --actual.Num;
                //if (actual != null)
                SetLobbyPlayerQty(actual);//e.Num, e.Max);
                break;
            case PacketBroadcastUpdateLobby.UpdateReason.NEW_PLAYER:
                ++actual.Num;
                //if (actual != null)
                SetLobbyPlayerQty(actual);//, e.Num, e.Max);
                break;
            case PacketBroadcastUpdateLobby.UpdateReason.NEW_BOT:
                ++actual.Num;
                //if (actual != null)
                SetLobbyPlayerQty(actual);//, e.Num, e.Max);
                break;
            case PacketBroadcastUpdateLobby.UpdateReason.ROOM_DELETED:
                //lobbyElements.Remove(e);
                //if (actual != null)
                lobbyJoinElements.Remove(actual);
                Destroy(actual.gameObject);
                //--i; // retry iterator
                break;
            default:
                return; // ignore
            }
            //    break;
            //}
        }
        
    }
}

