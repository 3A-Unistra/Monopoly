using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monopoly.Bank
{
    public class Bank 
    {
        public int NbHouse
        {
            get;
            set;
        }
        public int NbHotel
        {
            get;
            set;
        }        
        public Bank()
        {
            this.Nb_house = 32;
            this.Nb_hotel = 12;
        }
        bool BuyHouse()
        {
            if( NbHouse > 0)
            {
                NbHouse -= 1;
                return true
            }
            return false;
        }
        bool BuyHotel()
        {
            if(NbHotel > 0)
            {
                NbHotel -= 1;
                return true
            }   
            return false;
        }
        void SellHouse()
        {
            NbHouse ++;
        }
        void SellHotel()
        {
            NbHotel ++;
        }
        void BuyProperty(Player p, Ownable s)
        {
            if(p.Money > s.price)
            {
                s.setowner(p);
                p.Money -= s.price;
            }
        }
        void SellProperty(Player p, Ownable s)
        {
            s.Setowner(NULL);
            p.Money += s.price/2;
        }
    }
}
