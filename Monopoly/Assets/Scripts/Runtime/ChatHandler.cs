/*
 * ChatHandler.cs
 * Chat window handler.
 * 
 * Date created : 07/04/2022
 * Author       : Finn Rayment <rayment@etu.unistra.fr>
 */

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Monopoly.Runtime
{

    public class ChatHandler : MonoBehaviour
    {

        public TMP_Text chatHistory;
        public TMP_InputField chatInput;
        public Button chatSend;

        void Start()
        {
            chatHistory.text = "";
            chatInput.onSubmit.AddListener(OnSendMessage);
            chatSend.onClick.AddListener(
                delegate { OnSendMessage(chatInput.text); });
        }

        private string SanitiseInput(string msg)
        {
            // MAX 128 CHARS PER MESSAGE
            msg = msg.Trim();
            msg = msg.Replace("<", "");
            msg = msg.Replace(">", "");
            msg = msg.Substring(0, Mathf.Min(msg.Length, 128));
            return msg;
        }

        void OnSendMessage(string msg)
        {
            msg = SanitiseInput(msg);
            if (msg.Length == 0)
                return;
            chatInput.text = ""; // clear the input
            ClientGameState.current.DoMessage(msg);
        }

    }

}
