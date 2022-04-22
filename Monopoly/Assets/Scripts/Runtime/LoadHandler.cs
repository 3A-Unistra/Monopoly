/*
 * LoadHandler.cs
 * Loading screen handler.
 * 
 * Date created : 22/04/2022
 * Author       : Finn Rayment <rayment@etu.unistra.fr>
 */

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

using Monopoly.Util;

namespace Monopoly.Runtime
{

    public class LoadHandler : MonoBehaviour
    {

        public TMP_Text loadingText;

        private static string sceneToLoad;

        void Start()
        {
            loadingText.text = StringLocaliser.GetString("loading");
            SceneManager.LoadSceneAsync(sceneToLoad);
        }

        public static void LoadScene(string scene)
        {
            sceneToLoad = scene;
            SceneManager.LoadScene("Scenes/LoadingScene");
        }

    }

}
