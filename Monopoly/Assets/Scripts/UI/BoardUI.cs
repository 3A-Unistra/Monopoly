/*
 * BoardUI.cs
 * Board interaction handler.
 * 
 * Date created : 25/03/2022
 * Author       : Finn RAYMENT <rayment@etu.unistra.fr>
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using Monopoly.Graphics;
using Monopoly.Util;

namespace Monopoly.UI
{

    public class BoardUI : MonoBehaviour
    {

        public BoardCardDisplay cardDisplay;

        private void DisplayCardPreview()
        {
            Ray cubeRay =
                UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] cubeHits = Physics.RaycastAll(cubeRay);
            bool rendered = false;
            List<RaycastResult> uiRaycasts =
                RaycastUtil.GetRaycastEventSystemMouse();
            if (uiRaycasts.Count > 0)
            {
                // the raycast hit a UI element so ignore the display
                // request completely as it isn't valid
                return;
            }
            foreach (RaycastHit ray in cubeHits)
            {
                GameObject obj = ray.collider.gameObject;
                SquareCollider collider = obj.GetComponent<SquareCollider>();
                if (collider != null)
                {
                    cardDisplay.Render(collider.squareIndex, true);
                    rendered = true;
                    break;
                }
            }
            if (!rendered)
                cardDisplay.Render(-1, false); // hide card renderer
        }

        void Update()
        {
            if (!UIDirector.IsMenuOpen && Input.GetMouseButtonDown(0))
                DisplayCardPreview();
        }

    }

}
