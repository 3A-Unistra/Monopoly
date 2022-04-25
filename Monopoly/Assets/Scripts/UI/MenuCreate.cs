using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using Monopoly.Runtime;
using Monopoly.Util;
using Monopoly.Net.Packets;

namespace Monopoly.UI
{

    public class MenuCreate : MonoBehaviour
    {

        public GameObject InviteField;
        public GameObject HostInputObject;
        public TMP_InputField LobbyName;
        public TMP_InputField LobbyPassword;
        public TMP_InputField TurnDuration;
        public TMP_InputField TurnNumbers;
        public TMP_InputField StartingBalance;
        public Button PrivateSwitch;
        public Button AuctionsSwitch;
        public Button DoubleOnGoSwitch;
        public Button BuyFirstTurnSwitch;
        public Button CopyButton;
        public TMP_Text CopyText;
        public Button InviteButton;
        public TMP_Text InviteText;
        public Button StartButton;
        public TMP_Text StartText;
        public Button ReturnButton;
        public TMP_Text ReturnText;
        public TMP_Dropdown PlayersDropdown;
        public TMP_Dropdown BotsDropdown;
        public TMP_Text PrivateText;
        public TMP_Text DurationText;
        public TMP_Text NbTurnText;
        public TMP_Text NbPlayersText;
        public TMP_Text NbBotsText;
        public TMP_Text AuctionsText;
        public TMP_Text DoubleOnGoText;
        public TMP_Text BuyFirstTurnText;
        public TMP_Text StartingBalanceText;
    
        public GameObject PlayerFieldPrefab;
    
        public GameObject PlayerFieldViewport;
        
        [HideInInspector]
        public bool IsHost = false;

        [HideInInspector]
        public string GameToken;

        [HideInInspector]
        private List<LobbyPlayerField> playerFields;

        [HideInInspector]
        public static MenuCreate current;


        void Start()
        {
            if (current != null)
            {
                Debug.LogWarning("Cannot create multiple lobby menus!");
                Destroy(this.gameObject);
                return;
            }
            current = this;
    
            InitFields();
            InviteButton.onClick.AddListener(InvitePlayer);
            StartButton.onClick.AddListener(LaunchLobby);
            ReturnButton.onClick.AddListener(ReturnLobby);
            PlayersDropdown.onValueChanged.AddListener(
                delegate { PlayerNumberChange(); });
            BotsDropdown.onValueChanged.AddListener(
                delegate { BotsNumberChange(); });
            CopyButton.onClick.AddListener(CopyToken);
            BuildBotsDropdown();
            
            LobbyName.placeholder.GetComponent<TextMeshProUGUI>().text =
                StringLocaliser.GetString("enter_lobby_name");
            LobbyPassword.placeholder.GetComponent<TextMeshProUGUI>().text =
                StringLocaliser.GetString("enter_password");
            CopyText.text = StringLocaliser.GetString("copy_token");
            InviteText.text = StringLocaliser.GetString("invite");
            StartText.text = StringLocaliser.GetString("start");
            ReturnText.text = StringLocaliser.GetString("return");
            PrivateText.text = StringLocaliser.GetString("private");
            DurationText.text = StringLocaliser.GetString("turn_duration");
            NbPlayersText.text = StringLocaliser.GetString("number_players");
            NbBotsText.text = StringLocaliser.GetString("number_bots");
            NbTurnText.text = StringLocaliser.GetString("number_turns");
            AuctionsText.text = StringLocaliser.GetString("auctions");
            DoubleOnGoText.text = StringLocaliser.GetString("double_on_go");
            BuyFirstTurnText.text = StringLocaliser.GetString("buy_first_turn");
            StartingBalanceText.text = StringLocaliser.GetString("starting_balance");
        
            playerFields = new List<LobbyPlayerField>();

            ManagePlayerList(PacketBroadcastUpdateRoom.UpdateReason.NEW_PLAYER,
                             ClientLobbyState.clientUsername);
    
            UIDirector.IsMenuOpen = true;
            UIDirector.IsUIBlockingNet = false;
        }

        private void InitFields()
        {
            InviteButton.enabled = IsHost;
            StartButton.gameObject.SetActive(IsHost);
            HostInputObject.SetActive(IsHost);
            InviteField.SetActive(IsHost);
            LobbyName.enabled = IsHost;
            PlayersDropdown.enabled = IsHost;
            BotsDropdown.enabled = IsHost;
            PrivateSwitch.enabled = IsHost;
            TurnDuration.enabled = IsHost;
            TurnNumbers.enabled = IsHost;
            AuctionsSwitch.enabled = IsHost;
            BuyFirstTurnSwitch.enabled = IsHost;
            DoubleOnGoSwitch.enabled = IsHost;
            StartingBalance.enabled = IsHost;
        }

