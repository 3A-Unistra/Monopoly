using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Monopoly.UI
{
    public class MenuLobby : MonoBehaviour
    {
        public TMP_InputField TokenField;
        public Button SearchButton;
        public Button CreateButton;
        public Button MainMenuButton;

        public GameObject MainMenuPrefab;
        void Start()
        {
            MainMenuButton.onClick.AddListener(ReturnToMainMenu);
            SearchButton.onClick.AddListener(SearchToken);
            CreateButton.onClick.AddListener(CreateLobby);
        }

        public void SearchToken()
        {
            string txt = TokenField.text;
            TokenField.text = "";
        }
        
        public void ReturnToMainMenu()
        {
            GameObject MainMenu = Instantiate(MainMenuPrefab, transform.parent);
            Destroy(this.gameObject);
        }

        public void CreateLobby()
        {
            
        }
    }
}

