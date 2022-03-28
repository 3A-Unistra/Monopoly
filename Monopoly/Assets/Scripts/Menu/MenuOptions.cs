/*
 * MenuOptions.cs
 * This file contain the event listeners of the Meny buttons, slider and
 * switches.
 * 
 * Date created : 27/02/2022
 * Author       : Rayan MARMAR <rayan.marmar@etu.unistra.fr>
 *                Finn RAYMENT <rayment@etu.unistra.fr>
 */

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class MenuOptions : MonoBehaviour
{
    public TMP_Dropdown ResolutionDropdown;
    private Resolution[] AvailableResolutions;
    void Start()
    {
        ResolutionDropdown.onValueChanged.AddListener
            (delegate { ResolutionChanging();});
        BuildResolutions();
    }
    
    
    
    private void BuildResolutions()
    {
        AvailableResolutions = Screen.resolutions;

        ResolutionDropdown.options.Clear();
        foreach (Resolution Resolution in AvailableResolutions)
        {
            Debug.Log("Resolution " + Resolution.ToString() + " supported.");
            ResolutionDropdown.options.Add(new TMP_Dropdown.OptionData
                (string.Format("{0}x{1} @ {2}", Resolution.width, 
                    Resolution.height, Resolution.refreshRate)));
        }
        ResolutionDropdown.value = ResolutionDropdown.options.Count - 1;
    }
    void ResolutionChanging()
    {
        Screen.SetResolution
        (AvailableResolutions[ResolutionDropdown.value].width,
            AvailableResolutions[ResolutionDropdown.value].height, 
            Screen.fullScreen);
    }
}
