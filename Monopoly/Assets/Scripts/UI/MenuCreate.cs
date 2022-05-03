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
    
        public GameObject PlayerFieldViewport;
        
        [HideInInspector]
        public bool IsHost { get; private set; }

        [HideInInspector]
        public string GameToken;

        private List<LobbyPlayerField> playerFields;

        [HideInInspector]
        public static MenuCreate current;

        private bool canEdit;

        private Coroutine updateRoutine;
        [HideInInspector]
        public string CurrentHost;

        void Awake()
        {
            if (current != null)
            {
                Debug.LogWarning("Cannot create multiple lobby menus!");
                Destroy(this.gameObject);
                return;
            }

            current = this;
            updateRoutine = null;
            LobbyName.onValueChanged.AddListener(delegate { NameChange(); });
            InviteButton.onClick.AddListener(InvitePlayer);
            StartButton.onClick.AddListener(LaunchLobby);
            ReturnButton.onClick.AddListener(ReturnLobby);
            PlayersDropdown.onValueChanged.AddListener(
                delegate { PlayerNumberChange(); });
            BotsDropdown.onValueChanged.AddListener(
                delegate { BotsNumberChange(); });
            PrivateSwitch.onClick.AddListener(PrivacyChange);
            PrivateSwitch.GetComponent<OnOff>().
                Front.onClick.AddListener(PrivacyChange);
            AuctionsSwitch.onClick.AddListener(AuctionsChange);
            AuctionsSwitch.GetComponent<OnOff>().
                Front.onClick.AddListener(AuctionsChange);
            DoubleOnGoSwitch.onClick.AddListener(DoubleOnGoChange);
            DoubleOnGoSwitch.GetComponent<OnOff>().
                Front.onClick.AddListener(DoubleOnGoChange);
            BuyFirstTurnSwitch.onClick.AddListener(BuyingChange);
            BuyFirstTurnSwitch.GetComponent<OnOff>().
                Front.onClick.AddListener(BuyingChange);
            StartingBalance.onValueChanged.AddListener(
                delegate { BalanceChange(); });
            TurnNumbers.onValueChanged.AddListener(
                delegate{ TurnNumbersChange(); });
            TurnDuration.onValueChanged.AddListener(
                delegate{ TurnDurationChange(); });
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
            StartingBalanceText.text =
                StringLocaliser.GetString("starting_balance");

            playerFields = new List<LobbyPlayerField>();

            canEdit = false;
        }

        void Start()
        {
            /*ManagePlayerList(PacketBroadcastUpdateRoom.UpdateReason.NEW_PLAYER,
                             ClientLobbyState.clientUUID,
                             ClientLobbyState.clientUsername,
                             null,
                             ClientLobbyState.clientPiece);*/
            UIDirector.IsMenuOpen = true;
            UIDirector.IsUIBlockingNet = false;
        }

        public void EnableEdits(bool edit)
        {
            canEdit = IsHost && edit; // can't edit for others
            InviteButton.enabled = IsHost;
            StartButton.enabled = IsHost;
            HostInputObject.SetActive(IsHost);
            LobbyName.enabled = canEdit;
            PlayersDropdown.enabled = canEdit;
            BotsDropdown.enabled = canEdit;
            PrivateSwitch.enabled = canEdit;
            TurnDuration.enabled = canEdit;
            TurnNumbers.enabled = canEdit;
            AuctionsSwitch.enabled = canEdit;
            BuyFirstTurnSwitch.enabled = canEdit;
            DoubleOnGoSwitch.enabled = canEdit;
            StartingBalance.enabled = canEdit;
            PrivateSwitch.enabled = canEdit;
        }

        public void UpdateFields(string hostUUID)
        {
            UpdateFields(IsHost, hostUUID);
        }

        public void UpdateFields(bool isHost, string hostUUID)
        {
            this.IsHost = isHost;
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
            PrivateSwitch.enabled = IsHost;
            if (hostUUID != null)
                CurrentHost = hostUUID;
            if (CurrentHost != null)
            {
                foreach (LobbyPlayerField field in playerFields)
                    field.SetHost(field.HandlesPlayer(CurrentHost));
            }
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
            ManagePlayerList(packet.Reason,
                packet.PlayerId, packet.Username,
                null, packet.Piece);
            UpdateFields(CurrentHost);
        }

        private bool IsPlayerListed(string uuid)
        {
            foreach (LobbyPlayerField field in playerFields)
            {
                if (field.HandlesPlayer(uuid))
                    return true;
            }
            return false;
        }

        private LobbyPlayerField GetPlayerField(string uuid)
        {
            foreach (LobbyPlayerField field in playerFields)
            {
                if (field.HandlesPlayer(uuid))
                    return field;
            }
            return null;
        }

        public void ManagePlayerList(
            PacketBroadcastUpdateRoom.UpdateReason reason,
            string uuid, string username, string avatar, int piece)
        {
            // FIXME: implement and avatar if not null!!!
            switch (reason)
            {
            case PacketBroadcastUpdateRoom.UpdateReason.NEW_PLAYER:
                if (IsPlayerListed(uuid))
                    return; // no need to duplicate the player id
                GameObject playerField =
                    Instantiate(RuntimeData.current.LobbyPlayerFieldPrefab,
                                PlayerFieldViewport.transform);
                LobbyPlayerField fieldScript =
                    playerField.GetComponent<LobbyPlayerField>();
                fieldScript.SetUser(uuid, username, piece,
                    uuid.Equals(ClientLobbyState.clientUUID));
                playerFields.Add(fieldScript);
                break;
            case PacketBroadcastUpdateRoom.UpdateReason.PLAYER_LEFT:
                LobbyPlayerField field = GetPlayerField(uuid);
                if (field != null)
                {
                    playerFields.Remove(field);
                    Destroy(field.gameObject);
                }
                break;
            }
        }

        public void UpdateParticipants(List<PacketStatusInternal> players)
        {
            if (players.Count < playerFields.Count)
            {
                Debug.LogWarning(
                    "Server sent back a StatusRoom packet with less players" +
                    "than expected. Will balance...");
                for (int i = 0; i < playerFields.Count; ++i)
                {
                    LobbyPlayerField field = playerFields[i];
                    if (!players.Exists((x) => x.PlayerId.Equals(field.uuid)))
                    {
                        playerFields.Remove(field);
                        Destroy(field.gameObject);
                        --i; // go again
                    }
                }
            }
            foreach (PacketStatusInternal playerData in players)
            {
                ManagePlayerList(
                    PacketBroadcastUpdateRoom.UpdateReason.NEW_PLAYER,
                    playerData.PlayerId, playerData.Username,
                    playerData.AvatarURL, playerData.Piece);
            }
        }

        public void SetName(string lobbyName)
        {
            if (IsHost)
                return;
            LobbyName.text = lobbyName;
            LobbyName.enabled = IsHost;
        }

        public void SetPlayerNumber(int n)
        {
            if (IsHost)
                return;
            PlayersDropdown.value = n - 2;
            PlayersDropdown.enabled = IsHost;
        }

        public void SetBotsNumber(int n)
        {
            if (IsHost)
                return;
            BotsDropdown.value = n;
            BotsDropdown.enabled = IsHost;
        }

        public void SetAuctionSwitch(bool auction)
        {
            if (IsHost)
                return;
            if(AuctionsSwitch.GetComponent<OnOff>().switchOn != auction)
                AuctionsSwitch.onClick.Invoke();
            AuctionsSwitch.enabled = IsHost;
            AuctionsSwitch.GetComponent<OnOff>().Front.enabled = IsHost;

        }

        public void SetDoubleOnStartSwitch(bool doubleOnStart)
        {
            if (IsHost)
                return;
            if(DoubleOnGoSwitch.GetComponent<OnOff>().switchOn != doubleOnStart)
                DoubleOnGoSwitch.onClick.Invoke();
            DoubleOnGoSwitch.enabled = IsHost; 
            DoubleOnGoSwitch.GetComponent<OnOff>().Front.enabled = IsHost;
        }

        public void SetBuyingSwitch(bool canBuy)
        {
            if (IsHost)
                return;
            if(BuyFirstTurnSwitch.GetComponent<OnOff>().switchOn != canBuy)
                BuyFirstTurnSwitch.onClick.Invoke();
            BuyFirstTurnSwitch.enabled = IsHost;
            BuyFirstTurnSwitch.GetComponent<OnOff>().Front.enabled = IsHost;
        }

        public void SetPrivacy(bool isPrivate)
        {
            if (IsHost)
                return;
            if(PrivateSwitch.GetComponent<OnOff>().switchOn != isPrivate)
                PrivateSwitch.onClick.Invoke();
            PrivateSwitch.enabled = IsHost;
            PrivateSwitch.GetComponent<OnOff>().Front.enabled = IsHost;
        }

        public void SetStartingBalance(int balance)
        {
            if (IsHost)
                return;
            StartingBalance.text = balance.ToString();
            StartingBalance.enabled = IsHost;
        }

        public void SetTurnTime(int time)
        {
            if (IsHost)
                return;
            TurnDuration.text = time.ToString();
            TurnDuration.enabled = IsHost;
        }

        public void SetNbTurns(int nb)
        {
            if (IsHost)
                return;
            TurnNumbers.text = nb.ToString();
            TurnNumbers.enabled = IsHost;
        }
        
        private void SetPacketStatusRoom()
        {
            if (updateRoutine != null)
                StopCoroutine(updateRoutine);
            updateRoutine = StartCoroutine(SetPacketStatusRoomEnumerator());
        }

        private IEnumerator SetPacketStatusRoomEnumerator()
        {
            yield return new WaitForSeconds(1); // wait a second before send to avoid spam
            // TODO: store defaults in data
            if (!int.TryParse(TurnNumbers.text, out int turnNumber))
                turnNumber = 200;
            if (!int.TryParse(TurnDuration.text, out int turnDuration))
                turnDuration = 60;
            if (!int.TryParse(StartingBalance.text, out int startingBalance))
                startingBalance = 1500;
            ClientLobbyState.current.DoRoomModify(LobbyName.text.Trim(),
                playerFields.Count, PlayersDropdown.value + 2,
                null,
                AuctionsSwitch.GetComponent<OnOff>().switchOn,
                DoubleOnGoSwitch.GetComponent<OnOff>().switchOn,
                BuyFirstTurnSwitch.GetComponent<OnOff>().switchOn,
                turnNumber, turnDuration, startingBalance);
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

        public void NameChange()
        {
            if (canEdit)
                SetPacketStatusRoom();
        }

        public void PlayerNumberChange()
        {
            if (canEdit)
            {
                BuildBotsDropdown();
                SetPacketStatusRoom();
            }
        }

        public void BotsNumberChange()
        {
            if (canEdit)
                SetPacketStatusRoom();
        }

        public void PrivacyChange()
        {
            if (canEdit)
                SetPacketStatusRoom();
        }

        public void AuctionsChange()
        {
            if (canEdit)
                SetPacketStatusRoom();
        }
        
        public void DoubleOnGoChange()
        {
            if (canEdit)
                SetPacketStatusRoom();
        }
        
        public void BuyingChange()
        {
            if (canEdit)
                SetPacketStatusRoom();
        }
        
        public void TurnNumbersChange()
        {
            if (canEdit)
                SetPacketStatusRoom();
        }
        
        public void TurnDurationChange()
        {
            if (canEdit)
                SetPacketStatusRoom();
        }
        
        public void BalanceChange()
        {
            if (canEdit)
                SetPacketStatusRoom();
        }

        public void CopyToken()
        {
            GUIUtility.systemCopyBuffer = ClientLobbyState.currentLobby;
        }

    }

}
