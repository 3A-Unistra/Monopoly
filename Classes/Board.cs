using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monopoly.Classes
{
    public class Board
    {
        private Square board[];
        private List<Card> deckCommunity;
        private List<Card> deckChance;
        private int prisonSquare;
        public int BoardMoney
        {
            get;
            set;
        }
        public Square GetSquare(int pos)
        {
            return board[pos];
        }
        public List<Square> SquareOwned(Player p)
        {  
            List<square> tempList = new List<square>()
            for(int i = 0;i < 40;i++)
            {
                if(board[i].owner == p)
                    tempList.Add(board[i]);
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
        Card GetRandomChanceCard()
        {
            return deckChance[rnd.Next(1,deckChance.Count)]
        }
        Card GetRandomCommunityCard()
        {
            return deckCommunity[rnd.Next(1,deckCommunity.Count)]
        }
    }
}
