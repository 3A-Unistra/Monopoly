/*
 * OwnableSquare.cs
 * This file contain all the methods that handles the ownable squares on the
 * game board.
 * 
 * Date created : 21/02/2022
 * Author       : Rayan MARMAR <rayan.marmar@etu.unistra.fr>
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
         * <exception cref="InvalidPlayer">
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
        public bool Mortgaged{ get; set; }
  
        /**
         * <summary>
         * The price of the property that a player needs to pay if he chose to
         * buy.
         * </summary>
         */
        public int Price{ get; set; }
  
        /** <summary>
         * The rent of the property that a player has to pay if he reaches a
         * property owned by another player.
         *  </summary>
         */
        public int Rent{ get; set; }

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
          * <returns>
          * An instance of the ownable square object with the given type, id,
          * name, image, mortgaged status, price and rent.
          * </returns>
          * <exception cref="WrongIdException">
          * Throws an exception if the given id is a negative number, or a
          * number greater than 39 or a number of this list
          * {0,2,4,7,10,17,20,22,30,33,36,38}.
          * </exception>
          * <exception cref="WrongTypeException">
          * Throws an exception if the given type is different than a
          * SquareType.Field, SquareType.Station or SquareType.Company.
          * </exception>
          */
        public OwnableSquare(SquareType type, int id, string name, 
            Material image, int price, int rent)
            : base(type, id, name, image)
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
          * <exception cref="InvalidPlayer">
          * Throws an exception if the given player is not in the list of the
          * players playing the game.
          * </exception>
          */
        public virtual void PayRent(Player renter)
        {
            renter.Money -= Rent;
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
                return pa.Col == pb.Col;
                
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
        public bool IsOwnableIndex(int idx)
        {
            SquareType type = Board.Elements[idx].Type;
            return type == SquareType.Field || 
                   type == SquareType.Station || 
                   type == SquareType.Company;
        }
        
        
        /**
        * <summary>
        * player p mortgages the property on the square s
        * the square s owner is set to null
        * the player p's money is increased by half the value of square s 
        * </summary>
        * <param name="p">
        * player p the one who wants to mortgage the square
        * </param> 
        */         
        public void MortgageProperty(Player p)
        {
            if(Owner == p)
            {
                Mortgaged = true;
                Owner = null;
                p.Money += Price/2;
            }
        }
        
        
        /**
        * <summary>
        * player p unmortgages the property on the square s
        * the square s owner is set to null
        * the player p's money is increased by half the value of square s 
        * </summary>
        * <param name="p">
        * player p the one who wants to unmortgage the square
        * </param> 
        */         
        public void UnmortgageProperty(Player p)
        {
            if(Owner == p)
            {
                Mortgaged = false;
                Owner = p;
                p.Money -= Price/2;
            }
        }
    }
}

