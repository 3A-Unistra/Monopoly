/*
 * PreferenceApply.cs
 * This file contain the functions that set, get the player preferences
 * to apply it in the game options. 
 * 
 * Date created : 03/04/2022
 * Author       : Rayan MARMAR <rayan.marmar@etu.unistra.fr>
 */

using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UI;

using Monopoly.UI;

namespace Monopoly.Util
{
    public static class PreferenceApply
    {
        public static int Resolution { get; set; }
        public static int Quality { get; set; }
        public static string Language { get; set; }
        public static bool Fullscreen { get; set; }
        public static bool Shadow { get; set; }
        public static bool Antialiasing { get; set; }
        public static float Music { get; set; }
        public static float Sound { get; set; }

        private static Resolution[] ResolutionsArray = Screen.resolutions;
        public static void LoadSettings()
        {
            Resolution = PlayerPrefs.GetInt("resolution", ResolutionsArray.Length - 1);
            if (Resolution >= ResolutionsArray.Length)
                Resolution = 0;
            Quality = PlayerPrefs.GetInt("quality", 1);
            Language = PlayerPrefs.GetString("language", "french");
            Antialiasing = PlayerPrefs.GetInt("antialiasing", 1) == 1;
            Shadow = PlayerPrefs.GetInt("shadow", 1) == 1;
            Fullscreen = PlayerPrefs.GetInt("fullscreen", 1) == 1;
            Music = PlayerPrefs.GetFloat("music", 0.5f);
            Sound = PlayerPrefs.GetFloat("sound", 0.8f);
        }
    
        public static void ApplySettings()
        {
            if (ResolutionsArray.Length > 0)
            {
                Screen.SetResolution(ResolutionsArray[Resolution].width,
                        ResolutionsArray[Resolution].height, Fullscreen);
                QualitySettings.SetQualityLevel(Quality, true);
                StringLocaliser.SetLanguage(Language);
                Screen.fullScreen = Fullscreen;
                if (Shadow)
                    QualitySettings.shadows = ShadowQuality.All;
                else
                    QualitySettings.shadows = ShadowQuality.Disable;
                if (Antialiasing)
                    QualitySettings.antiAliasing = 4;
                else
                    QualitySettings.antiAliasing = 0;
                //TODO APPLY MUSIC PREFERENCES 
                //TODO APPLY SOUND PREFERENCES
            }
            else
            {
                // we're probably running in headless mode so we shouldn't apply
                // any of the settings just in case something acts up
            }
        }
        
        public static void SaveSettings()
        {
            PlayerPrefs.SetInt("resolution", Resolution);
            PlayerPrefs.SetInt("quality", Quality);
            PlayerPrefs.SetString("language", Language);
            PlayerPrefs.SetInt("antialiasing", Antialiasing ? 1 : 0);
            PlayerPrefs.SetInt("shadow", Shadow ? 1 : 0);
            PlayerPrefs.SetInt("fullscreen", Fullscreen ? 1 : 0);
            PlayerPrefs.SetFloat("music", Music);
            PlayerPrefs.SetFloat("sound", Sound);
        }
    }
}

