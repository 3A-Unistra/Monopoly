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
 class Card
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
 class Bank 
 {
    private int nbHouse;
    private int nbHotel;
     public Bank()
    {
        this.nb_house = 32;
        this.nb_hotel = 12;

    }
    public int NbHouse
    {
        get => nbHouse;
        set => nbHouse = value;
    }
    public int NbHotel
    {
        get => nbHotel;
        set => nbHotel = value;
    }

    bool buyHouse()
    {
        if( NbHouse > 0)
        {
            NbHouse -= 1;
            return true
        }
        return false;
    }
    bool buyHotel()
    {
        if( NbHotel > 0)
        {
            NbHotel -= 1;
            return true
        }   
        return false;
    }
    void sellHouse()
    {
        NbHouse ++;
    }
    void sellHotel()
    {
        NbHotel ++;
    }
    void buyProperty(Player p, Ownable s)
    {
        if(p.Money > s.price )
        {
            s.setowner(p);
            p.Money -=s.price;
        }
    }
    void sellProperty(Player p, Ownable s)
    {

        s.setowner(NULL);
        p.Money +=s.price/2;

    }
 }
}
 class Board
 {
    private Square board[];
    private List<Card> deckCommunity;
    private List<Card> deckChance;
    private int prisonSquare;
    private int boardMoney;
    public Square getSquare(int pos)
    {
        return board[pos];
    }
    public List<Square> squareOwned(Player p)
    {
        List<square> tempList = new List<square>()
        for( int i=0; i<40; i++)
        {
            if(board[i].owner == p)
                tempList.Add(board[i]);
        }
        return tempList;
    }
    public static List<property> getPropertySet(color c)
    {

    }
 }