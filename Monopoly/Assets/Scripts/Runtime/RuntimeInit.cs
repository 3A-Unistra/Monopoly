/*
 * RuntimeInit.cs
 * Applet initialisation script.
 * 
 * Date created : 31/03/2022
 * Author       : Finn RAYMENT <rayment@etu.unistra.fr>
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Monopoly.Util;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace Monopoly.Runtime
{

    public class RuntimeInit : MonoBehaviour
    {

        private static bool init = false;

        public static Dictionary<string, string> data;

        public static Guid localUUID;
        public static string localUsername;

        public static GameObject persistentGameObject;

        private void LoadLanguage(int language)
        {
            // NOTE: PUT LANGUAGE MODULES HERE TO LOAD PLEASE
            StringLocaliser.LoadStrings("Locales/english", "english",
                                        "English");
            StringLocaliser.LoadStrings("Locales/french", "french",
                                        "Français");
            language = Mathf.Clamp(language, 0, StringLocaliser.GetLanguageList().Length-1);
            if (!StringLocaliser.SetLanguage(StringLocaliser.GetLanguageList()[language]))
            {
                language = 0;
                StringLocaliser.SetLanguage(StringLocaliser.GetLanguageList()[language]);
            }
            PlayerPrefs.SetInt("language", language);
        }

        private void LoadLocalClientInfo()
        {
            // load local UUID
            string uuid =
                PlayerPrefs.GetString("local_uuid", Guid.NewGuid().ToString());
            if (!Guid.TryParse(uuid, out localUUID))
                localUUID = Guid.NewGuid();
            PlayerPrefs.SetString("local_uuid", localUUID.ToString());
            localUUID = Guid.NewGuid();
            // now load the username
            localUsername = PlayerPrefs.GetString("local_usernme", "Player");
            PlayerPrefs.SetString("local_username", localUsername);
        }

        private void WebGLNuke()
        {
            // first we need to completely nuke the scene to stop anything and
            // everything else from loading in the menu scene - we want this
            // script to run and exactly nothing else
            foreach (Transform obj in FindObjectsOfType<Transform>())
            {
                if (obj != this.transform)
                {
                    // destroy the transform because it isn't ours
                    Destroy(obj.gameObject);
                }
            }
            // now we are the only thing that remains and can patiently wait for
            // the bootstrap to occur
            Debug.Log("WebGL has been nuked!");
        }

        public void WebGLBootstrap(string paramBlock)
        {
            // this function will be called from the webbrowser in a message
            // request to bootstrap the rest of the application into opening the
            // game scene with the correct values
            Debug.Log(string.Format("Bootstrapping WebGL with param block: {0}",
                                    paramBlock));

            Dictionary<string, string> paramDic =
                JsonLoader.LoadJson<Dictionary<string, string>>(paramBlock);

            if (!(paramDic.ContainsKey("ip") &&
                  paramDic.ContainsKey("port") &&
                  paramDic.ContainsKey("token") &&
                  paramDic.ContainsKey("game") &&
                  paramDic.ContainsKey("uuid") &&
                  paramDic.ContainsKey("online") &&
                  paramDic.ContainsKey("secure")))
            {
                // crap json data because I wasn't given the right data
                // do nothing and die
                Application.Quit();
            }
            // okay so now we have a URL that is at least valid enough to start
            // the damn game. normally we pass through ClientLobbyState and init
            // a bunch of static variables which are later accessed in
            // ClientGameState to kickstart the socket connection.
            // adding a tonne of if branches and other code is a crappy solution
            // in the game state so instead we can just bootstrap the same
            // static variables that are normally set through the main menu
            // scene and load the game scene directly which will then call these
            // variables as if it was just opened from the menu scene and not
            // a random webpage

            ClientLobbyState.address = paramDic["ip"].Trim();
            if (!int.TryParse(paramDic["port"].Trim(),
                              out ClientLobbyState.port))
            {
                // crap port so die
                Application.Quit();
            }
            ClientLobbyState.token = paramDic["token"].Trim();
            ClientLobbyState.connectMode =
                paramDic["online"].Trim().Equals("1") ?
                    ClientLobbyState.ConnectMode.ONLINE :
                    ClientLobbyState.ConnectMode.BYIP;
            ClientLobbyState.currentLobby = paramDic["game"].Trim();
            ClientLobbyState.clientUUID = paramDic["uuid"].Trim();
            ClientLobbyState.secureMode = paramDic["secure"].Trim().Equals("1");

            Debug.Log("WebGL has been bootstrapped!");

            // alright, everything should be bootstrapped, time to load the
            // board scene and cross our fingers!
            LoadHandler.LoadScene("Scenes/BoardScene");
        }

        void Awake()
        {
            if (init)
            {
                // if this is attempted more than once, delete the script and
                // its object
                Destroy(gameObject);
                return;
            }
            persistentGameObject = this.gameObject;
            init = true;
            int language = PlayerPrefs.GetInt("language", 0);
            LoadLanguage(language);
#if !UNITY_WEBGL
            LoadLocalClientInfo();
#endif
            PreferenceApply.LoadSettings();
            PreferenceApply.ApplySettings();
            data = JsonLoader.LoadJsonAsset
                <Dictionary<string, string>>("Data/data");
            // this script is to be placed on a single object that will be
            // persistent throughout the entire runtime of the applet, that is,
            // it is never destroyed. this allows for global runtime handlers or
            // other important events to be handled 'statically'.
            DontDestroyOnLoad(gameObject);

#if UNITY_WEBGL
            WebGLNuke();
#else
            // init is done, delete me
            Destroy(this);
#endif
        }

    }

}
