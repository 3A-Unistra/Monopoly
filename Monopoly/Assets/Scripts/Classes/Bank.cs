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
    /**
    * <summary>
    * Class defining the role of the bank, has limited houses and hotels
    * for the player to buy, maximum 32 houses and 12 hotels
    * </summary>
    */
    public class Bank  
    {

        /**
        * <summary>
        * Nbhouse is the number of houses currently
        * in the bank
        * </summary>
        */    
        public int NbHouse
        {
            get;
            set;
        }

        /**
        * <summary>
        * Nbhotel is the number of hotels currently
        * in the bank
        * </summary>
        */           
        public int NbHotel
        {
            get;
            set;
        }

        /**
        * <summary>
        * Constructor
        * sets the number of houses and hotel at the beginning of the game
        * </summary>
        */        
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

        /**
        * <summary>
        * take a hotel from the bank if there are any
        * </summary>        
        * <return>
        * true if you took a hotel frm the bank, false if there are none
        * </return>
        */
        public bool BuyHotel()
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
        * increment the number of houses by one as the player sells one
        * </summary>
        */
        public void SellHouse()
        {
            NbHouse++;
        }

        /**
        * <summary>
        * increment the number of hotels by one as the player sells one
        * </summary>
        */        
        public void SellHotel()
        {
            NbHotel++;
        }

        /**
        * <summary>
        * player p buys the property on the square s
        * the square s owner is set to p
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
            s.Owner = p;
            p.Money -= s.Price;
        }

        /**
        * <summary>
        * player p loses the property on square s
        * the square s owner is set to null
        * </summary>
        * <param name="p">
        * player p the one who loses the square
        * </param> 
        * <param name="s">        
        * Ownablesquare s the square which is lost 
        * </param>
        */         
        public void RelinquishProperty(Player p, OwnableSquare s)
        {
            if (s.Owner == p)
            {
                s.Owner = null;
                if (s.IsProperty())
                {
                    PropertySquare ps = (PropertySquare) s;
                    ps.NbHouse = 0;
                }
            }
        }

    }
}
