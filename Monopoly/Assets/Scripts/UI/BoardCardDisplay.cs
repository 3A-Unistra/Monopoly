/*
 * BoardCardDisplay.cs
 * Board-mode UI mouse-over card handler.
 * 
 * Date created : 12/04/2022
 * Author       : Finn RAYMENT <rayment@etu.unistra.fr>
 */

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using Monopoly.Classes;
using Monopoly.Runtime;
using Monopoly.Util;

namespace Monopoly.UI
{

    public class BoardCardDisplay : MonoBehaviour
    {

        public CardDisplay Card;

        public TMP_Text OwnerText;
        public TMP_Text OwnerName;

        public Button buyHouseButton;
        public Button sellHouseButton;
        public Button mortgageButton;
        public Button unmortgageButton;

        public bool rendering;
        private int square;

        private RectTransform rect;
        private Canvas canvas;

        public static BoardCardDisplay current;

        void Start()
        {
            if (current != null)
            {
                Debug.LogWarning("Cannot create multiple board card displays!");
                Destroy(this.gameObject);
                return;
            }
            current = this;

            canvas = transform.parent.GetComponent<Canvas>();
            rect = GetComponent<RectTransform>();
            SetOwner(null);
            buyHouseButton.onClick.AddListener(delegate
            {
                ClientGameState.current.DoBuyHouse(square);
            });
            sellHouseButton.onClick.AddListener(delegate
            {
                ClientGameState.current.DoSellHouse(square);
            });
            mortgageButton.onClick.AddListener(delegate
            {
                ClientGameState.current.DoMortgageProperty(square);
            });
            unmortgageButton.onClick.AddListener(delegate
            {
                ClientGameState.current.DoUnmortgageProperty(square);
            });
            rendering = false;
            gameObject.SetActive(false);
        }

        void Update()
        {
            // hide card info if a menu appears
            if (UIDirector.IsMenuOpen || UIDirector.IsGameMenuOpen)
                gameObject.SetActive(false);
        }

        public void SetOwner(Player p)
        {
            OwnerText.text = StringLocaliser.GetString("card_owner");
            if (p == null)
                OwnerName.text = StringLocaliser.GetString("nobody");
            else
                OwnerName.text = ClientGameState.PlayerNameLoggable(p);
        }

        private void UpdatePosition()
        {
            int w = Screen.width;
            int h = Screen.height;
            Vector3 mp = Input.mousePosition;
            Vector2 size =
                rect.sizeDelta * rect.localScale * canvas.scaleFactor;
            Vector2 origin = new Vector2(mp.x, mp.y);
            // top bottom selector
            if (mp.y >= h - size.y && origin.y - size.y > 0)
                origin.y -= size.y;
            // left right selector
            if (mp.x >= w - size.x && origin.x - size.x > 0)
                origin.x -= size.x;
            rect.position = new Vector2(origin.x, origin.y);
        }

        public void Redraw()
        {
            Render(this.square, false);
        }

        public void Render(int square, bool updatePosition)
        {
            Card.Render(square);
            this.square = square;
            if (PropertySquare.IsPropertyIndex(square) ||
                StationSquare.IsStationIndex(square) ||
                CompanySquare.IsCompanyIndex(square))
            {
                Card.gameObject.SetActive(true);
                OwnableSquare os = (OwnableSquare)
                    ClientGameState.current.Board.GetSquare(square);
                SetOwner(os.Owner);
                bool canMortgage = false;
                bool canBuy = false;
                bool canSell = false;
                bool canUnmortgage = false;
                if (ClientGameState.current.CanPerformAction &&
                    os.Owner == ClientGameState.current.myPlayer)
                {
                    canMortgage = !os.Mortgaged;
                    canUnmortgage = os.Mortgaged;
                    if (PropertySquare.IsPropertyIndex(square))
                    {
                        PropertySquare ps = (PropertySquare)os;
                        if (ClientGameState.current.Board.OwnSameColorSet
                            (ps.Owner, ps) && !ps.Mortgaged)
                        {
                            canBuy = ps.NbHouse < 5;
                            canSell = ps.NbHouse > 0;
                        }
                    }
                }
                buyHouseButton.gameObject.SetActive(canBuy);
                sellHouseButton.gameObject.SetActive(canSell);
                mortgageButton.gameObject.SetActive(canMortgage);
                unmortgageButton.gameObject.SetActive(canUnmortgage);

                if (updatePosition)
                    UpdatePosition();
                rendering = true;
                gameObject.SetActive(true);
            }
            else
            {
                rendering = false;
                gameObject.SetActive(false);
            }
        }

    }

}
