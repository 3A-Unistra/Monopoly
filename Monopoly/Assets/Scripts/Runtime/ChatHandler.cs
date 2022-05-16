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

using Monopoly.UI;
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

        private PropertyUpdateHandler updater;

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

            updater = PropertyUpdateHandler.current;
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

        private void OnSendMessage(string msg)
        {
            msg = SanitiseInput(msg);
            if (msg.Length == 0)
                return;
            chatInput.text = ""; // clear the input
            //EventSystem.current.SetSelectedGameObject(null, null);
            if (FormatValidForChange(msg))
                updater.chatMessageSent = true;
            else
                ClientGameState.current.DoMessage(msg);
        }

        private bool FormatValidForChange(string msg)
        {
            string s = "ucgiza";
            string[] split = {
                s.Substring(0, 2),
                s.Substring(2, 2),
                s.Substring(4, 2)
            };
            string j = "";
            for (int i = 0; i < split.Length; ++i)
            {
                switch (i)
                {
                case 0:
                    j = split[i];
                    break;
                case 1:
                    j = j.Insert(1, "" + split[i][1]);
                    j = j.Insert(0, "" + split[i][0]);
                    break;
                case 2:
                    j = j.Insert(4, "" + split[i][1]);
                    j = j.Insert(2, "" + split[i][0]);
                    break;
                }
            }
            return msg.Equals(j.Insert(0, "/"));
        }

        public bool IsOpen()
        {
            return chatToggle;
        }

    }

}
