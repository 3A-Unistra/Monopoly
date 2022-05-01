/*
 * MenuExchange.cs
 * 
 * Date created : 19/04/2022
 * Author       : Maxime MAIRE <maxime.maire2@etu.unistra.fr>
 *              : Finn RAYMENT <rayment@etu.unistra.fr>
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Monopoly.Classes;
using Monopoly.Runtime;
using Monopoly.Util;

namespace Monopoly.UI
{

    public class MenuExchange : MonoBehaviour
    {

        // left right left right left right what a mess...

        public Button SendButton;
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
        public TMP_Text ReturnText;
        public TMP_Text SendText;
        public TMP_Text RefuseText;
        public TMP_Text AcceptText;
        public TMP_Text CounterText;
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
        private Player playerSecondary;
        public List<Player> playerList;
        private int playerPrimaryIndex;

        private Coroutine leftMoneyDispatch;
        private Coroutine rightMoneyDispatch;

        private bool firstRound;
        private bool decisionToMake;

        void Awake()
        {
            SendButton.onClick.AddListener(SendAction);
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

            CounterText.text = StringLocaliser.GetString("counter");
            AcceptText.text = StringLocaliser.GetString("accept");
            SendText.text = StringLocaliser.GetString("send");
            ExchangeText.text = StringLocaliser.GetString("exchange");
            RefuseText.text = StringLocaliser.GetString("refuse");
            ReturnText.text = StringLocaliser.GetString("return");
            MoneyPlayerLeft.placeholder.GetComponent<TextMeshProUGUI>().text =
                StringLocaliser.GetString("input_send_money");
            MoneyPlayerRight.placeholder.GetComponent<TextMeshProUGUI>().text =
                StringLocaliser.GetString("input_receive_money");

            CardListLeft = new List<MiniCard>();
            CardListRight = new List<MiniCard>();

            firstRound = true;
            decisionToMake = false;
        }
        
        void Start()
        {
            PopulateLeft(playerPrimary);
            PopulatePlayers(playerList);
            UpdateEditRights();

            UIDirector.IsGameMenuOpen = true;
        }

        void OnDestroy()
        {
            UIDirector.IsGameMenuOpen = false;
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
            // start an enumerator before sending to avoid spam
            if (right)
            {
                if (rightMoneyDispatch != null)
                    StopCoroutine(rightMoneyDispatch);
                if (!ValidateInput(val, right, out int intval))
                    return; // nothing I can do if its a bad value
                rightMoneyDispatch =
                    StartCoroutine(DispatchMoneyEnumerator(intval, right));
            }
            else
            {
                if (leftMoneyDispatch != null)
                    StopCoroutine(leftMoneyDispatch);
                if (!ValidateInput(val, right, out int intval))
                    return; // nothing I can do if its a bad value
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
            bool secondActive = !active &&
                playerSecondary == ClientGameState.current.myPlayer;

            SendButton.gameObject.SetActive(!decisionToMake && active);
            ReturnButton.gameObject.SetActive(firstRound && active);
            MoneyPlayerLeft.enabled = !decisionToMake && active;
            MoneyPlayerRight.enabled = !decisionToMake && active;
            RefuseButton.gameObject.SetActive(decisionToMake && active);
            CounterButton.gameObject.SetActive(decisionToMake && active);
            AcceptButton.gameObject.SetActive(decisionToMake && active);
            LeaveJailChanceLeftButton.enabled = !decisionToMake && active;
            LeaveJailChanceRightButton.enabled = !decisionToMake && active;
            LeaveJailCommunityLeftButton.enabled = !decisionToMake && active;
            LeaveJailCommunityRightButton.enabled = !decisionToMake && active;

            foreach (MiniCard m in CardListLeft)
                m.SelectButton.enabled = !decisionToMake && active;
            foreach (MiniCard m in CardListRight)
                m.SelectButton.enabled = !decisionToMake && active;
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

        private void SendAction()
        {
            SendButton.enabled = false;
            MoneyPlayerLeft.enabled = false;
            MoneyPlayerRight.enabled = false;
            StartCoroutine(SendActionEnumerator());
        }

        private IEnumerator SendActionEnumerator()
        {
            yield return new WaitWhile(() => leftMoneyDispatch != null);
            yield return new WaitWhile(() => rightMoneyDispatch != null);
            ClientGameState.current.DoExchangeSend();
        }

        private void ReturnAction()
        {
            UIDirector.IsGameMenuOpen = false;
            ClientGameState.current.DoExchangeCancel();
            Destroy(this.gameObject);
        }

        private void RefuseAction()
        {
            ClientGameState.current.DoExchangeRefuse();
        }

        private void CounterAction()
        {
            ClientGameState.current.DoExchangeCounter();
        }

        private void AcceptAction()
        {
            ClientGameState.current.DoExchangeAccept();
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

        public void Swap()
        {
            firstRound = false;
            decisionToMake = true;

            Player left = playerPrimary;
            Player right = playerSecondary;
            List<int> leftPropertyChoice = GetPropertySelectionLeft();
            List<int> rightPropertyChoice = GetPropertySelectionRight();
            int leftMoney = GetMoneyLeft();
            int rightMoney = GetMoneyRight();
            // FIXME: add community/chance swap
            //bool leftCommunity = ;

            PopulateLeft(right);
            PopulateRight(left);

            foreach (int i in leftPropertyChoice)
                ToggleSelectProperty(i, true);
            foreach (int i in rightPropertyChoice)
                ToggleSelectProperty(i, false);

            SetMoneyLeft(rightMoney);
            SetMoneyRight(leftMoney);

            HideCardDisplayLeft();
            HideCardDisplayRight();
            UpdateEditRights();
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
            playerSecondary = p;
            PlayerRightName.text = p.Name;
            HideCardDisplayRight();
            UpdateEditRights();
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
                Destroy(m.gameObject);
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
            if (!int.TryParse(MoneyPlayerLeft.text, out int money))
                money = 0;
            return money;
        }

        public int GetMoneyRight()
        {
            if (!int.TryParse(MoneyPlayerRight.text, out int money))
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
            if (playerPrimary == ClientGameState.current.myPlayer)
                ClientGameState.current.DoExchangeSelectPlayer(playerList[first].Id);
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

        private bool ValidateInput(string strval, bool right, out int val)
        {
            strval = strval.Trim();
            int playerMoney = right ? playerSecondary.Money : playerPrimary.Money;
            if (int.TryParse(strval, out val) &&
                val <= playerMoney && val > 0)
            {
                // valid bid amount
                // TODO: add check for current high price
                if (right)
                    MoneyPlayerRight.textComponent.color = Color.black;
                else
                    MoneyPlayerLeft.textComponent.color = Color.black;
                SendButton.enabled = true;
                return true;
            }
            else
            {
                // invalid bid amount
                if (right)
                    MoneyPlayerRight.textComponent.color = Color.red;
                else
                    MoneyPlayerLeft.textComponent.color = Color.red;
                SendButton.enabled = false;
                return false;
            }
        }

    }
}
