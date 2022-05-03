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
using UnityEngine.EventSystems;
using UnityEngine.UI;

using Monopoly.Util;

namespace Monopoly.Runtime
{

    public class ChatHandler : MonoBehaviour
    {

        public GameObject chatView;
        public TMP_Text chatHistory;
        public TMP_InputField chatInput;
        public Button chatSend;
        public TMP_Text chatSendText;

        public Button toggleButton;

        private bool chatToggle;

        void Start()
        {
            chatHistory.text = "";
            chatInput.onSubmit.AddListener(OnSendMessage);
            chatSend.onClick.AddListener(
                delegate { OnSendMessage(chatInput.text); });
            toggleButton.onClick.AddListener(ToggleChat);
            chatToggle = PlayerPrefs.GetInt("chat_toggle", 0) == 1;
            chatSendText.text = StringLocaliser.GetString("send");
            chatInput.placeholder.GetComponent<TextMeshProUGUI>().text =
                StringLocaliser.GetString("enter_chat");
            if (chatToggle)
                ShowChat();
            else
                HideChat();
        }

        private void ToggleChat()
        {
            chatToggle = !chatToggle;
            ClientGameState.current.chatHelper.Clear();
            if (chatToggle)
                ShowChat();
            else
                HideChat();
        }

        private void HideChat()
        {
            chatView.gameObject.SetActive(false);
            PlayerPrefs.SetInt("chat_toggle", 0);
        }

        private void ShowChat()
        {
            chatView.gameObject.SetActive(true);
            PlayerPrefs.SetInt("chat_toggle", 1);
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
            EventSystem.current.SetSelectedGameObject(null, null);
            ClientGameState.current.DoMessage(msg);
        }

    }

}
