using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monopoly.Classes
{
    public class Player
    {
        public int Money { get; set; }

        public int GetMoney()
        {
            return Money;
        }
        
        public void SetMoney(int a)
        {
            Money = a;
        }
    }
}

