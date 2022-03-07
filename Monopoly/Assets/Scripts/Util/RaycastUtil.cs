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

    public static class RaycastUtil
    {

        /*
         * See: http://answers.unity.com/answers/1653313/view.html
         */
        public static List<RaycastResult> GetRaycastEventSystemMouse()
        {
            PointerEventData ed = new PointerEventData(EventSystem.current);
            ed.position = Input.mousePosition;
            List<RaycastResult> res = new List<RaycastResult>();
            EventSystem.current.RaycastAll(ed, res);
            return res;
        }

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
