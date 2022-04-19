/*
 * PauseHelper.cs
 * UI helper script for the pause menu.
 * 
 * Date created : 03/04/2022
 * Author       : Rayan Marmar <rayan.marmar@etu.unistra.fr>
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Monopoly.UI
{
    
    [RequireComponent(typeof(Button))]
    public class PauseHelper : MonoBehaviour
    {

        public GameObject PrefabPause;
        public static bool MenuOpened;

        private GameObject pauseObject;

        public static PauseHelper current;

        void Start()
        {
            if (current != null)
            {
                Debug.LogWarning("Cannot instantiate multiple pause helpers!");
                Destroy(this.gameObject);
                return;
            }
            current = this;
            MenuOpened = false;
            GetComponent<Button>().onClick.AddListener(OpenPause);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!MenuOpened)
                    OpenPause();
                else
                    ClosePause();
            }
        }

        private void ClosePause()
        {
            if (MenuOpened && pauseObject != null)
            {
                Destroy(pauseObject);
                MenuOpened = false;
                UIDirector.IsMenuOpen = false;
            }
            else if (MenuOpened && MenuPause.OptionsOpenedFromPauseMenu)
            {
                // options escaped, open the pause menu
                Destroy(MenuOptions.current.gameObject);
                pauseObject = Instantiate(PrefabPause, transform.parent);
                UIDirector.IsMenuOpen = true;
            }
        }

        private void OpenPause()
        {
            if (!MenuOpened)
            {
                pauseObject = Instantiate(PrefabPause,transform.parent);
                MenuOpened = true;
                UIDirector.IsMenuOpen = true;
            }
        }

    }

}
