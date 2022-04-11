using System.Collections;
using System.Collections.Generic;
using Monopoly.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using  Monopoly.Util;

using UnityEngine.SceneManagement;
public class MenuCreate : MonoBehaviour
{
    public GameObject InviteField;
    public GameObject HostInputObject;
    public TMP_InputField LobbyName;
    public TMP_InputField LobbyPassword;
    public TMP_InputField TurnDuration;
    public TMP_InputField TurnNumbers;
    public OnOff PrivateSwitch;
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
        
    public GameObject LobbyMenuPrefab;
    
    [HideInInspector]
    public bool IsHost = false;
    void Start()
    {
        InitFields();
        InviteButton.onClick.AddListener(InvitePlayer);
        StartButton.onClick.AddListener(LaunchLobby);
        ReturnButton.onClick.AddListener(ReturnLobby);
        PlayersDropdown.onValueChanged.AddListener(delegate{PlayerNumberChange();});
        BotsDropdown.onValueChanged.AddListener(delegate{BotsNumberChange();});
        BuildBotsDropdown();
        
        LobbyName.placeholder.GetComponent<TextMeshProUGUI>().text = StringLocaliser.GetString("enter lobby name");
        LobbyPassword.placeholder.GetComponent<TextMeshProUGUI>().text = StringLocaliser.GetString("enter password");
        CopyText.text = StringLocaliser.GetString("copy token");
        InviteText.text = StringLocaliser.GetString("invite");
        StartText.text = StringLocaliser.GetString("start");
        ReturnText.text = StringLocaliser.GetString("return");
        PrivateText.text = StringLocaliser.GetString("private");
        DurationText.text = StringLocaliser.GetString("duration of a turn");
        NbPlayersText.text = StringLocaliser.GetString("number of players");
        NbBotsText.text = StringLocaliser.GetString("number of bots");
        NbTurnText.text = StringLocaliser.GetString("number of turns");
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
    }
    private void BuildBotsDropdown()
    {
        int nbPlayers = PlayersDropdown.value + 2;
        BotsDropdown.options.Clear();
        for (int i = 0 ; i <= 8 - nbPlayers; i++)
        {
            BotsDropdown.options.Add(new TMP_Dropdown.OptionData
                (string.Format("{0}",i)));
        }
        BotsDropdown.value = 0;
    }
    
    public void InvitePlayer()
    {
        
    }

    public void LaunchLobby()
    {
        SceneManager.LoadScene("Scenes/BoardScene");
    }

    public void ReturnLobby()
    {
        GameObject lobbyMenu = Instantiate(LobbyMenuPrefab, transform.parent);
        Destroy((this.gameObject));
    }

    public void PlayerNumberChange()
    {
        BuildBotsDropdown();
    }
    
    public void BotsNumberChange()
    {
    }
}
