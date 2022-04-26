/*
 * RuntimeData.cs
 * Global data that is loaded at runtime.
 * 
 * Date created : 26/04/2022
 * Author       : Finn RAYMENT <rayment@etu.unistra.fr>
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monopoly.Runtime
{

    public class RuntimeData : MonoBehaviour
    {

        public Sprite[] pieceImages;

        public GameObject MainMenuPrefab;
        public GameObject CreateMenuPrefab;
        public GameObject LobbyMenuPrefab;
        public GameObject LobbyPlayerFieldPrefab;

        [HideInInspector]
        public static RuntimeData current;

        void Start()
        {
            if (current != null)
            {
                Debug.LogWarning("Cannot instantiate multiple runtime datas!");
                Destroy(this.gameObject);
                return;
            }
            current = this;
        }

    }

}
