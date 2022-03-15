/*
 * CardDisplay.cs
 * UI mouse-over card handler.
 * 
 * Date created : 11/03/2022
 * Author       : Finn RAYMENT <rayment@etu.unistra.fr>
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using Monopoly.Classes;
using Monopoly.Util;

namespace Monopoly.UI
{

    [RequireComponent(typeof(RectTransform))]
    public class CardDisplay : MonoBehaviour
    {

        private RectTransform rect;

        public Transform hierarchyProperty;
        public Transform hierarchyTram;
        public Transform hierarchyMuseum;

        public TMP_Text titleProperty;
        public TMP_Text titleTram;
        public TMP_Text titleMuseum;

        void Start()
        {
            rect = GetComponent<RectTransform>();
            HideAll();
        }

        private void HideAll()
        {
            hierarchyProperty.gameObject.SetActive(false);
            hierarchyTram.gameObject.SetActive(false);
            hierarchyMuseum.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }

        private void UpdatePosition()
        {
            int w = Screen.width;
            int h = Screen.height;
            Vector3 mp = Input.mousePosition;
            Vector2 size = rect.sizeDelta;
            Vector2 origin = new Vector2(mp.x, mp.y);
            // top bottom selector
            if (mp.y - size.y < 0)
                origin.y += size.y + 10;
            else
                origin.y -= 10;
            // left right selector
            if (mp.x >= w - size.x)
                origin.x -= size.x - 10;
            else
                origin.x += 10;
            rect.position = new Vector2(origin.x, origin.y);
        }

        private void ShowProperty(int idx)
        {
            hierarchyProperty.gameObject.SetActive(true);
            hierarchyTram.gameObject.SetActive(false);
            hierarchyMuseum.gameObject.SetActive(false);
            gameObject.SetActive(true);
            string title = StringLocaliser.GetString(
                string.Format("property{0}", idx));
            titleProperty.text = title;
            UpdatePosition();
        }

        private void ShowStation(int idx)
        {
            hierarchyProperty.gameObject.SetActive(false);
            hierarchyTram.gameObject.SetActive(true);
            hierarchyMuseum.gameObject.SetActive(false);
            gameObject.SetActive(true);
            string title = StringLocaliser.GetString(
                string.Format("station{0}", idx));
            titleTram.text = title;
            UpdatePosition();
        }

        private void ShowMuseum(int idx)
        {
            hierarchyProperty.gameObject.SetActive(false);
            hierarchyTram.gameObject.SetActive(false);
            hierarchyMuseum.gameObject.SetActive(true);
            gameObject.SetActive(true);
            string title = StringLocaliser.GetString(
                string.Format("museum{0}", idx));
            titleMuseum.text = title;
            UpdatePosition();
        }

        public void Render(int square)
        {
            if (PropertySquare.IsPropertyIndex(square))
                ShowProperty(square);
            else if (StationSquare.IsStationIndex(square))
                ShowStation(square);
            else if (CompanySquare.IsCompanyIndex(square))
                ShowMuseum(square);
            else
                HideAll();
        }

    }

}
