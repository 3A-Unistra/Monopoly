/*
 * SquareCollider.cs
 * Collision mesh for card displaying.
 * 
 * Date created : 8/03/2022
 * Author       : Finn RAYMENT <rayment@etu.unistra.fr>
 */

using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using TMPro;

using Monopoly.Classes;
using Monopoly.Runtime;
using Monopoly.Util;

namespace Monopoly.UI
{

    [RequireComponent(typeof(BoxCollider))]
    public class SquareCollider : MonoBehaviour
    {

        [Range(0, 39)]
        public int squareIndex;

        private TMP_Text titleText, priceText, altText1, altText2;
        private RectTransform titleTrans, priceTrans, altTrans1, altTrans2;

        private void CreateTextObjects()
        {
            GameObject titleObj = new GameObject("Title");
            GameObject priceObj = new GameObject("Price");
            titleObj.transform.SetParent(this.transform);
            priceObj.transform.SetParent(this.transform);
            titleTrans = titleObj.AddComponent<RectTransform>();
            priceTrans = priceObj.AddComponent<RectTransform>();
            titleText = titleObj.AddComponent<TextMeshPro>();
            priceText = priceObj.AddComponent<TextMeshPro>();

            titleTrans.localScale = new Vector3(1f, 0.5f, 1f);
            titleTrans.localRotation = Quaternion.Euler(90f, 180f, 0f);
            priceTrans.localScale = new Vector3(1f, 0.5f, 1f);
            priceTrans.localRotation = Quaternion.Euler(90f, 180f, 0f);
            titleTrans.localPosition = new Vector3(0.5f, 0.2f, -0.25f);
            priceTrans.localPosition = new Vector3(0.5f, 0.2f, 0.328f);
            titleTrans.pivot = new Vector2(0f, 1f);
            priceTrans.pivot = new Vector2(0f, 1f);

            titleText.margin = new Vector4(0f, 0f, 19f, 4.8f);
            priceText.margin = new Vector4(0f, 0f, 19f, 4.8f);
            titleText.color = Color.black;
            priceText.color = Color.black;
            titleText.fontSize = 1.0f;
            priceText.fontSize = 1.0f;
            titleText.horizontalAlignment = HorizontalAlignmentOptions.Center;
            priceText.horizontalAlignment = HorizontalAlignmentOptions.Center;
            titleText.verticalAlignment = VerticalAlignmentOptions.Middle;
            priceText.verticalAlignment = VerticalAlignmentOptions.Middle;

            // square-specific text placements
            if (TaxSquare.IsTaxIndex(squareIndex))
            {
                titleText.fontSize = 1.7f;
                titleTrans.localPosition = new Vector3(0.5f, 0.2f, -0.34f);
                titleText.margin = new Vector4(0f, -0.27f, 19f, 4.8f);
            }
            else if (GoToJailSquare.IsGoToJailIndex(squareIndex))
            {
                titleText.characterSpacing = 5.76f;
                titleTrans.localPosition = new Vector3(-0.01f, 0.2f, 0.669f);
                titleTrans.localRotation = Quaternion.Euler(90f, 225f, -180f);
                titleTrans.localScale = new Vector3(1f, 1f, 1f);
                titleText.margin = new Vector4(0.17f, 0f, 19.2f, 4.8f);
                priceText.characterSpacing = 5.76f;
                priceTrans.localPosition = new Vector3(-0.517f, 0.2f, 0.236f);
                priceTrans.localRotation = Quaternion.Euler(90f, 225f, -180f);
                priceTrans.localScale = new Vector3(1f, 1f, 1f);
                priceText.margin = new Vector4(0.26f, 0.02f, 19.2f, 4.8f);
            }
            else if (FreeParkingSquare.IsFreeParkingIndex(squareIndex))
            {
                titleText.characterSpacing = 5.76f;
                titleTrans.localPosition = new Vector3(-0.669f, 0.2f, 0.01f);
                titleTrans.localRotation = Quaternion.Euler(90f, 225f, -90f);
                titleTrans.localScale = new Vector3(1f, 1f, 1f);
                titleText.margin = new Vector4(0.17f, 0f, 19.2f, 4.8f);
                priceText.characterSpacing = 5.76f;
                priceTrans.localPosition = new Vector3(-0.244f, 0.2f, -0.515f);
                priceTrans.localRotation = Quaternion.Euler(90f, 225f, -90f);
                priceTrans.localScale = new Vector3(1f, 1f, 1f);
                priceText.margin = new Vector4(0.26f, 0.02f, 19.2f, 4.8f);
            }
            else if (GoSquare.IsGoIndex(squareIndex))
            {
                titleText.characterSpacing = 1.5f;
                titleText.fontSize = 0.7f;
                titleTrans.localPosition = new Vector3(0.408f, 0.2f, -0.058f);
                titleTrans.localRotation = Quaternion.Euler(90f, 0f, 225f);
                titleTrans.localScale = new Vector3(1f, 1f, 1f);
                titleText.margin = new Vector4(-0.12f, 0f, 19.4f, 4.8f);
                priceText.fontSize = 2.08f;
                priceText.characterSpacing = -5.23f;
                priceTrans.localPosition = new Vector3(0.2f, 0.2f, 0.506f);
                priceTrans.localRotation = Quaternion.Euler(90f, 0f, 225f);
                priceTrans.localScale = new Vector3(1f, 0.75f, 1f);
                priceText.margin = new Vector4(0f, -0.52f, 19f, 4.98f);
                priceText.fontWeight = FontWeight.Bold;
            }
            else if (JailSquare.IsJailIndex(squareIndex))
            {
                titleTrans.localPosition = new Vector3(0.21f, 0.2f, -0.493f);
                titleTrans.localRotation = Quaternion.Euler(90f, 0f, 90f);
                titleTrans.localScale = new Vector3(1f, 0.75f, 1f);
                titleText.margin = new Vector4(0f, 0f, 19.3f, 4.8f);
                priceTrans.localPosition = new Vector3(0.194f, 0.2f, 0.211f);
                priceTrans.localRotation = Quaternion.Euler(90f, 0f, 180f);
                priceTrans.localScale = new Vector3(1f, 0.75f, 1f);
                priceText.margin = new Vector4(0f, 0f, 19.3f, 4.8f);

                // setup two more text fields for the interior of the jail
                GameObject altObj1 = new GameObject("Title");
                GameObject altObj2 = new GameObject("Price");
                altObj1.transform.SetParent(this.transform);
                altObj2.transform.SetParent(this.transform);
                altTrans1 = altObj1.AddComponent<RectTransform>();
                altTrans2 = altObj2.AddComponent<RectTransform>();
                altText1 = altObj1.AddComponent<TextMeshPro>();
                altText2 = altObj2.AddComponent<TextMeshPro>();

                altTrans1.localScale = new Vector3(1f, 0.75f, 1f);
                altTrans1.localRotation = Quaternion.Euler(90f, 0f, 135f);
                altTrans2.localScale = new Vector3(1f, 0.75f, 1f);
                altTrans2.localRotation = Quaternion.Euler(90f, 0f, 135f);
                altTrans1.localPosition = new Vector3(-0.258f, 0.2f, -0.5f);
                altTrans2.localPosition = new Vector3(0.007f, 0.2f, -0.063f);
                altTrans1.pivot = new Vector2(0f, 1f);
                altTrans2.pivot = new Vector2(0f, 1f);

                altText1.margin = new Vector4(0f, 0f, 19.66f, 4.86f);
                altText2.margin = new Vector4(-0.17f, 0f, 19.7f, 4.84f);
                altText1.color = Color.black;
                altText2.color = Color.black;
                altText1.fontSize = 1.0f;
                altText2.fontSize = 1.0f;
                altText1.horizontalAlignment = HorizontalAlignmentOptions.Center;
                altText2.horizontalAlignment = HorizontalAlignmentOptions.Center;
                altText1.verticalAlignment = VerticalAlignmentOptions.Middle;
                altText2.verticalAlignment = VerticalAlignmentOptions.Middle;
            }
        }

