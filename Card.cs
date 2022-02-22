using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monopoly.Classes
{
    public class Card
    {
        public string type;
        public int id;
        public string desc;
        public Card(string type, int id, string desc)
        {
            this.id = id;
            this.type = type;
            this.desc = desc;
        }
    }
}
