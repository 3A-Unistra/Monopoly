/*
 * MenuExchange.cs
 * This file contain the event listeners of the exchange's Menu buttons.
 * 
 * Date created : 19/04/2022
 * Author       : Maxime MAIRE <maxime.maire2@etu.unistra.fr
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Monopoly.UI
{

    public class MenuExchange : MonoBehaviour
    {

        public GameObject MenuSelectCard;
        public Button SelectCardButton;
        public GameObject selectCardObject;

        public Button ValidCardButton;
        public Button ValidExchangeButton;
        public Button CancelButton;

        void Start()
        {
            SelectCardButton.onClick.AddListener(SelectCard);
            ValidCardButton.onClick.AddListener(ValidCard);
            ValidExchangeButton.onClick.AddListener(ValidExchange);
            CancelButton.onClick.AddListener(ResumeGame);
            UIDirector.IsMenuOpen = true;
        }

        private void SelectCard()
        {
            selectCardObject = Instantiate(MenuSelectCard, transform.parent);
        }

        private void ValidCard()
        {

        }

        private void ValidExchange()
        {

        }
        
        private void ResumeGame()
        {
            Destroy(this.gameObject);
            UIDirector.IsMenuOpen = false;
        }

    }
}
