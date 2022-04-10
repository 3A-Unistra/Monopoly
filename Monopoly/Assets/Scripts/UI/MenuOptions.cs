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
        public TMP_Dropdown ResolutionDropdown;
        public TMP_Dropdown QualityDropdown;
        public TMP_Dropdown LanguageDropdown;
        public Button AntialiasingButton;
        public Button ShadowButton;
        public Button FullscreenButton;
        public Button AntialiasingFrontButton;
        public Button ShadowFrontButton;
        public Button FullscreenFrontButton;
        public Slider MusicSlider;
        public Slider SoundSlider;
        public Button Apply;
        public Button Reset;
        public Button Close;
        private Resolution[] AvailableResolutions;

        public static bool ChangesApplied, Dirty;

        void Start()
        {
            BuildResolutions();
            BuildQuality();
            
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
        
            
            RectTransform rt = GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0.5f, 0.5f);
            rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.pivot = new Vector2(0.5f, 0.5f);
            rt.localPosition = Vector3.zero;
        }
        

        private void BuildQuality()
        {
            QualityDropdown.options.Clear();
            QualityDropdown.options.Add(new TMP_Dropdown.OptionData("Low"));
            QualityDropdown.options.Add(new TMP_Dropdown.OptionData("Medium"));
            QualityDropdown.options.Add(new TMP_Dropdown.OptionData("High"));
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
        }
        
        public void ApplyChanges()
        {
            PreferenceApply.Resolution = ResolutionDropdown.value;
            PreferenceApply.Quality = QualityDropdown.value;
            PreferenceApply.Language = LanguageDropdown.value == 1 ? "english" : "french";
            PreferenceApply.Antialiasing = AntialiasingButton.GetComponent<OnOff>().switchOn;
            PreferenceApply.Shadow = ShadowButton.GetComponent<OnOff>().switchOn;
            PreferenceApply.Fullscreen = FullscreenButton.GetComponent<OnOff>().switchOn;
            PreferenceApply.Music = MusicSlider.value;
            PreferenceApply.Sound = SoundSlider.value;
            PreferenceApply.SaveSettings();
            ChangesApplied = true;
        }

        public void CloseMenu()
        {
            if (!ChangesApplied)
            {
                ResolutionDropdown.value = PreferenceApply.Resolution;
                QualityDropdown.value = PreferenceApply.Quality;
                LanguageDropdown.value = PreferenceApply.Language == "french" ? 1 : 0;
                MusicSlider.value = PreferenceApply.Music;
                SoundSlider.value = PreferenceApply.Sound;
                PreferenceApply.ApplySettings();
            }

            if (MenuPause.OptionsOpenedFromPauseMenu)
            {
                MenuPause.OptionsOpenedFromPauseMenu = false;
                GameObject pauseMenu = Instantiate(PrefabPause, transform.parent);
            }
            else
                MainMenu.OptionsOpened = false;
            Destroy(this.gameObject);
        }
    }
}

