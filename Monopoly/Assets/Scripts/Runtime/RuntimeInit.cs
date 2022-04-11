/*
 * RuntimeInit.cs
 * Applet initialisation script.
 * 
 * Date created : 31/03/2022
 * Author       : Finn RAYMENT <rayment@etu.unistra.fr>
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Monopoly.Util;

namespace Monopoly.Runtime
{

    public class RuntimeInit : MonoBehaviour
    {

        private static bool init = false;
        public GameObject MainMenuPrefab;
        public GameObject Canvas;

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

        void Awake()
        {
            if (init)
            {
                // if this is attempted more than once, delete the script and
                // its object
                Destroy(gameObject);
                return;
            }
            init = true;
            int language = PlayerPrefs.GetInt("language", 0);
            LoadLanguage(language);
            PreferenceApply.LoadSettings();
            PreferenceApply.ApplySettings();
            // this script is to be placed on a single object that will be
            // persistent throughout the entire runtime of the applet, that is,
            // it is never destroyed. this allows for global runtime handlers or
            // other important events to be handled 'statically'.
            DontDestroyOnLoad(gameObject);
            // start the menu
            Instantiate(MainMenuPrefab, Canvas.transform);
            // init is done, delete me
            Destroy(this);
        }

    }

}
