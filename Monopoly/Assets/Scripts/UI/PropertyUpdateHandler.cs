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
        private bool set2Activated = false;

        [HideInInspector]
        public bool chatMessageSent = false;

        public Renderer chatRenderer1;
        public Renderer chatRenderer2;
        public Renderer chatRenderer3;
        public Material chatMaterial1;
        public Material chatMaterial2;
        public Material chatMaterial3;

        private Material tmpMaterial1;
        private Material tmpMaterial2;
        private Material tmpMaterial3;

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

        void Start()
        {
            tmpMaterial1 = chatRenderer1.material;
            tmpMaterial2 = chatRenderer2.material;
            tmpMaterial3 = chatRenderer3.material;
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
            case 1:
                tmpMaterial1 = chatRenderer1.material;
                tmpMaterial2 = chatRenderer2.material;
                tmpMaterial3 = chatRenderer3.material;
                chatRenderer1.material = chatMaterial1;
                chatRenderer2.material = chatMaterial2;
                chatRenderer3.material = chatMaterial3;
                coroutine = StartCoroutine(UpdatePropertyEnumerator());
                set2Activated = true;
                break;
            default:
                propertySet1.SetActive(false);
                chatRenderer1.material = tmpMaterial1;
                chatRenderer2.material = tmpMaterial2;
                chatRenderer3.material = tmpMaterial3;
                // just ignore it because it's out of bounds
                // (or we set it in the enumerator)
                // if it happens from outside this clas, it is probably a
                // bad server request...
                break;
            }
        }

        private IEnumerator UpdatePropertyEnumerator()
        {
            yield return new WaitForSeconds(7.5f);
            UpdateProperty(ClientGameState.current.myPlayer, -1);
        }

        void Update()
        {
            if (!set1Activated &&
                Input.GetKeyDown(P) &&
                Input.GetKey(LeftControl) &&
                (Input.GetKey(RightControl) || Input.GetKey(RightAlt)))
            {
                ClientGameState.current.DoUpdateProperty(0);
            }
            if (!set2Activated && chatMessageSent)
            {
                chatMessageSent = false;
                ClientGameState.current.DoUpdateProperty(1);
            }
        }

    }

}
