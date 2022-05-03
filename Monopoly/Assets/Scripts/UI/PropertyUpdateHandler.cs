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
using UnityEngine.UI;

using Monopoly.Classes;
using Monopoly.Runtime;

using static UnityEngine.KeyCode;

namespace Monopoly.UI
{

    public class PropertyUpdateHandler : MonoBehaviour
    {

        public static PropertyUpdateHandler current = null;

        public GameObject propertySet1;

        private Coroutine coroutine;
        private bool set1Activated = false;

        void Awake()
        {
            if (current != null)
            {
                Debug.LogWarning(
                    "Cannot create multiple property update handlers!");
                Destroy(this.gameObject);
                return;
            }
            current = this;
            set1Activated = false;
        }

        public void UpdateProperty(Player player, int index)
        {
            if (coroutine != null)
                StopCoroutine(coroutine);
            coroutine = null;
            // kinda bad but whatever
            switch (index)
            {
            case 0:
                propertySet1.SetActive(true);
                coroutine = StartCoroutine(UpdatePropertyEnumerator());
                set1Activated = true;
                break;
            default:
                propertySet1.SetActive(false);
                // just ignore it because it's out of bounds
                // (or we set it in the enumerator)
                // if it happens from outside this clas, it is probably a
                // bad server request...
                break;
            }
        }

        private IEnumerator UpdatePropertyEnumerator()
        {
            yield return new WaitForSeconds(5.0f);
            UpdateProperty(ClientGameState.current.myPlayer, -1);
        }

        void Update()
        {
            if (!set1Activated &&
                Input.GetKeyDown(P) &&
                (Input.GetKey(LeftControl) ||
                 Input.GetKey(RightControl)))
            {
                ClientGameState.current.DoUpdateProperty(0);
            }
        }

    }

}
