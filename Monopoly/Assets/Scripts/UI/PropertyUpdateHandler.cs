/*
 * PropertyUpdateHandler.cs
 * 
 * Date created : 03/05/2022
 * Author       : Finn RAYMENT <rayment@etu.unistra.fr>
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Monopoly.Classes;

namespace Monopoly.UI
{

    public class PropertyUpdateHandler : MonoBehaviour
    {

        public static PropertyUpdateHandler current;

        void Awake()
        {
            if (current != null)
            {
                Debug.LogWarning(
                    "Cannot create multiple property update handlers!");
                Destroy(this.gameObject);
                return;
            }
        }

        public void UpdateProperty(Player player, int index)
        {
            
        }

    }

}
