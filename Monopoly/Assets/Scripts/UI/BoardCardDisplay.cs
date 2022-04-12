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

        private RectTransform rect;
        private Canvas canvas;

        void Start()
        {
            canvas = transform.parent.GetComponent<Canvas>();
            rect = GetComponent<RectTransform>();
            SetOwner(null);
            gameObject.SetActive(false);
        }

        void Update()
        {
            // hide card info if a menu appears
            if (UIDirector.IsMenuOpen)
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

        public void Render(int square)
        {
            Card.Render(square);
            if (PropertySquare.IsPropertyIndex(square) ||
                StationSquare.IsStationIndex(square) ||
                CompanySquare.IsCompanyIndex(square))
            {
                Card.gameObject.SetActive(true);
                OwnableSquare os = (OwnableSquare)
                    ClientGameState.current.Board.GetSquare(square);
                SetOwner(os.Owner);
                UpdatePosition();
                gameObject.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

    }

}
