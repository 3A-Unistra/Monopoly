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

using Monopoly.Graphics;

namespace Monopoly.UI
{

    // TODO: its still colliding with UI elements...
    public class BoardUI : MonoBehaviour
    {

        public BoardCardDisplay cardDisplay;

        private void DisplayCardPreview()
        {
            Ray cubeRay =
                UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] cubeHits = Physics.RaycastAll(cubeRay);
            bool rendered = false;
            foreach (RaycastHit ray in cubeHits)
            {
                GameObject obj = ray.collider.gameObject;
                SquareCollider collider = obj.GetComponent<SquareCollider>();
                if (collider != null)
                {
                    cardDisplay.Render(collider.squareIndex);
                    rendered = true;
                    break;
                }
            }
            if (!rendered)
                cardDisplay.Render(-1); // hide card renderer
        }

        void Update()
        {
            if (!UIDirector.IsMenuOpen && Input.GetMouseButtonDown(0))
                DisplayCardPreview();
        }

    }

}
