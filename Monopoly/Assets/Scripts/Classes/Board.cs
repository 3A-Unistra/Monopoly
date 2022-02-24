/*
 * Fichier.cs
 * Fichier définissant la classe plateau et ses 
 * interaction avec les différentes cases
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
    * Class Board listing the squares of the board
    * depending on their type and their set
    * Also store money for the player to get at free parking
    * </summary>
    */
    public class Board
    {
        public static List<Square> Elements
        {
            get;
            set;
        }
        public int PrisonSquare
        {
            get;
            set;
        }
        public int BoardMoney
        {
            get;
            set;
        }
        /**
        * <summary>
        * get the square at position pos from the list of elements of the board
        * </summary>
        * <parameter>
        * int pos the position of the square we want to get
        * </parameter>         
        * <return>
        * the square at position pos
        * </return>               
        */
        public Square GetSquare(int pos)
        {
            return Elements[pos];
        }
        public List<OwnableSquare> SquareOwned(Player p)
        {
            List<OwnableSquare> tempList = new List<OwnableSquare>();
            foreach (Square s in Elements)
            {
                if (s.GetType() == typeof(OwnableSquare))
                {
                    OwnableSquare sos = (OwnableSquare) s; 
                    if(sos.Owner == p)
                        tempList.Add(sos); 
                }
            }
            return tempList;
        }
        /**
        * <summary>
        * returns the set of properties of color c>
        * </summary>
        * <parameter>
        * Color c the color of the set 
        * </parameter        
        * <return>
        * propertySet the set of properties of color c
        * </return>        
        */

        public static List<PropertySquare> GetPropertySet(Color c)
        {
            List<PropertySquare> propertySet = new List<PropertySquare>();
            foreach (Square s in Elements)
            {
                if (s.GetType() == typeof(PropertySquare))
                {
                    PropertySquare sps = (PropertySquare) s; 
                    if(sps.Col.Equals(c))
                        propertySet.Add(sps); 
                }
            }
            return propertySet;
        }
        /**
        * <summary>
        * player p gets the money from free parking
        * resets free parking money
        * </summary>
        * <parameter>
        * player p the player who landed on free parking
        * </parameter>        
        */
        public void FreeParking(Player p)
        {
            p.Money += BoardMoney;
            BoardMoney = 0;
        }
        /**
        * <summary>
        * method to add i amount of money to player p money
        * </summary>
        * <parameter>
        * player p the player who recieves money,
        * int i the amount of money
        * </parameter>        
        */        
        public void AddMoney(Player p, int i)
        {
            p.Money += i;
        }
    }
}
