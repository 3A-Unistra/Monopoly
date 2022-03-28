using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPause : MonoBehaviour
{
    public GameObject Menu;
    public GameObject MenuButton;

    void Start()
    {
    }

    void Update()
    {
    }

    public void MenuDisable()
    {
        Menu.SetActive(false);
        MenuButton.SetActive(true);
        // il faut aussi activer le bouton pour ouvrir le menu
    }

    public void OptionActive()
    {
        ;
    }

    public void DisconnectToTheGame()
    {
        ;
    }

    public void QuitGame()
    {
        ;
    }
}
