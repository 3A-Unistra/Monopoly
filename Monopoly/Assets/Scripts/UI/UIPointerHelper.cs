/*
 * UIPointerHelper.cs
 * UI pointer action store.
 * 
 * Date created : 03/05/2022
 * Author       : Finn Rayment <rayment@etu.unistra.fr>
 */

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

using Monopoly.Runtime;

namespace Monopoly.UI
{

    public class UIPointerHelper : MonoBehaviour, IPointerClickHandler
    {

        public delegate void UIPointerDelegate();

        public event UIPointerDelegate onClick;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (onClick != null)
                onClick.Invoke();
        }

    }

}
