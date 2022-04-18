/*
 * LobbyJoin.cs
 * Join button for the lobby viewer.
 * 
 * Date created : 02/04/2022
 * Author       : Finn RAYMENT <rayment@etu.unistra.fr>
 */

using System.Collections;
using System.Collections.Generic;
using Monopoly.Util;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using Monopoly.Runtime;

namespace Monopoly.UI
{

    public class LobbyJoin : MonoBehaviour
    {

        public TMP_Text LobbyName;
        public Button JoinButton;
        public TMP_Text JoinText;
        public TMP_Text NumberPlayersText;
        public GameObject CreateMenuPrefab;

        [HideInInspector]
        public GameObject ParentMenu;

        [HideInInspector]
        public string Token;
        [HideInInspector]
        public int Num;
        [HideInInspector]
        public int Max;

        void Start()
        {
            JoinButton.onClick.AddListener(JoinGame);
            JoinText.text = StringLocaliser.GetString("join");
        }

        public void JoinGame()
        {
            // TODO: Add a way to use passwords
            ClientLobbyState.current.DoJoinGame(this.Token, "");
        }

    }

}
