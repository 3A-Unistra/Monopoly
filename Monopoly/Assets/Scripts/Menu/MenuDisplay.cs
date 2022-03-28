using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuDisplay : MonoBehaviour
{
    public GameObject Menu;
    public GameObject Boutton;
    bool afficher = false;

    void Start()
    {
    }

    void Update()
    {
    }

    public void MenuActive()
    {
        afficher = !afficher;
        Menu.SetActive(afficher);
        Boutton.SetActive(!afficher);
    }
}
