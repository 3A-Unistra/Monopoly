using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyJoin : MonoBehaviour
{
    public Button JoinButton;
    void Start()
    {
        JoinButton.onClick.AddListener(JoinGame);
    }

    public void JoinGame()
    {
        SceneManager.LoadScene("Scenes/BoardScene");
    }
}
