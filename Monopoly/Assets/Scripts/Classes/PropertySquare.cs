/*
 * PropertySquare.cs
 * File that contains all the functions used for all the field squares on the
 * board.
 * 
 * Date created : 22/02/2022
 * Author       : Rayan MARMAR <rayan.marmar@etu.unistra.fr>
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monopoly.Classes
{
    /**
     * <summary>
     * This class extends from the <c cref="OwnableSquare">OwnableSquare</c>
     * class and models a Property square on the board.
     * </summary>
     */
    public class PropertySquare : OwnableSquare
    {
        
        public int NbHouse { get; set; }

        
        public int HouseCost { get; set; }
        
        
        public int House1Rent { get; set; }
        
        
        public int House2Rent { get; set; }
        
        
        public int House3Rent { get; set; }
        
        
        public int House4Rent { get; set; }
        
        
        public int HotelRent { get; set; }
        
        
        public Color Col { get; set; }
        
        public PropertySquare(SquareType type, int id, string name,
            Material image, int price, int rent,int houseCost, int house1Rent,
            int house2Rent, int house3Rent, int house4Rent, int hotelRent,
            Color color)
            : base(type, id, name, image, price, rent)
        {
            NbHouse = 0;
            HouseCost = houseCost;
            House1Rent = house1Rent;
            House2Rent = house2Rent; 
            House3Rent = house3Rent;
            House4Rent = house4Rent;
            HotelRent = hotelRent;
            Col = color;
        }

        public void BuyHouse()
        {
            
        }

        public void SellHouse()
        {
            
        }

        public override void PayRent(Player tenant)
        {
            if (NbHouse == 0)
                tenant.Money -= Rent;
            else if (NbHouse == 1)
                tenant.Money -= House1Rent;
            else if (NbHouse == 2)
                tenant.Money -= House2Rent;
            else if (NbHouse == 3)
                tenant.Money -= House3Rent;
            else if (NbHouse == 4)
                tenant.Money -= House4Rent;
            else if (NbHouse == 5)
                tenant.Money -= HotelRent;
            else
            {
                //should throw an exception in here
                //print this message temporarily till we implement the
                //exceptions
                Debug.Log("exception: The nb of houses is invalid");
            }
        }

        public int GetRent(int nbHouses)
        {
            if (nbHouses == 0)
                return Rent;
            else if (nbHouses == 1)
                return House1Rent;
            else if (nbHouses == 2)
                return House2Rent;
            else if (nbHouses == 3)
                return House3Rent;
            else if (nbHouses == 4)
                return House4Rent;
            else if (nbHouses == 5)
                return HotelRent;
            else
            {
                //should throw an exception in here
                //return -1 until implementing the exceptions
                return -1;
            }
        }
    }
}

