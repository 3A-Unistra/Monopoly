/*
 * MenuAuction.cs
 * Auction menu UI handler.
 * 
 * Date created : 19/04/2022
 * Author       : Maxime MAIRE <maxime.maire2@etu.unistra.fr
 *              : Finn RAYMENT <rayment@etu.unistra.fr>
 */

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using Monopoly.Classes;
using Monopoly.Runtime;
using Monopoly.Util;

namespace Monopoly.UI
{

    public class MenuAuction : MonoBehaviour
    {

        public GameObject PlayerFieldPrefab;

        public Button BidButton;
        public TMP_InputField BidField;
        public TMP_Text AuctionText;
        public TMP_Text BidText;
        public TMP_Text PriceText;
        public GameObject PlayerFieldViewport;
        public CardDisplay CardDisplayObject;
        public Timeout Timeout;

        public TMP_Text CurrentPrice;
        public TMP_Text OriginalPrice;

        [HideInInspector]
        public int TimeoutDuration;

        [HideInInspector]
        public int Index;

        [HideInInspector]
        public int CurrentBid;

        private int bidWish;

        private List<AuctionPlayerField> fields;

        void Start()
        {
            BidButton.onClick.AddListener(BidAction);

            BidField.onValueChanged.AddListener(ValidateInput);
            BidText.text = StringLocaliser.GetString("bid");
            AuctionText.text = StringLocaliser.GetString("auction");
            PriceText.text = StringLocaliser.GetString("price");
            BidField.placeholder.GetComponent<TextMeshProUGUI>().text = StringLocaliser.GetString("input_bid");
            
            BidButton.enabled = false;

            fields = new List<AuctionPlayerField>();

            BuildUI();
            BuildCard();

            Timeout.SetTime(TimeoutDuration);
            Timeout.Restart();

            UIDirector.IsGameMenuOpen = true;
        }

        void OnDestroy()
        {
            UIDirector.IsGameMenuOpen = false;
        }

        private void UpdatePrice(int amount)
        {
            CurrentBid = amount;
            CurrentPrice.text = string.Format(
                StringLocaliser.GetString("money_format"), amount);
        }

        private void BuildCard()
        {
            CardDisplayObject.Render(Index);
            Square square = ClientGameState.current.Board.GetSquare(Index);
            if (square.IsOwnable())
            {
                OwnableSquare os = (OwnableSquare) square;
                OriginalPrice.text = string.Format(
                    StringLocaliser.GetString("money_format"), os.Price);
            }
        }

        private void BuildUI()
        {
            foreach (Player player in ClientGameState.current.players)
            {
                GameObject fieldObject =
                    Instantiate(PlayerFieldPrefab,
                                PlayerFieldViewport.transform);
                AuctionPlayerField fieldScript =
                    fieldObject.GetComponent<AuctionPlayerField>();
                fieldScript.SetUser(player,
                                    player == ClientGameState.current.myPlayer);
                fieldScript.SetPrice(0, false);
                fields.Add(fieldScript);
            }
        }

        private void ValidateInput(string strval)
        {
            strval = strval.Trim();
            if (int.TryParse(strval, out int val) &&
                val <= ClientGameState.current.myPlayer.Money && val > 0 &&
                val > CurrentBid)
            {
                // valid bid amount
                // TODO: add check for current high price
                BidField.textComponent.color = Color.black;
                BidButton.enabled = true;
                bidWish = val;
            }
            else
            {
                // invalid bid amount
                BidField.textComponent.color = Color.red;
                BidButton.enabled = false;
            }
        }

        private void BidAction()
        {
            ClientGameState.current.DoAuctionBid(bidWish);
            BidField.text = "";
        }

        public void Bid(string playerUUID, int amount)
        {
            foreach (AuctionPlayerField field in fields)
            {
                if (field.HandlesPlayer(playerUUID))
                    field.SetPrice(amount, true);
                else
                    field.SetPrice(field.GetPrice(), false);
            }
            UpdatePrice(amount);
            Timeout.Restart();
        }

    }

}
