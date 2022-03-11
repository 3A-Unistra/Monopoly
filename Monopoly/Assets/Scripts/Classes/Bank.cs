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
         /**
        * <summary>
        * takes a house from the bank if there are any
        * </summary>        
        * <return>
        * true if you took a house frm the bank, false if there are none
        * </return>
        */
         public bool BuyHouse()
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
        /**
        * <summary>
        * player p buys the property on the square s
        * the square s owner is set to p
        * the player p's money is decreased by the value of square s
        * </summary>
        * <param name="p">
        * player p the one who buys the square  
        * </param>        
        * <param name="s">
        * Ownablesquare s the square which is bought
        * </param>          
        */
        public void BuyProperty(Player p, OwnableSquare s)
        {
            if(p.Money > s.Price)
            {
                s.Owner = p;
                p.Money -= s.Price;
            }
        }
    }
}
