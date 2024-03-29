/*
 * OwnableSquare.cs
 * This file contain all the methods that handles the ownable squares on the
 * game board.
 * 
 * Date created : 21/02/2022
 * Author       : Rayan MARMAR <rayan.marmar@etu.unistra.fr>
 *              : Finn RAYMENT <rayment@etu.unistra.fr>
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Monopoly.Util;

namespace Monopoly.Classes
{
    /**
     * <summary>
     * This class extends from the <c cref="Square">Square</c>
     * class and models an Ownable square on the board.
     * </summary>
     */
    public class OwnableSquare : Square
    {
        /**
         * <summary>
         * The owner of the property.
         * </summary>
         * <exception cref="Monopoly.Exceptions.InvalidPlayer">
         * Throws an exception if the given player is not in the list of the
         * players playing the game.
         * </exception>
         */
        public Player Owner { get; set; }
  
        /**
         * <summary>
         * The status of a property(mortgaged or not).
         * </summary>
         */
        public bool Mortgaged{ get; private set; }
  
        /**
         * <summary>
         * The price of the property that a player needs to pay if he chose to
         * buy.
         * </summary>
         */
        public int Price{ get; set; }
  
        /**
         * <summary>
         * The rent of the property that a player has to pay if he reaches a
         * property owned by another player.
         *  </summary>
         */
        public int Rent{ get; set; }

        /**
         * <summary>
         * Load the name of the property via. localisation and return it.
         * Not settable.
         * </summary>
         */
        public string Name {
            get
            {
                string key;
                if (IsProperty())
                    key = "property";
                else if (IsStation())
                    key = "station";
                else
                    key = "museum";
                return StringLocaliser.GetString(
                    string.Format("{0}{1}", key, Id));
            }
        }

        /**
          * <summary>
          * Constructor of the class <c>OwnableSquare</c>.
          * </summary>
          * <param name="type">
          * The new type of the property.
          * </param>
          * <param name="id">
          * The new id of the property.
          * </param>
          * <param name="price">
          * The new price of the property.
          * </param>
          * <param name="rent">
          * The new rent of the property.
          * </param>
          * <returns>
          * An instance of the ownable square object with the given type, id,
          * name, image, mortgaged status, price and rent.
          * </returns>
          * <exception cref="Monopoly.Exceptions.WrongIdException">
          * Throws an exception if the given id is a negative number, or a
          * number greater than 39 or a number of this list
          * {0,2,4,7,10,17,20,22,30,33,36,38}.
          * </exception>
          * <exception cref="Monopoly.Exceptions.WrongTypeException">
          * Throws an exception if the given type is different than a
          * SquareType.Field, SquareType.Station or SquareType.Company.
          * </exception>
          */
        public OwnableSquare(SquareType type, int id, int price, int rent)
            : base(type, id)
        {
            Owner = null;
            Mortgaged = false;
            Price = price;
            Rent = rent;
            int[] ids = {0,2,4,7,10,17,20,22,30,33,36,38};
            List<int> validIdNumbers = new List<int>(ids);
            if (validIdNumbers.Contains(id) || id>39 || id<0)
                throw new Monopoly.Exceptions.WrongIdException
                    ("The id should be a valid ownable square number.");
            if (type != SquareType.Field && type != SquareType.Station && type != SquareType.Company)
                throw new Monopoly.Exceptions.WrongTypeException
                    ("The type should be SquareType.Field, " +
                     "SquareType.Company or SquareType.Station.");
        }

        /**
          * <summary>
          * This function is used to deduct the given rent from the player's
          * money.
          * </summary>
          * <param name="renter">
          * The player that lands on another player's owned property and has
          * to pay the rent for its owner.
          * </param>
          * <exception cref="Monopoly.Exceptions.InvalidPlayer">
          * Throws an exception if the given player is not in the list of the
          * players playing the game.
          * </exception>
          */
        public virtual void PayRent(Player renter)
        {
            if (Owner != null)
            {
                renter.Money -= Rent;
                Owner.Money += Rent;
            }
        }

        /**
         * <summary>
         * This function verifies if two ownable squares belongs to the same
         * group (ex: two stations, two companies or two fields with the same
         * color).
         * </summary>
         * <param name="a">
         * The first ownable square to be compared.
         * </param>
         * <param name="b">
         * The second ownable square to be compared.
         * </param>
         * <returns>
         * true if the two squares belongs to the same group and false if not
         * </returns>
         */
        public static bool IsSameGroup(OwnableSquare a, OwnableSquare b)
        {
            if (a.Type == SquareType.Field && b.Type == SquareType.Field)
            {
                PropertySquare pa = (PropertySquare) a;
                PropertySquare pb = (PropertySquare) b;
                return PropertySquare.GetColorIndex(pa.Id).Equals(
                       PropertySquare.GetColorIndex(pb.Id));
                
            }
            else
                return a.Type == b.Type;
        }
        
        /**
          * <summary>
          * This function is used to verify if a given index is
          * an ownable square index.
          * </summary>
          * <param name="idx">
          * The index of the given square.
          * </param>
          * <returns>
          * true if the given square is ownable and false if not.
          * </returns>
          */
        public static bool IsOwnableIndex(int idx)
        {
            int[] ids = {1,3,5,6,8,9,11,12,13,14,15,16,18,19,21,23,
                24,25,26,27,28,29,31,32,34,35,37,39};
            List<int> validIdx = new List<int>(ids);
            return validIdx.Contains(idx);
        }
        
        
        /**
        * <summary>
        * Mortgage a property for half purchase price.
        * </summary>
        */         
        public void MortgageProperty()
        {
            if (!Mortgaged)
            {
                Mortgaged = true;
                Owner.Money += Price / 2;
            }
        }
        
        /**
        * <summary>
        * Unmortgage a property by paying half purchase price + 10% interest.
        * </summary>
        */         
        public void UnmortgageProperty()
        {
            if (Mortgaged)
            {
                Mortgaged = false;
                Owner.Money -= (Price / 2) + (Price/10);
            }
        }
    }
}

