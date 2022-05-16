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
        public Image ActiveKnob;
        public Image ChanceKnob;
        public Image CommunityKnob;
        public TMP_Text Username;
        public TMP_Text Money;
        public TMP_Text MoneyModifier;

        public DiceAnimation Dice;

        private string uuid;
        private int lastAmount;

        private Coroutine coroutine;

        private Color color;

        void Start()
        {
            ActiveKnob.gameObject.SetActive(false);
            ChanceKnob.gameObject.SetActive(false);
            CommunityKnob.gameObject.SetActive(false);
            MoneyModifier.gameObject.SetActive(false);
            coroutine = null;
        }

        public void SetUser(Player player, Color color, bool me)
        {
            Username.text = player.Name;
            int avatar = player.CharacterIdx;
            if (avatar >= 0 &&
                avatar < RuntimeData.current.pieceImages.Length)
            {
                Avatar.sprite = RuntimeData.current.pieceImages[avatar];
            }
            if (me)
                Username.fontStyle = FontStyles.Italic;
            Username.color = color; //me ? playerColor : otherColor;
            this.color = color;
            uuid = player.Id;
            lastAmount = player.Money;
            SetMoney(player.Money, false);
        }

        public void SetActive(bool active)
        {
            ActiveKnob.gameObject.SetActive(active);
        }

        private void SetMoneyDirect(int amount)
        {
            StringBuilder sb = new StringBuilder();
            bool red = amount < 0;
            if (red)
                sb.Append("<color=#ff5555>");
            sb.Append(string.Format(
                StringLocaliser.GetString("money_format"), amount));
            if (red)
                sb.Append("</color>");
            Money.text = sb.ToString();
        }

        public void SetMoney(int amount, bool animate)
        {
            if (coroutine != null)
                StopCoroutine(coroutine);
            coroutine = StartCoroutine(AnimateMoney(amount, animate));
        }

        private IEnumerator AnimateMoney(int amount, bool animate)
        {
            // show the actual money
            SetMoneyDirect(amount);
            if (animate && lastAmount != amount)
            {
                // show the money modifier
                StringBuilder modsb = new StringBuilder();
                int modamount = Mathf.Abs(lastAmount - amount);
                bool modred = amount < lastAmount;
                if (modred)
                    modsb.Append("<color=#ff5555>-");
                else
                    modsb.Append("<color=#55ff55>+");
                modsb.Append(string.Format(
                    StringLocaliser.GetString("money_format"), modamount));
                modsb.Append("</color>");
                MoneyModifier.text = modsb.ToString();
                MoneyModifier.gameObject.SetActive(true);
                // wait a while
                yield return new WaitForSeconds(1.5f);
                MoneyModifier.gameObject.SetActive(false);
            }
            else
            {
                MoneyModifier.gameObject.SetActive(false);
            }
            lastAmount = amount;
            coroutine = null;
        }

        public void SetChance(bool on)
        {
            ChanceKnob.gameObject.SetActive(on);
        }

        public void SetCommunity(bool on)
        {
            CommunityKnob.gameObject.SetActive(on);
        }

        public bool HandlesPlayer(Player player)
        {
            return player.Id.Equals(uuid);
        }

        public bool HandlesPlayer(string uuid)
        {
            return uuid.Equals(this.uuid);
        }

        public Color getColor()
        {
            return color;
        }

    }

}
