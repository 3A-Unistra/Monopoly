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
          * <returns>
          * An instance of the property square object with the given type, id,
          * name, image, mortgaged status, price, rent, different house rents,
          * house and cost.
          * </returns>
          * <exception cref="Monopoly.Exceptions.WrongIdException">
          * Throws an exception if the given id does not belong to this list
          * {1,3,6,8,9,11,13,14,16,18,19,21,23,24,26,27,29,31,32,34,37,39}.
          * </exception>
          * <exception cref="Monopoly.Exceptions.WrongTypeException">
          * Throws an exception if the given type is different than a
          * SquareType.Field.
          * </exception>
          */
        public PropertySquare(SquareType type, int id, string name,
            Material image, int price, int rent,int houseCost, int house1Rent,
            int house2Rent, int house3Rent, int house4Rent, int hotelRent)
            : base(type, id, name, image, price, rent)
        {
            NbHouse = 0;
            HouseCost = houseCost;
            House1Rent = house1Rent;
            House2Rent = house2Rent; 
            House3Rent = house3Rent;
            House4Rent = house4Rent;
            HotelRent = hotelRent;
            int[] ids = {1,3,6,8,9,11,13,14,16,18,19,21,23,24,26,27,29,
                31,32,34,37,39};
            List<int> validIdNumbers = new List<int>(ids);
            if (!validIdNumbers.Contains(id))
                throw new Monopoly.Exceptions.WrongIdException
                    ("The id should be a valid property number.");
            if (type != SquareType.Field)
                throw new Monopoly.Exceptions.WrongTypeException
                    ("The type should be SquareType.Field.");
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
         * <exception cref="Monopoly.Exceptions.InvalidPlayer">
         * Throws an exception if the given player is not in the list of the
         * players playing the game.
         * </exception>
         * <exception cref="Monopoly.Exceptions.InvalidHouseNumberException">
         * Throws an exception if the owned houses number is negative or
         * greater than 5.
         * </exception>
         */
        public override void PayRent(Player tenant)
        {
            // TO DO THROW AN EXCEPTION IF THE PLAYER IS NOT IN THE GAME
            // WAITING FOR THE GAME STATE CLASS
            if (NbHouse == 0)
            {
                tenant.Money -= Rent;
                Owner.Money += Rent;
            }
                
            else if (NbHouse == 1)
            {
                tenant.Money -= House1Rent;
                Owner.Money += Rent;
            }
            else if (NbHouse == 2)
            {
                tenant.Money -= House2Rent;
                Owner.Money += Rent;
            }
                
            else if (NbHouse == 3)
            {
                tenant.Money -= House3Rent;
                Owner.Money += Rent;
            }
            else if (NbHouse == 4)
            {
                tenant.Money -= House4Rent;
                Owner.Money += Rent;
            }
            else if (NbHouse == 5)
            {
                tenant.Money -= HotelRent;
                Owner.Money += Rent;
            }
            else
            {
                throw new Monopoly.Exceptions.InvalidHouseNumberException
                ("The number of houses should be between 0 and 5.");
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
         * <exception cref="Monopoly.Exceptions.InvalidHouseNumberException">
         * Throws an exception if the owned houses number is negative or
         * greater than 5.
         * </exception>
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
                throw new Monopoly.Exceptions.InvalidHouseNumberException
                ("The number of houses should be between 0 and 5.");
            }
        }
        
        /**
          * <summary>
          * This function is used to verify if a given index is
          * an property square index.
          * </summary>
          * <param name="idx">
          * The index of the given square.
          * </param>
          * <returns>
          * true if the given square is property and false if not.
          * </returns>
          */
        public static bool IsPropertyIndex(int idx)
        {
            int[] ids = {1,3,6,8,9,11,13,14,16,18,19,21,23,
                24,26,27,29,31,32,34,37,39};
            List<int> validIdx = new List<int>(ids);
            return validIdx.Contains(idx);
        }

        public static Color GetColorIndex(int idx)
        {
            switch (idx)
            {
            case 1:
            case 3:
                return new Color(88 / 255f, 12 / 255f, 57 / 255f, 1f);
            case 6:
            case 8:
            case 9:
                return new Color(135 / 255f, 165 / 255f, 215 / 255f, 1f);
            case 11:
            case 13:
            case 14:
                return new Color(239 / 255f, 56 / 255f, 120 / 255f, 1f);
            case 16:
            case 18:
            case 19:
                return new Color(245 / 255f, 128 / 255f, 35 / 255f, 1f);
            case 21:
            case 23:
            case 24:
                return new Color(212 / 255f, 0 / 255f, 0 / 255f, 1f);
            case 26:
            case 27:
            case 29:
                return new Color(255 / 255f, 204 / 255f, 0 / 255f, 1f);
            case 31:
            case 32:
            case 34:
                return new Color(9 / 255f, 135 / 255f, 51 / 255f, 1f);
            case 37:
            case 39:
                return new Color(40 / 255f, 78 / 255f, 161 / 255f, 1f);
            default:
                return Color.white;
            }
        }

    }
}

