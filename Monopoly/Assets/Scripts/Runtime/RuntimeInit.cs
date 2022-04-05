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

        private void LoadLanguage(string language)
        {
            // NOTE: PUT LANGUAGE MODULES HERE TO LOAD PLEASE
            StringLocaliser.LoadStrings("Locales/english", "english",
                                        "English");
            StringLocaliser.LoadStrings("Locales/french", "french",
                                        "Français");
            if (!StringLocaliser.SetLanguage(language))
            {
                language = "french";
                StringLocaliser.SetLanguage(language);
            }
            PlayerPrefs.SetString("language", language);
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
            string language = PlayerPrefs.GetString("language", "french");
            LoadLanguage(language);
            PreferenceApply.LoadSettings();
            PreferenceApply.ApplySettings();
            // this script is to be placed on a single object that will be
            // persistent throughout the entire runtime of the applet, that is,
            // it is never destroyed. this allows for global runtime handlers or
            // other important events to be handled 'statically'.
            DontDestroyOnLoad(gameObject);
            // init is done, delete me
            Destroy(this);
        }

    }

}
