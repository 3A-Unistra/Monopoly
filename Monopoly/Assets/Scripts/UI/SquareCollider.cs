/*
 * SquareCollider.cs
 * Collision mesh for card displaying.
 * 
 * Date created : 8/03/2022
 * Author       : Finn RAYMENT <rayment@etu.unistra.fr>
 */

using System.Collections;
using System.Collections.Generic;
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

        public int squareIndex;

        private TMP_Text titleText;
        private TMP_Text priceText;

        private void CreateTextObjects()
        {
            GameObject titleObj = new GameObject("Title");
            GameObject priceObj = new GameObject("Price");
            RectTransform titleTrans, priceTrans;
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
            titleTrans.localPosition = new Vector3(0.49f, 0.2f, -0.25f);
            priceTrans.localPosition = new Vector3(0.49f, 0.2f, 0.328f);
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
        }

        public void UpdateText()
        {
            if (PropertySquare.IsPropertyIndex(squareIndex))
            {
                titleText.text = StringLocaliser.GetString(
                    string.Format("property{0}", squareIndex)).ToUpper();
                priceText.text =
                    string.Format("${0}", 
                    ClientGameState.current.GetSquareDataIndex(squareIndex)
                    ["buy_price"]);
            }
            else if (CompanySquare.IsCompanyIndex(squareIndex))
            {
                titleText.text = StringLocaliser.GetString(
                    string.Format("museum{0}", squareIndex)).ToUpper();
                priceText.text =
                    string.Format("${0}",
                    ClientGameState.current.GetSquareDataIndex(squareIndex)
                    ["buy_price"]);
            }
            else if (StationSquare.IsStationIndex(squareIndex))
            {
                titleText.text = StringLocaliser.GetString(
                    string.Format("station{0}", squareIndex)).ToUpper();
                priceText.text =
                    string.Format("${0}",
                    ClientGameState.current.GetSquareDataIndex(squareIndex)
                    ["buy_price"]);
            }
        }

        void Start()
        {
            CreateTextObjects();
            UpdateText();
            StartCoroutine(switchLang());
        }

        IEnumerator switchLang()
        {
            yield return new WaitForSeconds(5);
            StringLocaliser.SetLanguage("english");
            UpdateText();
        }

    }

}
