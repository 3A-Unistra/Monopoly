using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Monopoly.UI
{
    
    [RequireComponent(typeof(Button))]
    public class PauseHelper : MonoBehaviour
    {
        public GameObject OptionsMenu;
        public GameObject PrefabPause;
        public Canvas canvas;
        public static bool MenuOpened;
        public GameObject PauseMenu;

        void Start()
        {
            OptionsMenu = GameObject.Find("MenuOptions");
            PauseMenu = GameObject.Find("MenuPause"); 
            PauseMenu.SetActive(false);
            OptionsMenu.SetActive(false);
            MenuOpened = false;
            GetComponent<Button>().onClick.AddListener(OpenPause);
        }

        private void OpenPause()
        {
            if (!MenuOpened)
            {
                //GameObject pauseMenu = Instantiate(PrefabPause);
                PauseMenu.SetActive(true);
                //pauseMenu.transform.SetParent(canvas.transform);
                //MenuOpened = true;
            }
        }

    }

}