        public void UpdateText()
        {
            if (PropertySquare.IsPropertyIndex(squareIndex))
            {
                titleText.text = StringLocaliser.GetString(
                    string.Format("property{0}", squareIndex)).ToUpper();
                priceText.text = string.Format("{0} €",
                    ClientGameState.current.GetSquareDataIndex(squareIndex)
                    ["buy_price"].ToString());
            }
            else if (CompanySquare.IsCompanyIndex(squareIndex))
            {
                titleText.text = StringLocaliser.GetString(
                    string.Format("museum{0}", squareIndex)).ToUpper();
                priceText.text = string.Format("{0} €",
                    ClientGameState.current.GetSquareDataIndex(squareIndex)
                    ["buy_price"].ToString());
            }
            else if (StationSquare.IsStationIndex(squareIndex))
            {
                titleText.text = StringLocaliser.GetString(
                    string.Format("station{0}", squareIndex)).ToUpper();
                priceText.text = string.Format("{0} €",
                    ClientGameState.current.GetSquareDataIndex(squareIndex)
                    ["buy_price"].ToString());
            }
            else if (CommunitySquare.IsCommunityIndex(squareIndex))
            {
                titleText.text =
                    StringLocaliser.GetString("community").ToUpper();
                titleTrans.localPosition = new Vector3(0.5f, 0.2f, -0.465f);
                titleText.margin = new Vector4(0f, -0.07f, 19f, 4.76f);
            }
            else if (ChanceSquare.IsChanceIndex(squareIndex))
            {
                titleText.text =
                    StringLocaliser.GetString("chance").ToUpper();
                titleTrans.localPosition = new Vector3(0.5f, 0.2f, -0.465f);
                titleText.margin = new Vector4(0f, -0.07f, 19f, 4.76f);
            }
            else if (TaxSquare.IsTaxIndex(squareIndex))
            {
                if (squareIndex < 10) // the south tax
                {
                    titleText.text =
                        StringLocaliser.GetString("income_tax").ToUpper();
                }
                else // the east tax
                {
                    titleText.text =
                        StringLocaliser.GetString("luxury_tax").ToUpper();
                }
                priceText.text = string.Format("{0} €",
                    ClientGameState.current.GetSquareDataIndex(squareIndex)
                    ["value"].ToString());
            }
            else if (GoToJailSquare.IsGoToJailIndex(squareIndex))
            {
                titleText.text =
                    StringLocaliser.GetString("gotojail1").ToUpper();
                priceText.text =
                    StringLocaliser.GetString("gotojail2").ToUpper();
            }
            else if (FreeParkingSquare.IsFreeParkingIndex(squareIndex))
            {
                titleText.text =
                    StringLocaliser.GetString("freeparking1").ToUpper();
                priceText.text =
                    StringLocaliser.GetString("freeparking2").ToUpper();
            }
            else if (GoSquare.IsGoIndex(squareIndex))
            {
                titleText.text =
                    StringLocaliser.GetString("go1").ToUpper();
                priceText.text =
                    StringLocaliser.GetString("go2").ToUpper();
            }
            else if (JailSquare.IsJailIndex(squareIndex))
            {
                titleText.text =
                    StringLocaliser.GetString("jail_out1").ToUpper();
                priceText.text =
                    StringLocaliser.GetString("jail_out2").ToUpper();
                altText1.text =
                    StringLocaliser.GetString("jail_in1").ToUpper();
                altText2.text =
                    StringLocaliser.GetString("jail_in2").ToUpper();
            }
        }

        void Start()
        {
            CreateTextObjects();
            UpdateText();
        }

    }

}
