/*
 * UIDirector.cs
 * Static UI direction script.
 * 
 * Date created : 12/04/2022
 * Author       : Finn RAYMENT <rayment@etu.unistra.fr>
 */

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using Monopoly.Util;

namespace Monopoly.UI
{

    public static class UIDirector
    {

        /**
         * <summary>
         *     Boolean property that equates to <c>true</c> if and only if a
         *     menu is currently open.
         * </summary>
         */
        public static bool IsMenuOpen { get; set; }

        /**
         * <summary>
         *     Boolean property that equates to <c>true</c> if and only if a
         *     game menu (exchanges, auctions, etc.) is currently open.
         * </summary>
         */
        public static bool IsGameMenuOpen { get; set; }

        /**
         * <summary>
         *     Boolean property that equates to <c>true</c> if and only if a
         *     menu is trying to open and thus blocking net packets.
         * </summary>
         */
        public static bool IsUIBlockingNet { get; set; }

        // http://answers.unity.com/answers/1797644/view.html
        public static bool IsEditingInputField()
        {
            if (EventSystem.current != null)
            {
                GameObject currentFocus = EventSystem.current.currentSelectedGameObject;
                if (currentFocus != null)
                    return currentFocus.TryGetComponent(out TMP_InputField _);
                else
                    return false;
            }
            else
            {
                return false;
            }
        }

        public static bool IsOverScrollView()
        {
            List<RaycastResult> rays = RaycastUtil.GetRaycastEventSystemMouse();
            foreach (RaycastResult ray in rays)
            {
                if (ray.gameObject.TryGetComponent<ScrollRect>
                        (out ScrollRect _))
                    return true;
            }
            return false;
        }

    }

}
