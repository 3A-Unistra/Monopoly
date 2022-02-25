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

/*
  *<summary>
    *Class Board listing the squares of the board
    *depending on their type and their set

  *</summary>
*/
namespace Monopoly.Classes
{
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
        public void FreeParking(Player p)
        {
            p.Money += BoardMoney;
            BoardMoney = 0;
        }
        public void AddMoney(Player p, int i)
        {
            p.Money += i;
        }
    }
}
