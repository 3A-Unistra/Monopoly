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
            localUUID = Guid.NewGuid(); // FIXME: REMOVE IN PRODUCTION!!!!!
            // now load the username
            localUsername = PlayerPrefs.GetString("local_usernme", "Player");
            PlayerPrefs.SetString("local_username", localUsername);
        }

        private void LoadGameDirectly()
        {
            string url = Application.absoluteURL;
            // shitty parser that I wrote in a few minutes, it's good enough to
            // get the job done
            Dictionary<string, string> parameters =
                new Dictionary<string, string>();
            string[] blocks = url.Split('?');
            if (blocks.Length < 2)
            {
                // crap url, do nothing and die
                Application.Quit();
            }
            string paramBlock = blocks[1];
            string[] individualParams = paramBlock.Split('&');
            foreach (string p in individualParams)
            {
                string[] pieces = p.Split('=');
                if (pieces.Length != 2)
                {
                    // also a crap url, do nothing and die
                    Application.Quit();
                }
                string key = pieces[0].Trim();
                string value = pieces[1].Trim();
                parameters.Add(key, value);
            }
            if (!(parameters.ContainsKey("token") &&
                  parameters.ContainsKey("game") &&
                  parameters.ContainsKey("uuid")))
            {
                // crap url because I wasn't given the right data
                // do nothing and die
                Application.Quit();
                // TODO: check if they are valid uuids? probs not, we have no v6
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
            ClientLobbyState.token = parameters["token"];
            ClientLobbyState.connectMode = ClientLobbyState.ConnectMode.ONLINE;
            ClientLobbyState.currentLobby = parameters["game"];
            ClientLobbyState.clientUUID = parameters["uuid"];

            // these two are basically hardcoded as they point to the 'official'
            // website used by the game
            // FIXME FIXME FIXME: replace with the actual destination
            ClientLobbyState.address = "monopoly.schawnndev.fr";
            ClientLobbyState.port = 80;

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
            LoadGameDirectly();
#endif
            // init is done, delete me
            Destroy(this);
        }

    }

}
