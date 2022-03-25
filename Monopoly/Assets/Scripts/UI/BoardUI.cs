using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monopoly.UI
{

    public class BoardUI : MonoBehaviour
    {

        public CardDisplay cardDisplay;

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
            if (Input.GetMouseButtonDown(0))
                DisplayCardPreview();
        }

    }

}
