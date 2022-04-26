/*
 * LobbyPlayerField.cs
 * Player info for the lobby list.
 * 
 * Date created : 16/04/2022
 * Author       : Finn RAYMENT <rayment@etu.unistra.fr>
 */

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using Monopoly.Classes;
using Monopoly.Runtime;

namespace Monopoly.UI
{

    public class LobbyPlayerField : MonoBehaviour
    {

        public Image Avatar;
        public TMP_Text Name;

        private string uuid;

        private static readonly Color playerColor =
            new Color(1.0f, 0.86f, 0.32f);
        private static readonly Color otherColor = Color.white;

        public void SetUser(string uuid, string name, int charIdx, bool me)
        {
            Name.text = name;
            int avatar = charIdx;
            if (avatar >= 0 &&
                avatar < RuntimeData.current.pieceImages.Length)
            {
                Avatar.sprite = RuntimeData.current.pieceImages[avatar];
            }
            this.uuid = uuid;
            Name.color = me ? playerColor : otherColor;
        }

        public bool HandlesPlayer(string uuid)
        {
            return uuid.Equals(this.uuid);
        }

    }

}
