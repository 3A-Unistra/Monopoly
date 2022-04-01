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

namespace Monopoly.UI
{
    public class MenuOptions : MonoBehaviour
    {

        public GameObject PrefabPause;
        public GameObject OptionsMenu;
        public GameObject PauseMenu;
        public TMP_Dropdown ResolutionDropdown;
        public TMP_Dropdown QualityDropdown;
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
        public static int Res;
        public static int Quality;
        public static bool FullScreen = true;
        public static bool Antialiasing = true;
        public static bool Shadow = true;
        public static float Music;
        public static float Sound;

        public static bool ChangesApplied, Dirty;

        void Start()
        {
            //PauseMenu = GameObject.Find("MenuPause");
            BuildResolutions();
            BuildQuality();
            ResolutionDropdown.onValueChanged.AddListener
                (delegate { ResolutionChanging(); });
            QualityDropdown.onValueChanged.AddListener(delegate { QualityChanging(); });

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


            Dirty = false;
            //OptionsMenu.GetComponent<RectTransform>().ForceUpdateRectTransforms();
            ResetDefault();
            ChangesApplied = true;
            Dirty = true;
            /*
            RectTransform rt = GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0.5f, 0.5f);
            rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.pivot = new Vector2(0.5f, 0.5f);
            rt.localPosition = Vector3.zero;*/
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
            //Dirty = true;
            Screen.SetResolution
            (AvailableResolutions[ResolutionDropdown.value].width,
                AvailableResolutions[ResolutionDropdown.value].height, 
                Screen.fullScreen);
        }


        public void QualityChanging()
        {
            ChangesApplied = false;
            //Dirty = true;
            QualitySettings.SetQualityLevel(QualityDropdown.value, true);
        }
        
        public void ResetDefault()
        {
            // TODO: scrap this and replace with a persistent config file loader
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
            //Dirty = true;
            if (!AntialiasingButton.GetComponent<OnOff>().switchOn)
                QualitySettings.antiAliasing = 4;
            else
                QualitySettings.antiAliasing = 0;
        }
        
        public void ShadowChanging()
        {
            ChangesApplied = false;
            //Dirty = true;

            
            if (!ShadowButton.GetComponent<OnOff>().switchOn)
                QualitySettings.shadows = ShadowQuality.All; 
                 
            else
                QualitySettings.shadows = ShadowQuality.Disable;

        }
        
        public void FullscreenChanging()
        {
            ChangesApplied = false;
            Screen.fullScreen = !Screen.fullScreen;
            //Dirty = true;
        }
        
        public void MusicChanging()
        {
            ChangesApplied = false;
            //Dirty = true;
        }

        public void SoundChanging()
        {
            ChangesApplied = false;
            //Dirty = true;
        }

        public void ApplyChanges()
        {
            // TODO: add a file write to the config file
            FullScreen = FullscreenButton.GetComponent<OnOff>().switchOn;
            Res = ResolutionDropdown.value;
            Quality = QualityDropdown.value;
            Antialiasing = AntialiasingButton.GetComponent<OnOff>().switchOn;
            Shadow = ShadowButton.GetComponent<OnOff>().switchOn;
            Music = MusicSlider.value;
            Sound = SoundSlider.value;
            ChangesApplied = true;
            //Dirty = false;
        }

        public void CloseMenu()
        {
            if (!ChangesApplied)
            {
                ResolutionDropdown.value = Res;
                QualityDropdown.value = Quality;
                if (AntialiasingButton.GetComponent<OnOff>().switchOn != Antialiasing)
                    AntialiasingButton.GetComponent<OnOff>().Front.onClick.Invoke();
                if (ShadowButton.GetComponent<OnOff>().switchOn != Shadow)
                    ShadowButton.GetComponent<OnOff>().Front.onClick.Invoke();
                if (FullscreenButton.GetComponent<OnOff>().switchOn != FullScreen)
                    FullscreenButton.GetComponent<OnOff>().Front.onClick.Invoke();
                MusicSlider.value = Music;
                SoundSlider.value = Sound;
                
                Screen.fullScreen = FullScreen;
                Screen.SetResolution(AvailableResolutions[Res].width, 
                    AvailableResolutions[Res].height, FullScreen);
                QualitySettings.SetQualityLevel(Quality, FullScreen);
                if (Antialiasing)
                    QualitySettings.antiAliasing = 4;
                else
                    QualitySettings.antiAliasing = 0;
                if (Shadow)
                    QualitySettings.shadows = ShadowQuality.All;
                else
                    QualitySettings.shadows = ShadowQuality.Disable;

            }

            PauseMenu.SetActive(true);
            OptionsMenu.SetActive(false);
        }
    }
}

