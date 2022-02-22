using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monopoly.Bank
{
public class Bank 
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
        if(NbHotel > 0)
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
        if(p.Money > s.price)
        {
            s.setowner(p);
            p.Money -= s.price;
        }
    }
    void sellProperty(Player p, Ownable s)
    {

        s.setowner(NULL);
        p.Money += s.price/2;

    }
 }
}
