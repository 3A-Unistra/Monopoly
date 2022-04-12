/*
 * MenuOptions.cs
 * This file contain the event listeners of the options' Menu buttons, slider and
 * switches.
 * 
 * Date created : 27/02/2022
 * Author       : Rayan MARMAR <rayan.marmar@etu.unistra.fr>
 *                Finn RAYMENT <rayment@etu.unistra.fr>
 *                Maxime MAIRE <maxime.maire2@etu.unistra.fr
 */

using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using Monopoly.Util;



//TODO LANGUAGES
//TODO DICE ANIMATION

namespace Monopoly.UI
{
    public class MenuOptions : MonoBehaviour
    {

        public GameObject PrefabPause;
        public GameObject OptionMenuPrefab;
        public GameObject MainMenuPrefab;
        
        public TMP_Dropdown ResolutionDropdown;
        public TMP_Dropdown QualityDropdown;
        public TMP_Dropdown LanguageDropdown;
        public Button AntialiasingButton;
        public Button ShadowButton;
        public TMP_Text ShadowText;
        public Button FullscreenButton;
        public Button AntialiasingFrontButton;
        public Button ShadowFrontButton;
        public Button FullscreenFrontButton;
        public TMP_Text FullscreenText;
        public Slider MusicSlider;
        public TMP_Text MusicText;
        public Slider SoundSlider;
        public TMP_Text SoundText;
        public Button Apply;
        public TMP_Text ApplyText;
        public Button Reset;
        public TMP_Text ResetText;
        public Button Close;
        public TMP_Text CloseText;
        private Resolution[] AvailableResolutions;

        public static bool ChangesApplied, Dirty, LanguageDirty;

        void Start()
        {
            BuildResolutions();
            BuildQuality();
            BuildLanguages();
            
            QualityDropdown.onValueChanged.AddListener(delegate { QualityChanging(); });
            LanguageDropdown.onValueChanged.AddListener(delegate { LanguageChanging(); });
            
            AntialiasingButton.onClick.AddListener(AntialiasingChanging);
            AntialiasingFrontButton.onClick.AddListener(AntialiasingChanging);
            FullscreenButton.onClick.AddListener(FullscreenChanging);
            FullscreenFrontButton.onClick.AddListener(FullscreenChanging);
            ShadowButton.onClick.AddListener(ShadowChanging);
            ShadowFrontButton.onClick.AddListener(ShadowChanging);

            MusicSlider.onValueChanged.AddListener(delegate { MusicChanging(); });
            SoundSlider.onValueChanged.AddListener(delegate { SoundChanging();  });

            Apply.onClick.AddListener(ApplyChanges);
            Reset.onClick.AddListener(delegate { ResetDefault(); });
            Close.onClick.AddListener(CloseMenu);
            #if UNITY_WEBGL
                ResolutionDropdown.gameObject.SetActive(false);
            #else
                ResolutionDropdown.onValueChanged.AddListener
                    (delegate { ResolutionChanging(); });
            #endif

            LanguageDirty = false;
            Dirty = false;
            
            ResolutionDropdown.value = PreferenceApply.Resolution;
            QualityDropdown.value = PreferenceApply.Quality;
            AntialiasingButton.GetComponent<OnOff>().switchOn = PreferenceApply.Antialiasing;
            ShadowButton.GetComponent<OnOff>().switchOn = PreferenceApply.Shadow;
            FullscreenButton.GetComponent<OnOff>().switchOn = PreferenceApply.Fullscreen;
            MusicSlider.value = PreferenceApply.Music;
            SoundSlider.value = PreferenceApply.Sound;
            
            
            if (AntialiasingButton.GetComponent<OnOff>().switchOn != PreferenceApply.Antialiasing)
                AntialiasingButton.GetComponent<OnOff>().Front.onClick.Invoke();
            if (ShadowButton.GetComponent<OnOff>().switchOn != PreferenceApply.Shadow)
                ShadowButton.GetComponent<OnOff>().Front.onClick.Invoke();
            if (FullscreenButton.GetComponent<OnOff>().switchOn != PreferenceApply.Fullscreen)
                FullscreenButton.GetComponent<OnOff>().Front.onClick.Invoke();
            
            ChangesApplied = true;
            Dirty = true;

            FullscreenText.text = StringLocaliser.GetString("fullscreen");
            ShadowText.text = StringLocaliser.GetString("shadow");
            SoundText.text = StringLocaliser.GetString("sound");
            MusicText.text = StringLocaliser.GetString("music");
            ApplyText.text = StringLocaliser.GetString("apply");
            ResetText.text = StringLocaliser.GetString("reset");
            CloseText.text = StringLocaliser.GetString("close");

            UIDirector.IsMenuOpen = true;
        }
        

        private void BuildQuality()
        {
            QualityDropdown.options.Clear();
            QualityDropdown.options.Add(new TMP_Dropdown.OptionData(StringLocaliser.GetString("low")));
            QualityDropdown.options.Add(new TMP_Dropdown.OptionData(StringLocaliser.GetString("medium")));
            QualityDropdown.options.Add(new TMP_Dropdown.OptionData(StringLocaliser.GetString("high")));
            QualityDropdown.value = 1; // default value is medium
        }
        private void BuildResolutions()
        {
            AvailableResolutions = Screen.resolutions;
    
            ResolutionDropdown.options.Clear();
            foreach (Resolution r in AvailableResolutions)
            {
                ResolutionDropdown.options.Add(new TMP_Dropdown.OptionData
                    (string.Format("{0}x{1} @ {2}", r.width, 
                        r.height, r.refreshRate)));
            }
            ResolutionDropdown.value = ResolutionDropdown.options.Count - 1;
        }

