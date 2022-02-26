/*
 * PropertySquare.cs
 * File that contains all the methods used for all the field squares on the
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
        /**
         * <summary>
         * The number of houses that a player owns on this property.
         * </summary>
         */
        public int NbHouse { get; set; }
        
        /**
         * <summary>
         * The cost of a house if a player wants to buy one.
         * </summary>
         */
        public int HouseCost { get; set; }
        
        /**
         * <summary>
         * The rent of the property if the player owns 1 house on it.
         * </summary>
         */
        public int House1Rent { get; set; }
        
        /**
         * <summary>
         * The rent of the property if the player owns 2 houses on it.
         * </summary>
         */
        public int House2Rent { get; set; }
        
        /**
         * <summary>
         * The rent of the property if the player owns 3 houses on it.
         * </summary>
         */
        public int House3Rent { get; set; }
        
        /**
         * <summary>
         * The rent of the property if the player owns 4 houses on it.
         * </summary>
         */
        public int House4Rent { get; set; }
        
        /**
         * <summary>
         * The rent of the property if the player owns a hotel on it.
         * </summary>
         */
        public int HotelRent { get; set; }
        
        /**
         * The color of the group of properties that this square belongs to.
         */
        public Color Col { get; set; }
        
        /**
          * <summary>
          * Constructor of the class <c>PropertySquare</c>.
          * </summary>
          * <param name="type">
          * The new type of the property.
          * </param>
          * <param name="id">
          * The new id of the property.
          * </param>
          * <param name="name">
          * The new name of the property.
          * </param>
          * <param name="image">
          * The new image of the property.
          * </param>
          * <param name="price">
          * The new price of the property.
          * </param>
          * <param name="rent">
          * The new rent of the property.
          * </param>
          * <param name="houseCost">
          * The cost of a single house that a player wishes to buy.
          * </param>
          * <param name="house1Rent">
          * The new rent of the property after building one house on this
          * property.
          * </param>
          * <param name="house2Rent">
          * The new rent of the property after building two houses on this
          * property.
          * </param>
          * <param name="house3Rent">
          * The new rent of the property after building three houses on this
          * property.
          * </param>
          * <param name="house4Rent">
          * The new rent of the property after building four houses on this
          * property.
          * </param>
          * <param name="hotelRent">
          * The new rent of the property after building five houses on this
          * property.
          * </param>
          * <param name="color">
          * The new color of the property.
          * </param>
          * <returns>
          * An instance of the property square object with the given type, id,
          * name, image, mortgaged status, price, rent, different house rents,
          * house cost and color.
          * </returns>
          * <exception cref="WrongIdException">
          * Throws an exception if the given id does not belong to this list
          * {1,3,6,8,9,11,13,14,16,18,19,21,23,24,26,27,29,31,32,34,37,39}.
          * </exception>
          * <exception cref="WrongTypeException">
          * Throws an exception if the given type is different than a
          * SquareType.Field.
          * </exception>
          */
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
        
        /**
         * <summary>
         * This function is responsible about the house buying action in the
         * game.
         * </summary>
         * <param name="p">
         * The player who wants to buy the house.
         * </param>
         */
        public void BuyHouse(Player p)
        {
            //TO DO
            // CHECKING IF THE PLAYER CAN BUILD A HOUSE
            p.Money -= HouseCost;
            NbHouse++;
        }

        /**
         * <summary>
         * This function is responsible about the house selling action in the
         * game.
         * </summary>
         * <param name="p">
         * The player who wants to buy the house.
         * </param>
         */
        public void SellHouse(Player p)
        {
            // TO DO
            // CHECKING IF THE PLAYER CAN SELL A HOUSE
            p.Money += HouseCost;
            NbHouse--;
        }   
        
        /**
         * <summary>
         * This function is used to deduct the given rent from the player's
         * money. This function overrides <c cref="OwnableSquare">OwnableSquare
         * </c>'s PayRent function because it is relative to the number of
         * houses that a player own on this property.
         * </summary>
         * <param name="tenant">
         * The player that lands on another player's owned property and has
         * to pay the rent for its owner.
         * </param>
         * <exception cref="InvalidPlayer">
         * Throws an exception if the given player is not in the list of the
         * players playing the game.
         * </exception>
         */
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
                //TO DO
                //should throw an exception in here
                //print this message temporarily till we implement the
                //exceptions
                Debug.Log("exception: The nb of houses is invalid");
            }
        }
        
        /**
         * <summary>
         * This function is a getter of the property's rent which is relative
         * to the number of houses built on the property.
         * </summary>
         * <param name="nbHouses">
         * The number of houses owned on this property.
         * </param>
         * <returns>
         * The rent of the property with the given number houses.
         * </returns>
         */
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
                //TO DO
                //should throw an exception in here
                //return -1 until implementing the exceptions
                return -1;
            }
        }
    }
}

