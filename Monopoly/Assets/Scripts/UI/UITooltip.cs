/*
 * UITooltip.cs
 * UI tooltip trigger.
 * 
 * Date created : 14/04/2022
 * Author       : Finn RAYMENT <rayment@etu.unistra.fr>
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using Monopoly.Util;

namespace Monopoly.UI
{

    public class UITooltip : MonoBehaviour,
                             IPointerEnterHandler, IPointerExitHandler
    {

        /**
         * <summary>
         *     Localisation string that describes the tooltip.
         * </summary>
         */
        public string LocaleText;

        private string text;

        private static UITooltip current;

        static UITooltip()
        {
            current = null;
        }

        void Start()
        {
            text = StringLocaliser.GetString(LocaleText);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            //if (current != null)
            //    return;
            //current = this;
            TooltipViewer viewer = TooltipViewer.current;
            if (viewer != null)
            {
                viewer.SetText(text);
                viewer.gameObject.SetActive(true);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            current = null;
            TooltipViewer viewer = TooltipViewer.current;
            if (viewer != null)
                viewer.gameObject.SetActive(false);
        }
    }

}
