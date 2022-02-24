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
/**
* <summary>
*   Class defining the role of the bank, has limited houses and hotels
*   for the player to buy, maximum 32 houses and 12 hotels
*   Stores money for the player to get at the free parking square
* </summary>
*/
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
        /**
        * <summary>
        *   sets the number of houses and hotel at the beginning of the game
        * </summary>
        */        
        public Bank()
        {
            this.NbHouse = 32;
            this.NbHotel = 12;
        }
         /**
        * <summary>
        *   take a house from the bank if there are any
        *  <return>
        *   true if you took a house frm the bank, false if there are none
        *  </return>
        * </summary>
        */ 
        bool BuyHouse()
        {
            if( NbHouse > 0)
            {
                NbHouse -= 1;
                return true;
            }
            return false;
        }
        /**
        * <summary>
        *   take a hotel from the bank if there are any
        *  <return>
        *   true if you took a hotel frm the bank, false if there are none
        *  </return>
        * </summary>
        */
        bool BuyHotel()
        {
            if(NbHotel > 0)
            {
                NbHotel -= 1;
                return true;
            }   
            return false;
        }
        /**
        * <summary>
        *   increment the number of houses by one as the player sells one
        * </summary>
        */
        void SellHouse()
        {
            NbHouse++;
        }
        /**
        * <summary>
        *   increment the number of hotels by one as the player sells one
        * </summary>
        */        
        void SellHotel()
        {
            NbHotel++;
        }
        /**
        * <summary>
        *   player p buys the property on the square s
        *   the square s owner is set to p
        *   the player p's money is decreased by the value of square s
        * <parameters>
        *   player p the one who buys the square and 
        *   Ownablesquare s the square which is bought  
        * </summary>
        */          
        void BuyProperty(Player p, OwnableSquare s)
        {
            if(p.Money > s.Price)
            {
                s.Owner = p;
                p.Money -= s.Price;
            }
        }
        /**
        * <summary>
        *   player p sells the property on the square s
        *   the square s owner is set to null
        *   the player p's money is increased by half the value of square s
        * <parameters>
        *   player p the one who sells the square and 
        *   Ownablesquare s the square which is sold  
        * </summary>
        */         
        void SellProperty(Player p, OwnableSquare s)
        {
            s.Owner = null;
            p.Money += s.Price/2;
        }
    }
}
