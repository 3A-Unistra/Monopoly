using System.Collections;
using System.Collections.Generic;
using Monopoly.Util;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyJoin : MonoBehaviour
{

    public TMP_Text LobbyName;
    public Button JoinButton;
    public TMP_Text JoinText;
    public GameObject CreateMenuPrefab;

    [HideInInspector]
    public GameObject ParentMenu;

    [HideInInspector]
    public string Token;
    
    void Start()
    {
        JoinButton.onClick.AddListener(JoinGame);
        JoinText.text = StringLocaliser.GetString("join");
    }

    public void JoinGame()
    {
        GameObject CreateMenu = Instantiate(CreateMenuPrefab, ParentMenu.transform.parent);
        CreateMenu.GetComponent<MenuCreate>().IsHost = false;
        Destroy(ParentMenu);
    }

}