        void OnDestroy()
        {
            if (current == this)
                current = null;
        }

        private void BuildBotsDropdown()
        {
            int nbPlayers = PlayersDropdown.value + 2;
            BotsDropdown.options.Clear();
            for (int i = 0; i <= 8 - nbPlayers; i++)
            {
                BotsDropdown.options.Add(new TMP_Dropdown.OptionData
                    (string.Format("{0}", i)));
            }
            BotsDropdown.value = 0;
        }

        public void UpdateLobby(PacketBroadcastUpdateRoom packet)
        {
            ManagePlayerList(packet.Reason, packet.Player);
        }

        private void ManagePlayerList(
            PacketBroadcastUpdateRoom.UpdateReason reason, string username)
        {
            // FIXME: implement
            switch (reason)
            {
            case PacketBroadcastUpdateRoom.UpdateReason.NEW_PLAYER:
                GameObject playerField =
                    Instantiate(PlayerFieldPrefab,
                                PlayerFieldViewport.transform);
                LobbyPlayerField fieldScript =
                    playerField.GetComponent<LobbyPlayerField>();
                fieldScript.Name.text = username;
                playerFields.Add(fieldScript);
                break;
            case PacketBroadcastUpdateRoom.UpdateReason.PLAYER_LEFT:
                LobbyPlayerField field = null;
                foreach (LobbyPlayerField f in playerFields)
                {
                    if (f.Name.text.Equals(username))
                    {
                        field = f;
                        break;
                    }
                }
                if (field != null)
                {
                    playerFields.Remove(field);
                    Destroy(field.gameObject);
                }
                break;
            }
        }

        public void SetName(string lobbyName)
        {
            LobbyName.text = lobbyName;
        }

        public void SetPlayerNumber(int n)
        {
            PlayersDropdown.value = n - 2;
        }

        public void SetBotsNumber(int n)
        {
            BotsDropdown.value = n;
        }

        public void SetAuctionSwitch(bool auction)
        {
            if(AuctionsSwitch.GetComponent<OnOff>().switchOn != auction)
                AuctionsSwitch.onClick.Invoke();
        }

        public void SetDoubleOnStartSwitch(bool doubleOnStart)
        {
            if(DoubleOnGoSwitch.GetComponent<OnOff>().switchOn != doubleOnStart)
                DoubleOnGoSwitch.onClick.Invoke();
        }

        public void SetBuyingSwitch(bool canBuy)
        {
            if(BuyFirstTurnSwitch.GetComponent<OnOff>().switchOn != canBuy)
                BuyFirstTurnSwitch.onClick.Invoke();
        }

        public void SetPrivacy(bool isPrivate)
        {
            if(PrivateSwitch.GetComponent<OnOff>().switchOn != isPrivate)
                PrivateSwitch.onClick.Invoke();
        }

        public void SetStartingBalance(int balance)
        {
            StartingBalance.text = balance.ToString();
        }
        
        public void SetTurnTime(int time)
        {
            TurnDuration.text = time.ToString();
        }

        public void SetNbTurns(int nb)
        {
            TurnNumbers.text = nb.ToString();
        }

        public PacketStatusRoom SetPacketStatusRoom()
        {
            if (!int.TryParse(TurnNumbers.text, out int turnNumber))
                turnNumber = 200;
            if (!int.TryParse(TurnDuration.text, out int turnDuration))
                turnDuration = 60;
            if (!int.TryParse(StartingBalance.text, out int startingBalance))
                startingBalance = 1500;
            List<string> playerNames = new List<string>();
            foreach (LobbyPlayerField l in playerFields)
                playerNames.Add(l.name);
            PacketStatusRoom p = 
                new PacketStatusRoom(ClientLobbyState.currentLobby,
                    LobbyName.text,
                    playerFields.Count, PlayersDropdown.value + 2,
                    playerNames,AuctionsSwitch.GetComponent<OnOff>().switchOn,
                    DoubleOnGoSwitch.GetComponent<OnOff>().switchOn,
                    BuyFirstTurnSwitch.GetComponent<OnOff>().switchOn,
                    turnNumber, turnDuration, startingBalance);
            return p;
        }
        
        public void InvitePlayer()
        {

        }

        public void LaunchLobby()
        {
            ClientLobbyState.current.DoLaunchGame();
        }

        public void ReturnLobby()
        {
            ClientLobbyState.current.DoLeaveGame(ClientLobbyState.currentLobby);
        }

        public void PlayerNumberChange()
        {
            BuildBotsDropdown();
        }

        public void BotsNumberChange()
        {

        }

        public void CopyToken()
        {
            GUIUtility.systemCopyBuffer = ClientLobbyState.currentLobby;
        }

    }

}
