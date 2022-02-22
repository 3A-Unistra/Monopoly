/*
 * Fichier.cs
 * Fichier définissant la classe Banque et ses interaction
 * 
 * Date created : 22/02/2022
 * Author       : Christophe Pierson <christophe.pierson@etu.unistra.fr>
 */


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monopoly.Classes
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
            this.NbHouse = 32;
            this.NbHotel = 12;
        }
        bool BuyHouse()
        {
            if( NbHouse > 0)
            {
                NbHouse -= 1;
                return true;
            }
            return false;
        }
        bool BuyHotel()
        {
            if(NbHotel > 0)
            {
                NbHotel -= 1;
                return true;
            }   
            return false;
        }
        void SellHouse()
        {
            NbHouse++;
        }
        void SellHotel()
        {
            NbHotel++;
        }
        void BuyProperty(Player p, OwnableSquare s)
        {
            if(p.Money > s.Price)
            {
                s.Owner = p;
                p.Money -= s.Price;
            }
        }
        void SellProperty(Player p, OwnableSquare s)
        {
            s.Owner = null;
            p.Money += s.Price/2;
        }
    }
}
