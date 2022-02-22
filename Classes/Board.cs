using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monopoly.Classes
{
    public class Board
    {
        public Square Board[]
        {
            get;
            set;
        }
        public int PrisonSquare
        {
            get;
            set;
        }
        public int BoardMoney
        {
            get;
            set;
        }
        public Square GetSquare(int pos)
        {
            return Board[pos];
        }
        public List<Square> SquareOwned(Player p)
        {  
            List<square> tempList = new List<square>()
            for(int i = 0;i < 40;i++)
            {
                if(Board[i].Owner == p)
                    tempList.Add(Board[i]);
            }
            return tempList;
        }
        public static List<property> GetPropertySet(color c)
        {
            static List<property> propertySet = new List<property>();
            for(int i = 0; i < 40; i++)
            {
                if(Board[i].color == c)
                    propertySet.add(Board[i]);
            }
            return propertySet;
        }
        public void FreeParking(Player p)
        {
            p.Money += BoardMoney;
            BoardMoney = 0;
        }
        public void AddMoney(Player p; int i)
        {
            p.Money += i;
        }
    }
}
