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

        void Start()
        {
            MenuOpened = false;
            GetComponent<Button>().onClick.AddListener(OpenPause);
        }

        private void OpenPause()
        {
            if (!MenuOpened)
            {
                GameObject pauseMenu = Instantiate(PrefabPause,transform.parent);
                MenuOpened = true;
            }
        }

    }

}
