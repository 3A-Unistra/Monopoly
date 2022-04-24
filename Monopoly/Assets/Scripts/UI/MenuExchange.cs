/*
 * MenuExchange.cs
 * 
 * Date created : 19/04/2022
 * Author       : Maxime MAIRE <maxime.maire2@etu.unistra.fr>
 *                Finn Rayment <rayment@etu.unistra.fr>
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Monopoly.Classes;
using Monopoly.Runtime;

namespace Monopoly.UI
{

    public class MenuExchange : MonoBehaviour
    {

        public Button ReturnButton;
        public Button RefuseButton;
        public Button CounterButton;
        public Button AcceptButton;
        public Button LeaveJailChanceLeftButton;
        public Button LeaveJailCommunityLeftButton;
        public Button LeaveJailChanceRightButton;
        public Button LeaveJailCommunityRightButton;
        public TMP_Dropdown PlayerDropdown;
        public TMP_InputField MoneyPlayerLeft;
        public TMP_InputField MoneyPlayerRight;

        public GameObject MiniCardPrefab;
        public CardDisplay CardDisplayLeft;
        public CardDisplay CardDisplayRight;
        public GameObject CardFieldLeft;
        public GameObject CardFieldRight;
        public GameObject ListFieldLeft;
        public GameObject ListFieldRight;

        public GameObject CardViewportLeft;
        public GameObject CardViewportRight;

        private List<MiniCard> CardListLeft;
        private List<MiniCard> CardListRight;

        private Player playerPrimary;
        private List<Player> playerList;
        private int playerPrimaryIndex;

        void Start()
        {
            ReturnButton.onClick.AddListener(ReturnAction);
            RefuseButton.onClick.AddListener(RefuseAction);
            CounterButton.onClick.AddListener(CounterAction);
            AcceptButton.onClick.AddListener(AcceptAction);
            LeaveJailChanceLeftButton.onClick.AddListener(LeaveJailChanceLeftAction);
            LeaveJailCommunityLeftButton.onClick.AddListener(LeaveJailCommunityLeftAction);
            LeaveJailChanceRightButton.onClick.AddListener(LeaveJailChanceRightAction);
            LeaveJailCommunityRightButton.onClick.AddListener(LeaveJailCommunityRightAction);

            PlayerDropdown.onValueChanged.AddListener(ChangePlayer);

            CardListLeft = new List<MiniCard>();
            CardListRight = new List<MiniCard>();

            UIDirector.IsMenuOpen = true;
        }

        private void ReturnAction()
        {
            UIDirector.IsMenuOpen = false;
            Destroy(this.gameObject);
        }

        private void RefuseAction()
        {
            ClientGameState.current.DoExchangeRefuse();
            Destroy(this.gameObject);
        }

        private void CounterAction()
        {
            ClientGameState.current.DoExchangeCounter();
            Destroy(this.gameObject);
        }

        private void AcceptAction()
        {
            ClientGameState.current.DoExchangeAccept();
            Destroy(this.gameObject);
        }

        private void LeaveJailChanceLeftAction()
        {

        }

        private void LeaveJailCommunityLeftAction()
        {

        }

        private void LeaveJailChanceRightAction()
        {

        }
        private void LeaveJailCommunityRightAction()
        {

        }

        public void PopulateLeft(Player p)
        {
            ClearCardList(CardListLeft);
            foreach (Square s in ClientGameState.current.Board.Elements)
            {
                if (!s.IsOwnable())
                    continue;
                OwnableSquare os = (OwnableSquare) s;
                GameObject cardObject = Instantiate(MiniCardPrefab, CardViewportLeft.transform);
                MiniCard cardScript = cardObject.GetComponent<MiniCard>();
                cardScript.Price.text = os.Price.ToString();
                cardScript.Index = s.Id;
                //TODO fonction r?cupere le nom

                CardListLeft.Add(cardScript);
            }
            playerPrimary = p;
        }

        public void PopulateRight(Player p)
        {
            ClearCardList(CardListRight);
            foreach (Square s in ClientGameState.current.Board.Elements)
            {
                if (!s.IsOwnable())
                    continue;
                OwnableSquare os = (OwnableSquare)s;
                if (os.Owner != p)
                    continue;
                GameObject cardObject = Instantiate(MiniCardPrefab, CardViewportRight.transform);
                MiniCard cardScript = cardObject.GetComponent<MiniCard>();
                cardScript.Price.text = os.Price.ToString();
                cardScript.Index = s.Id;
                //TODO fonction recupere le nom

                CardListRight.Add(cardScript);
            }
        }

        private void ClearCardList(List<MiniCard> cards)
        {
            foreach (MiniCard m in cards)
            {
                Destroy(m.gameObject);
            }
            cards.Clear();
        }

        public List<int> GetPropertySelectionLeft()
        {
            List<int> indices = new List<int>();

            foreach (MiniCard m in CardListLeft)
            {
                if (m.Selected)
                    indices.Add(m.Index);
            }
            return indices;
        }

        public List<int> GetPropertySelectionRight()
        {
            List<int> indices = new List<int>();

            foreach (MiniCard m in CardListRight)
            {
                if (m.Selected)
                    indices.Add(m.Index);
            }
            return indices;
        }

        public int GetMoneyLeft()
        {
            int money;
            if (!int.TryParse(MoneyPlayerLeft.text, out money))
                money = 0;
            return money;
        }

        public int GetMoneyRight()
        {
            int money;
            if (!int.TryParse(MoneyPlayerRight.text, out money))
                money = 0;
            return money;
        }

        public void PopulatePlayers(List<Player> players)
        {
            PlayerDropdown.options.Clear();
            playerList = players;
            int first = -1;
            for (int i = 0; i < players.Count; ++i)
            {
                Player p = players[i];
                if (playerPrimary == p)
                {
                    playerPrimaryIndex = i;
                    continue;
                }
                if (first == -1)
                    first = i;
                PlayerDropdown.options.Add(new TMP_Dropdown.OptionData(p.Name));
            }
            PlayerDropdown.value = 0;
            PopulateRight(players[first]);
        }

        private void ChangePlayer(int index)
        {
            if (index >= playerPrimaryIndex)
                ++index;

            PopulateRight(playerList[index]);
        }

    }
}
