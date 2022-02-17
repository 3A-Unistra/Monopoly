using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonopolyPlayer
{

 class Character
 {
    private int id;
    public int Id
    {
        get => id;
        set => id = value;
    }    
 }
 class Player
 {
    private int id;
    private string name;
    private Character character;
    private int position;
    private int score;
    private int money;
    private int jailTurns;
    private bool inJail;
    private bool bankrupt;
    private bool chanceJailCard;
    private bool communityJailCard;
    private bool bot; 
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
    public int Id
    {
        get => id;
        set => id = value;
    }
    public Character Character
    {
        get => character;
        set => character = value;
    }
    public string Name
    {
        get => name;
        set => name = value;
    }
    public int Money
    {
        get => money;
        set => money = value;
    }

    public int Score
    {
        get => score;
        set => score = value;
    }

    public int Position
    {
        get => position;
        set => position = value;
    }

    public int JailTurns 
    {
        get => jailTurns;
        set => jailTurns = value;
    }

    public bool InJail 
    {
        get => inJail;
        set => inJail = value;
    }

    public bool Bankrupt 
    {
        get => bankrupt;
        set => bankrupt = value;
    }

    public bool ChanceJailCard
    {
        get => chanceJailCard;
        set => chanceJailCard = value;
    }
    public bool CommunityJailCard 
    {
        get => communityJailCard ;
        set => communityJailCard  = value;
    }
    public bool Bot
    {
        get => bot;
        set => bot  = value;
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
 }
 class Card
 {
     public string type;
     public int id;
     public string desc;
     public Card(string type,int id,string desc)
     {
        this.id=id;
        this.type=type;
        this.desc=desc;

     }
 }
 class Bank 
 {
     public int nb_house;
     public int nb_hotel;
     public int stored_money;
     public Bank(int nb_house,int nb_hotel,int stored_money)
     {
         this.nb_house=nb_house;
         this.nb_hotel=nb_hotel;
         this.stored_money=stored_money;

     }
 }
 class Train_Station
 {
    public int owner;
    public int price;
    public int base_rent;     
    public Train_Station()
    {
        this.owner=0;
        this.price=200;
        this.base_rent=50;
    }
 }
}
