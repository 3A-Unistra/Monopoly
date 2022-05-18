using System.Collections;
using System.Collections.Generic;
using Monopoly.Util;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

using Monopoly.Net;
using Monopoly.Runtime;

namespace Monopoly.UI
{
    public class MenuLogin : MonoBehaviour
    {
        public Button MainMenuButton;
        public TMP_Text MainMenuText;
        public Button ConnectButton;
        public TMP_Text ConnectText;
        public TMP_InputField UsernameInput;
        public TMP_InputField PasswordInput;
        public GameObject ErrorTextField;
        public TMP_Text ErrorText;

        private ClientLobbyState connector = null;
        private bool connecting = false;

        // needed because the Uni server uses a self-signed certificate which
        // simply is not recognised by Unity on some platforms
        private class BypassCertificate : CertificateHandler
        {
            protected override bool ValidateCertificate(byte[] certificateData)
            {
                return true;
            }
        }

        void Start()
        {
            MainMenuButton.onClick.AddListener(ReturnToMainMenu);
            ConnectButton.onClick.AddListener(Connect);

            MainMenuText.text = StringLocaliser.GetString("main_menu");
            ConnectText.text = StringLocaliser.GetString("connect");
            UsernameInput.placeholder.GetComponent<TextMeshProUGUI>().text =
                StringLocaliser.GetString("username");
            PasswordInput.placeholder.GetComponent<TextMeshProUGUI>().text =
                StringLocaliser.GetString("password");

            ErrorTextField.SetActive(false);

            string defaultUsername =
                PlayerPrefs.GetString("favourite_username", "");
            UsernameInput.text = defaultUsername;

            UIDirector.IsMenuOpen = true;
            UIDirector.IsUIBlockingNet = false;
        }
        
        public void Connect()
        {
            string username = UsernameInput.text.Trim();
            string password = PasswordInput.text.Trim();
            if (username.Equals("") || password.Equals(""))
            {
                DisplayError("connection_nouser");
                return;
            }
            if (ClientLobbyState.current == null && !connecting)
            {
                connecting = true;
                StartCoroutine(Login(
                "mai-projet-integrateur.u-strasbg.fr/vmProjetIntegrateurgrp4-1"
                , 443, username, password));
            }
        }

        private IEnumerator Login(string address, int port, string username, string password)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("username", username);
            dic.Add("password", password);

            string json = JsonConvert.SerializeObject(dic);
            string addr = string.Format(
                "https://{0}/api/users/login",
                PacketSocket.SpliceAddress(address, port));

            Debug.Log(string.Format("Logging into server '{0}'...", addr));

            UnityWebRequest req = new UnityWebRequest(addr, "POST")
            {
                certificateHandler = new BypassCertificate()
            };
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
            req.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
            req.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            req.SetRequestHeader("Content-Type", "application/json");
            req.timeout = 5;

            yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.LogWarning("HTTP POST error: " + req.error);
                switch (req.responseCode)
                {
                case 403:
                    DisplayError("connection_badcreds");
                    break;
                default:
                    DisplayError("connection_fail");
                    break;
                }
                req.Dispose();
                connecting = false;
            }
            else
            {
                Dictionary<string, string> packet =
                    JsonLoader.LoadJson<Dictionary<string, string>>(req.downloadHandler.text);
                foreach (string key in packet.Keys)
                    Debug.Log(key + ": " + packet[key]);
                GameObject clientLobbyObject = new GameObject("ClientLobbyState");
                ClientLobbyState state =
                    clientLobbyObject.AddComponent<ClientLobbyState>();
                ClientLobbyState.secureMode = true;
                state.Canvas = transform.parent.gameObject;
                connector = state;
                PlayerPrefs.SetString("favourite_username", username);
                req.Dispose();
                state.StartCoroutine(
                    state.Connect(address, port,
                                  packet["userid"], packet["token"],
                                  this, ClientLobbyState.ConnectMode.ONLINE));
                connecting = false;
            }
        }

        public void ReturnToMainMenu()
        {
            // if we manage to return during a socket connection, then close the
            // socket as we leave
            if (connector != null)
            {
                connector.StopAllCoroutines();
                Destroy(connector);
            }

            UIDirector.IsMenuOpen = false;
            GameObject MainMenu =
                Instantiate(RuntimeData.current.MainMenuPrefab, transform.parent);
            Destroy(this.gameObject);
        }

        public void DisplayError(string error)
        {
            ErrorText.text = StringLocaliser.GetString(error);
            ErrorTextField.SetActive(true);
        }

    }
}

