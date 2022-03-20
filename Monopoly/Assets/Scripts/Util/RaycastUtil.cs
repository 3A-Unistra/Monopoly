/*
 * RaycastUtil.cs
 * Utility methods for raycasting.
 * 
 * Date created : 07/03/2022
 * Author       : Finn RAYMENT <rayment@etu.unistra.fr>
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Monopoly.Util
{

    /**
     * <summary>
     *     Utility methods for detecting or obtaining raycast results.
     * </summary>
     */
    public static class RaycastUtil
    {

        /*
         * See: http://answers.unity.com/answers/1653313/view.html
         */
        /**
         * <summary>
         *     Get a list of all raycasts related to the Unity Event System from
         *     the mouse-pointer.
         * </summary>
         * <returns>
         *     A list of all raycasts from the mouse to Event System elements.
         * </returns>
         */
        public static List<RaycastResult> GetRaycastEventSystemMouse()
        {
            PointerEventData ed = new PointerEventData(EventSystem.current);
            ed.position = Input.mousePosition;
            List<RaycastResult> res = new List<RaycastResult>();
            EventSystem.current.RaycastAll(ed, res);
            return res;
        }

        /**
         * <summary>
         *     Check whether the mouse-pointer is currently hovering over
         *     (checked via. a raycast) any element of the Event System of a
         *     given layer.
         * </summary>
         * <param name="layer">
         *     The layer to check for.
         * </param>
         * <returns>
         *     <c>true</c> if the mouse is hovering over an element of a given
         *     layer.
         * </returns>
         */
        public static bool IsMouseRaycast(string layer)
        {
            List<RaycastResult> rays = GetRaycastEventSystemMouse();
            foreach (RaycastResult ray in rays)
            {
                if (ray.gameObject.layer == LayerMask.NameToLayer(layer))
                    return true;
            }
            return false;
        }

    }

}
