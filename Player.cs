using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monopoly.Classes
{

    public class Character
    {
        public int id
        {
            get;
            set;
        }    
    }
    public class Player
    {
        private int id;
        {
            get;
            set;
        }
        private string name;
        {
            get;
            set;
        }
        private Character character;
        {
            get;
            set;
        }
        private int position;
        {
            get;
            set;
        }
        private int score;
        {
            get;
            set;
        }
        private int money;
        {
            get;
            set;
        }
        private int jailTurns;
        {
            get;
            set;
        }
        private bool inJail;
        {
            get;
            set;
        }
        private bool bankrupt;
        {
            get;
            set;
        }
        private bool chanceJailCard;
        {
            get;
            set;
        }
        private bool communityJailCard;
        {
            get;
            set;
        }
        private bool bot;
        {
            get;
            set;
        } 
    public Player (int id,string name,Character character)
    {

        this.id = id;
        this.character = character;
        this.name = name;
        this.money = 1500;
        this.score = 0;
        this.position = 0;
        this.jailTurns = 0;
        this.inJail = false;
        this.bankrupt = false;
        this.chanceJailCard = false;
        this.communityJailCard = false;
        this.bot = false;
    }
    void enterPrison()
    {
        Position = 9;
        InJail = true;
    }
    void exitPrison()
    {
        InJail = false;
    }

    void transferMoney(Player to, int amount)
    {
        if( Money > amount)
        {
            Money -= amount;
            to.Money += amount;
        }
        else
        {
            to.Money += Money;
            Money = 0;
            Bankrupt = true;
        }
    }
    void transferProperty(Player to, Ownable p)
    {
        p.setOwner(to);
    }

    void transferCard(Player to, Card card)
    {
        if ((card.type == "CHANCE") && (ChanceJailCard == true))
        {
            ChanceJailCard = false;
            to.ChanceJailCard = true;
        }
        else if((card.type == "COMMUNITY")&& (CommunityJailCard == true))
        {
            CommunityJailCard = false;
            to.CommunityJailCard = true;
        }
        else
        {
            throw new InvalidOperationException("Erreur vous ne pouvez pas échanger une carte que vous ne possèdez pas.");
        }
    }
 }
 public class Card
 {
     public string type;
     public int id;
     public string desc;
     public Card(string type,int id,string desc)
     {
        this.id = id;
        this.type = type;
        this.desc = desc;

     }
 }
}
