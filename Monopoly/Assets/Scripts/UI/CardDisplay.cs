/*
 * CardDisplay.cs
 * UI mouse-over card handler.
 * 
 * Date created : 11/03/2022
 * Author       : Finn RAYMENT <rayment@etu.unistra.fr>
 */

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Monopoly.Classes;
using Monopoly.Runtime;
using Monopoly.Util;

namespace Monopoly.UI
{

    [RequireComponent(typeof(RectTransform))]
    public class CardDisplay : MonoBehaviour
    {

        // TODO: NULL checks!

        private RectTransform rect;

        public Transform hierarchyProperty;
        public Transform hierarchyTram;
        public Transform hierarchyMuseum;

        // property UI pieces
        public TMP_Text titleProperty;
        public TMP_Text subtitleProperty;
        public RawImage propertyColor;
        public TMP_Text propertyRentBase;
        public TMP_Text[] propertyRents;
        public TMP_Text[] propertyRentTexts;
        public TMP_Text[] propertyRentSubtexts;
        public TMP_Text propertyMidText;
        public TMP_Text propertyHouseCost;
        public TMP_Text propertyHotelCost;
        public TMP_Text propertyMortgageValue;
        public TMP_Text propertyHouseCostText;
        public TMP_Text propertyHotelCostText;
        public TMP_Text propertyMortgageValueText;

        // tram UI pieces
        public TMP_Text titleTram;
        public TMP_Text[] tramRents;
        public TMP_Text[] tramRentTexts;
        public TMP_Text tramMortgageValue;
        public TMP_Text tramMortgageValueText;

        // museum UI pieces
        public TMP_Text titleMuseum;
        public TMP_Text museumText1;
        public TMP_Text museumText2;
        public TMP_Text museumMortgageValue;
        public TMP_Text museumMortgageValueText;

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

        private void ShowProperty(int idx)
        {
            float h, s, v;
            hierarchyProperty.gameObject.SetActive(true);
            hierarchyTram.gameObject.SetActive(false);
            hierarchyMuseum.gameObject.SetActive(false);
            gameObject.SetActive(true);
            string title = StringLocaliser.GetString(
                string.Format("property{0}", idx));
            titleProperty.text = title.ToUpper();
            // calculate the background colour as well as use a threshold of
            // HSV brightness to determine whether or not to show
            // black or white text
            propertyColor.color = PropertySquare.GetColorIndex(idx);
            Color.RGBToHSV(propertyColor.color, out h, out s, out v);
            if (v < 0.65f)
            {
                titleProperty.color = Color.white;
                subtitleProperty.color = Color.white;
            }
            else
            {
                titleProperty.color = Color.black;
                subtitleProperty.color = Color.black;
            }
            subtitleProperty.text =
                StringLocaliser.GetString("title_deed").ToUpper();
            // now update all of the rent data
            Dictionary<string, int> data =
                ClientGameState.current.GetSquareDataIndex(idx);
            propertyRentBase.text = data["rent_base"].ToString();
            for (int i = 0; i < propertyRents.Length; ++i)
            {
                propertyRents[i].text =
                    data[string.Format("rent_{0}", i+1)].ToString();
            }
            for (int i = 0; i < propertyRentTexts.Length; ++i)
            {
                propertyRentTexts[i].text =
                    StringLocaliser.GetString("rent").ToUpper();
            }
            for (int i = 0; i < propertyRentSubtexts.Length; ++i)
            {
                string str;
                if (i == 0)
                {
                    str = StringLocaliser.GetString("site_only");
                }
                else if (i == 1)
                {
                    str = StringLocaliser.GetString("with_1_house");
                }
                else if (i == propertyRentSubtexts.Length - 1)
                {
                    str = StringLocaliser.GetString("with_hotel");
                }
                else
                {
                    str = string.Format(
                        StringLocaliser.GetString("with_n_house"), i);
                }
                propertyRentSubtexts[i].text = str;
            }
            propertyMidText.text =
                StringLocaliser.GetString("property_longtext");
            propertyHouseCostText.text =
                StringLocaliser.GetString("cost_per_house");
            propertyHotelCostText.text =
                StringLocaliser.GetString("cost_per_hotel");
            propertyMortgageValueText.text =
                StringLocaliser.GetString("mortgage_value");
            propertyHouseCost.text = data["house_price"].ToString();
            propertyHotelCost.text = data["house_price"].ToString();
            propertyMortgageValue.text = (data["buy_price"] / 2).ToString();
        }

        private void ShowStation(int idx)
        {
            hierarchyProperty.gameObject.SetActive(false);
            hierarchyTram.gameObject.SetActive(true);
            hierarchyMuseum.gameObject.SetActive(false);
            gameObject.SetActive(true);
            string title = StringLocaliser.GetString(
                string.Format("station{0}", idx));
            titleTram.text = title.ToUpper();
            Dictionary<string, int> data =
                ClientGameState.current.GetSquareDataIndex(idx);
            int j = data["rent_base"];
            for (int i = 0; i < tramRents.Length; ++i)
            {
                string rentText;
                if (i == 0)
                {
                    tramRents[i].text = string.Format("€{0}", j.ToString());
                    rentText = StringLocaliser.GetString("rent");
                }
                else
                {
                    tramRents[i].text = j.ToString();
                    rentText = string.Format(
                        StringLocaliser.GetString("tram_condition"), i + 1);
                }
                tramRentTexts[i].text = rentText;
                j *= 2;
            }
            tramMortgageValueText.text =
                StringLocaliser.GetString("mortgage_value");
            tramMortgageValue.text = (data["buy_price"] / 2).ToString();
        }

        private void ShowMuseum(int idx)
        {
            hierarchyProperty.gameObject.SetActive(false);
            hierarchyTram.gameObject.SetActive(false);
            hierarchyMuseum.gameObject.SetActive(true);
            gameObject.SetActive(true);
            string title = StringLocaliser.GetString(
                string.Format("museum{0}", idx));
            titleMuseum.text = title.ToUpper();
            museumText1.text = StringLocaliser.GetString("museum_longtext1");
            museumText2.text = StringLocaliser.GetString("museum_longtext2");
            museumMortgageValueText.text =
                StringLocaliser.GetString("mortgage_value");
            Dictionary<string, int> data =
                ClientGameState.current.GetSquareDataIndex(idx);
            museumMortgageValue.text = (data["buy_price"] / 2).ToString();
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
