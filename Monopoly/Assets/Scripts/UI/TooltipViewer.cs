/*
 * TooltipViewer.cs
 * UI tooltip previewer.
 * 
 * Date created : 14/04/2022
 * Author       : Finn RAYMENT <rayment@etu.unistra.fr>
 */

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Monopoly.UI
{

    [RequireComponent(typeof(RectTransform))]
    public class TooltipViewer : MonoBehaviour
    {

        public static TooltipViewer current;

        public TMP_Text textField;
        public Canvas canvas;

        private RectTransform rect;

        static TooltipViewer()
        {
            current = null;
        }

        void Start()
        {
            if (current == null)
            {
                current = this;
            }
            else
            {
                Debug.LogWarning("Can't instantiate multiple tooltips!");
                Destroy(this.gameObject);
            }
            rect = GetComponent<RectTransform>();
            gameObject.SetActive(false);
        }

        void Update()
        {
            UpdatePosition();
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

        public void SetText(string txt)
        {
            textField.text = txt;
        }

    }

}
