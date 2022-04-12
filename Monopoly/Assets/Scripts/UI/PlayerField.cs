/*
 * PlayerField.cs
 * Individual UI piece for player information.
 * 
 * Date created : 12/04/2022
 * Author       : Finn RAYMENT <rayment@etu.unistra.fr>
 */

using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using Monopoly.Classes;
using Monopoly.Runtime;
using Monopoly.Util;

namespace Monopoly.UI
{

    public class PlayerField : MonoBehaviour
    {

        public Image Avatar;
        public TMP_Text Username;
        public TMP_Text Money;

        private string uuid;

        public void SetUser(Player player)
        {
            Username.text = player.Name;
            int avatar = player.CharacterIdx;
            if (avatar > 0 &&
                avatar < ClientGameState.current.pieceImages.Length)
            {
                Avatar.sprite = ClientGameState.current.pieceImages[avatar];
            }
            uuid = player.Id;
            SetMoney(player.Money);
        }

        public void SetMoney(int amount)
        {
            StringBuilder sb = new StringBuilder();
            bool red = amount < 0;
            if (red)
                sb.Append("<color=#ff0000>-");
            sb.Append(string.Format(
                StringLocaliser.GetString("money_format"), amount));
            if (red)
                sb.Append("</color>");
            Money.text = sb.ToString();
        }

        public bool HandlesPlayer(Player player)
        {
            return player.Id.Equals(uuid);
        }

    }

}
