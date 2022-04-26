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
        public GameObject HostObject;

        public string uuid;
        public new string name;

        private static readonly Color playerColor =
            new Color(1.0f, 0.86f, 0.32f);
        private static readonly Color otherColor = Color.white;

        public void SetHost(bool host)
        {
            HostObject.SetActive(host);
        }

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
            this.name = name;
            Name.color = me ? playerColor : otherColor;
        }

        public bool HandlesPlayer(string uuid)
        {
            return uuid.Equals(this.uuid);
        }

    }

}
