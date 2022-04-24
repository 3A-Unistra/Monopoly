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

        // left right left right left right what a mess...

        public Button ReturnButton;
        public Button RefuseButton;
        public Button CounterButton;
        public Button AcceptButton;
        public Button LeaveJailChanceLeftButton;
        public Button LeaveJailCommunityLeftButton;
        public Button LeaveJailChanceRightButton;
        public Button LeaveJailCommunityRightButton;
        public TMP_InputField MoneyPlayerLeft;
        public TMP_InputField MoneyPlayerRight;

        public TMP_Text ExchangeText;
        public TMP_Text PlayerLeftName;
        public TMP_Text PlayerRightName;
        public TMP_Dropdown PlayerRightDropdown;

        public GameObject MiniCardPrefab;
        public CardDisplay CardDisplayLeft;
        public CardDisplay CardDisplayRight;
        public GameObject CardFieldLeft;
        public GameObject CardFieldRight;
        public GameObject ListFieldLeft;
        public GameObject ListFieldRight;

        public GameObject CardViewportLeft;
        public GameObject CardViewportRight;
        public Button CardHideButtonLeft;
        public Button CardHideButtonRight;

        private List<MiniCard> CardListLeft;
        private List<MiniCard> CardListRight;

        public Player playerPrimary;
        public List<Player> playerList;
        private int playerPrimaryIndex;

        private Coroutine leftMoneyDispatch;
        private Coroutine rightMoneyDispatch;

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
            CardHideButtonLeft.onClick.AddListener(HideCardDisplayLeft);
            CardHideButtonRight.onClick.AddListener(HideCardDisplayRight);

            PlayerRightDropdown.onValueChanged.AddListener(ChangePlayer);

            MoneyPlayerLeft.onValueChanged.AddListener(DispatchLeftMoney);
            MoneyPlayerRight.onValueChanged.AddListener(DispatchRightMoney);

            CardListLeft = new List<MiniCard>();
            CardListRight = new List<MiniCard>();

            PopulateLeft(playerPrimary);
            PopulatePlayers(playerList);

            UIDirector.IsMenuOpen = true;
        }

        private void DispatchLeftMoney(string val)
        {
            DispatchMoney(val, false);
        }

        private void DispatchRightMoney(string val)
        {
            DispatchMoney(val, true);
        }

        private void DispatchMoney(string val, bool right)
        {
            val = val.Trim();
            int intval;
            if (!int.TryParse(val, out intval))
                return; // nothing I can do if its a bad value
            // start an enumerator before sending to avoid spam
            if (right)
            {
                if (rightMoneyDispatch != null)
                    StopCoroutine(rightMoneyDispatch);
                rightMoneyDispatch =
                    StartCoroutine(DispatchMoneyEnumerator(intval, right));
            }
            else
            {
                if (leftMoneyDispatch != null)
                    StopCoroutine(leftMoneyDispatch);
                leftMoneyDispatch =
                    StartCoroutine(DispatchMoneyEnumerator(intval, right));
            }
        }

        private IEnumerator DispatchMoneyEnumerator(int val, bool right)
        {
            // wait exactly one second. if a value was edited, this routine
            // will be cancelled and restarted again. thus, this yield passes
            // only if no further input was detected for at least a second
            yield return new WaitForSeconds(1);
            ClientGameState.current.DoExchangeSelectTrade(right, val,
                Net.Packets.PacketActionExchangeTradeSelect.SelectType.MONEY);
            // now remove the reference
            if (right)
                rightMoneyDispatch = null;
            else
                leftMoneyDispatch = null;
        }

        private void DispatchCard(int idx, bool selected, bool right)
        {
            ClientGameState.current.DoExchangeSelectTrade(right, idx,
                Net.Packets.PacketActionExchangeTradeSelect.SelectType.PROPERTY);
        }

        private void UpdateEditRights()
        {
            bool active = playerPrimary == ClientGameState.current.myPlayer;
            // TODO: activate/disactivate buttons for users
        }

        private void HideCardDisplayLeft()
        {
            CardFieldLeft.SetActive(false);
            ListFieldLeft.SetActive(true);
        }

        private void HideCardDisplayRight()
        {
            CardFieldRight.SetActive(false);
            ListFieldRight.SetActive(true);
        }

        private void DisplayCardLeft(int idx)
        {
            CardDisplayLeft.Render(idx);
            CardFieldLeft.SetActive(true);
            ListFieldLeft.SetActive(false);
        }

        private void DisplayCardRight(int idx)
        {
            CardDisplayRight.Render(idx);
            CardFieldRight.SetActive(true);
            ListFieldRight.SetActive(false);
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
                OwnableSquare os = (OwnableSquare)s;
                if (os.Owner != p)
                    continue;
                GameObject cardObject = Instantiate(MiniCardPrefab, CardViewportLeft.transform);
                MiniCard cardScript = cardObject.GetComponent<MiniCard>();
                cardScript.Price.text = os.Price.ToString();
                cardScript.Index = s.Id;
                cardScript.Name.text = os.Name;
                cardScript.editable = p == ClientGameState.current.myPlayer;
                cardScript.selectCallback = (x, y) => DispatchCard(x, y, false);
                cardScript.previewCallback =
                    delegate { DisplayCardLeft(s.Id); };

                CardListLeft.Add(cardScript);
            }
            playerPrimary = p;
            PlayerLeftName.text = p.Name;
            // activate right side based on who we are
            PlayerRightName.gameObject.SetActive(
                playerPrimary != ClientGameState.current.myPlayer);
            PlayerRightDropdown.gameObject.SetActive(
                playerPrimary == ClientGameState.current.myPlayer);
            HideCardDisplayLeft();
            UpdateEditRights();
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
                cardScript.Name.text = os.Name;
                cardScript.editable =
                    playerPrimary == ClientGameState.current.myPlayer;
                cardScript.selectCallback = (x, y) => DispatchCard(x, y, true);
                cardScript.previewCallback =
                    delegate { DisplayCardRight(s.Id); };

                CardListRight.Add(cardScript);
            }
            PlayerRightName.text = p.Name;
            HideCardDisplayRight();
        }

        public void ToggleSelectProperty(int index, bool right)
        {
            List<MiniCard> list = right ? CardListRight : CardListLeft;
            foreach (MiniCard m in list)
            {
                if (m.Index == index)
                {
                    m.ToggleSelect(false);
                    break;
                }
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

        public void SetMoneyLeft(int val)
        {
            MoneyPlayerLeft.text = val.ToString();
        }

        public void SetMoneyRight(int val)
        {
            MoneyPlayerRight.text = val.ToString();
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
            PlayerRightDropdown.options.Clear();
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
                PlayerRightDropdown.options.Add(new TMP_Dropdown.OptionData(p.Name));
            }
            //PlayerRightDropdown.value = -1;
            PlayerRightDropdown.value = 0;
            PopulateRight(playerList[first]);
        }

        private void ChangePlayer(int index)
        {
            int playerIdx = -1;
            // not sure if the player list is guaranteed to be in the same order on
            // all instances so its definitely safer to check for the correct person
            // opposed to just assume
            for (int i = 0; i < playerList.Count; ++i)
            {
                if (playerList[i].Name.Equals(PlayerRightDropdown.options[index].text))
                    playerIdx = i;
            }
            if (playerIdx == -1)
            {
                // but if all else fails, go back to the original method I came up with
                playerIdx = index >= playerPrimaryIndex ? index + 1 : index;
            }
            //PopulateRight(playerList[index]);
            ClientGameState.current.DoExchangeSelectPlayer(playerList[index].Id);
        }

    }
}