        private void BuildLanguages()
        {
            LanguageDropdown.options.Clear();
            foreach (string s in StringLocaliser.GetFriendlyLanguageList())
                LanguageDropdown.options.Add(new TMP_Dropdown.OptionData(s));
            LanguageDropdown.value = PreferenceApply.Language;
        }
        public void ResolutionChanging()
        {
            ChangesApplied = false;
            Screen.SetResolution
            (AvailableResolutions[ResolutionDropdown.value].width,
                AvailableResolutions[ResolutionDropdown.value].height, 
                Screen.fullScreen);
        }


        public void QualityChanging()
        {
            ChangesApplied = false;
            QualitySettings.SetQualityLevel(QualityDropdown.value, ShadowButton.GetComponent<OnOff>().switchOn);
        }
        
        public void ResetDefault()
        {
            ChangesApplied = false;
            ResolutionDropdown.value = ResolutionDropdown.options.Count - 1;
            QualityDropdown.value = 1;
            if (!AntialiasingButton.GetComponent<OnOff>().switchOn)
                AntialiasingButton.GetComponent<OnOff>().Front.onClick.Invoke();
            if (!ShadowButton.GetComponent<OnOff>().switchOn)
                ShadowButton.GetComponent<OnOff>().Front.onClick.Invoke();
            if (!FullscreenButton.GetComponent<OnOff>().switchOn)
                FullscreenButton.GetComponent<OnOff>().Front.onClick.Invoke();
            
            LanguageDropdown.value = PreferenceApply.Language;
            MusicSlider.value = 0.5f;
            SoundSlider.value = 0.8f;
            if (Dirty)
            {
                Screen.SetResolution(
                    AvailableResolutions[ResolutionDropdown.value].width,
                    AvailableResolutions[ResolutionDropdown.value].height,
                    true);
                QualitySettings.antiAliasing = 4;
                QualitySettings.SetQualityLevel(1, true);
                QualitySettings.shadows = ShadowQuality.All;
                Screen.fullScreen = true;
            }
        }

        public void AntialiasingChanging()
        {
            ChangesApplied = false;
            if (!AntialiasingButton.GetComponent<OnOff>().switchOn)
                QualitySettings.antiAliasing = 4;
            else
                QualitySettings.antiAliasing = 0;
        }
        
        public void ShadowChanging()
        {
            ChangesApplied = false;
            if (!ShadowButton.GetComponent<OnOff>().switchOn)
                QualitySettings.shadows = ShadowQuality.All; 
                 
            else
                QualitySettings.shadows = ShadowQuality.Disable;

        }
        
        public void FullscreenChanging()
        {
            ChangesApplied = false;
            Screen.fullScreen = !Screen.fullScreen;
        }
        
        public void MusicChanging()
        {
            ChangesApplied = false;
        }

        public void SoundChanging()
        {
            ChangesApplied = false;
        }

        public void LanguageChanging()
        {
            ChangesApplied = false;
            LanguageDirty = true;
        }
        
        public void ApplyChanges()
        {
            PreferenceApply.Resolution = ResolutionDropdown.value;
            PreferenceApply.Quality = QualityDropdown.value;
            PreferenceApply.Language = LanguageDropdown.value;
            PreferenceApply.Antialiasing = AntialiasingButton.GetComponent<OnOff>().switchOn;
            PreferenceApply.Shadow = ShadowButton.GetComponent<OnOff>().switchOn;
            PreferenceApply.Fullscreen = FullscreenButton.GetComponent<OnOff>().switchOn;
            PreferenceApply.Music = MusicSlider.value;
            PreferenceApply.Sound = SoundSlider.value;
            ChangesApplied = true;
            if (LanguageDirty)
            {
                PreferenceApply.ApplySettings();
                PreferenceApply.SaveSettings();
                GameObject optionMenu = Instantiate(OptionMenuPrefab, transform.parent);
                Destroy(this.gameObject);

            }
            else
            {
                PreferenceApply.SaveSettings();
            }
        }

        public void CloseMenu()
        {
            if (!ChangesApplied)
            {
                ResolutionDropdown.value = PreferenceApply.Resolution;
                QualityDropdown.value = PreferenceApply.Quality;
                LanguageDropdown.value = PreferenceApply.Language;
                MusicSlider.value = PreferenceApply.Music;
                SoundSlider.value = PreferenceApply.Sound;
                PreferenceApply.ApplySettings();
            }

            UIDirector.IsMenuOpen = false;
            if (MenuPause.OptionsOpenedFromPauseMenu)
            {
                MenuPause.OptionsOpenedFromPauseMenu = false;
                GameObject pauseMenu = Instantiate(PrefabPause, transform.parent);
            }
            else
            {
                Instantiate(MainMenuPrefab, transform.parent);
                MainMenu.OptionsOpened = false;
            }
            Destroy(this.gameObject);
        }
    }
}

