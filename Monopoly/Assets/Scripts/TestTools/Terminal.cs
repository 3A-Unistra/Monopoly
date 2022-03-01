using System;
using System.Collections.Generic;
using System.Linq;
using Monopoly.Classes;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Random = System.Random;

namespace Monopoly.TestTools
{
    public class Terminal : MonoBehaviour
    {

        public int i;
        public TMP_InputField input;
        public Button sendButton;
        private List<Player> sortedPlayersList = new List<Player>(6);
        private bool endGame = false;
        void Start()
        {
            Debug.Log("The game started. Enter your next move");
            while (!endGame)
            {
                for ( i = 0; i < sortedPlayersList.Count() ; i++)
                {
                    Player p = sortedPlayersList[i];
                        Debug.Log("It\'s " + p.Name + "\'s turn.");
                    sendButton.onClick.AddListener(SendInput);
                }
            }
        }

        private void SendInput()
        {
            string txt = input.text;
            input.text = "";
            Debug.Log(txt);
            
            void RollDice(Player p)
            {
                Random rnd = new Random();
                int dice1 = rnd.Next(1, 7);
                int dice2 = rnd.Next(1, 7);
                int dices = dice1 + dice2;

                if (dice1 != dice2)
                {
                    p.Position = (p.Position + 1) % 40;
                    p.Doubles = 0;
                }
                else if (dice1 == dice2 && ++p.Doubles < 3)
                {
                    p.Position = (p.Position + 1) % 40;
                    Debug.Log("Your double counter is at " + p.Doubles);
                    i--;
                }
                else if (dice1 == dice2 && p.Doubles == 3)
                {
                    p.EnterPrison();
                    if(p.InJail)
                        Debug.Log("You went to jail in position " + p.Position);
                    else
                        Debug.LogError("AN ERROR OCCURED THE PLAYER SHOULD " +
                                  "BE IN PRISON");
                }
            }
            switch (txt)
            {
                case "RollDice" : 
                    RollDice(sortedPlayersList[i]);
                    break;
            }
            
        }
        
    }
}