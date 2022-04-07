/*
 * ChatHandler.cs
 * Chat window handler.
 * 
 * Date created : 07/04/2022
 * Author       : Finn Rayment <rayment@etu.unistra.fr>
 */

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Monopoly.Runtime
{

    public class ChatHandler : MonoBehaviour
    {

        public TMP_Text chatHistory;

        void Start()
        {
            chatHistory.text = "";
        }

    }

}
