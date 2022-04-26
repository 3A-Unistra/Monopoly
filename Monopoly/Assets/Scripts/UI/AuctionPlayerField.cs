/*
 * AuctionPlayerField.cs
 * This file contain the event listeners of the options' Menu buttons, slider and
 * switches.
 * 
 * Date created : 25/04/2022
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

    public class AuctionPlayerField : MonoBehaviour
    {

        public Image avatar;
        public TMP_Text username;
        public TMP_Text price;

        private Player player;
        private int priceVal;

        private static readonly Color playerColor =
            new Color(1.0f, 0.86f, 0.32f);
        private static readonly Color otherColor = Color.white;

        public void SetUser(Player player, bool me)
        {
            username.text = player.Name;
            int avatar = player.CharacterIdx;
            if (avatar >= 0 &&
                avatar < RuntimeData.current.pieceImages.Length)
            {
                this.avatar.sprite =
                    RuntimeData.current.pieceImages[avatar];
            }
            username.color = me ? playerColor : otherColor;
            this.player = player;
        }

        public void SetPrice(int price, bool winning)
        {
            string priceFormat = StringLocaliser.GetString("money_format");
            StringBuilder sb = new StringBuilder();
            if (winning)
                sb.Append("<color=#55ff55>");
            sb.Append(string.Format(priceFormat, price));
            if (winning)
                sb.Append("</color>");
            this.price.text = sb.ToString();
            this.priceVal = price;
        }

        public int GetPrice()
        {
            return priceVal;
        }

        public bool HandlesPlayer(Player player)
        {
            return player == this.player;
        }

        public bool HandlesPlayer(string uuid)
        {
            return uuid.Equals(player.Id);
        }

    }

}
