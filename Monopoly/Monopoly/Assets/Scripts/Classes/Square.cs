/*
 * Square.cs
 * File that models a square on the board.
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
     *<summary>
     * Abstract class <c>Square</c> models a square on the board
     * </summary>
     */
    public class Square
    {
        /**
         * <summary>
         * The type of the square
         * </summary>
         */
        public SquareType Type { get; set; }
 
        /**
         * <summary>
         * The name of the square that will be written on the board.
         * </summary>
         */
        public string Name { get; set; }
 
        /**
         * <summary>
         * The id of the square is the unique number that represents its placement
         * on the board from 0 being the first square to 39 being the last.
         * </summary>
         */
        public int Id { get; set; }
 
        /**
         * <summary>
         * The image that represents the square that will be shown on the board.
         * </summary>
         */
        public Material Image { get; set; }
 
        /**
         * <summary>
         * Constructor of the class <c>Square</c>.
         * </summary>
         * <param name="type">
         * The new type of the square.
         * </param>
         * <param name="id">
         * The new id of the square.
         * </param>
         * <param name="name">
         * The new name of the square on the board.
         * </param>
         * <param name="image">
         * The new image of the square on the board.
         * </param>
         * <returns>
         * Returns an instance of the Square object with the given type, id, name and
         * image.
         * </returns>
         * * <exception cref="Monopoly.Exceptions.WrongIdException">
         * Throws an exception if the given id is a negative number, or a number
         * greater than 39.
         * </exception>
         */
        protected Square(SquareType type, int id, string name, Material image)
        {
            Type = type;
            Name = name;
            Image = image;
            if (id < 0 || id > 39)
                throw new Monopoly.Exceptions.WrongIdException
                ("The id should be a number between 0 and 39.");
        }

        /**
          * <summary>
          * This function is used to verify if a given square is a property.
          * </summary>
          * <returns>
          * true if the given square is a property and false if not.
          * </returns>
          */
        public bool IsProperty()
        {
            return Type == SquareType.Field || 
                   Type == SquareType.Station || 
                   Type == SquareType.Company;
        }

        /**
          * <summary>
          * This function is used to verify if a given square is a tax square.
          * </summary>
          * <returns>
          * true if the given square is a tax square and false if not.
          * </returns>
          */
        public bool IsTax()
        {
            return Type == SquareType.Tax;
        }

        /**
          * <summary>
          * This function is used to verify if a given square is the Go square.
          * </summary>
          * <returns>
          * true if the given square is is the Go square and false if not.
          * </returns>
          */
        public bool IsGo()
        {
            return Type == SquareType.Go;
        }
        
        /**
          * <summary>
          * This function is used to verify if a given square is the free parking
          * square.
          * </summary>
          * <returns>
          * true if the given square is is the free parking square and false if not.
          * </returns>
          */
        public bool IsFreeParking()
        {
            return Type == SquareType.Parking;
        }
        /**
          * <summary>
          * This function is used to verify if a given square is the go to jail
          * square.
          * </summary>
          * <returns>
          * true if the given square is is the go to jail square and false if
          * not.
          * </returns>
          */
        bool IsGoToJail()
        {
            return Type == SquareType.GoToJail;
        }
        
        /**
          * <summary>
          * This function is used to verify if a given square is a community
          * square.
          * </summary>
          * <returns>
          * true if the given square is a community square and false if not.
          * </returns>
          */
        public bool IsCommunityChest()
        {
            return Type == SquareType.Community;
        }

        /**
          * <summary>
          * This function is used to verify if a given square is a chance square.
          * </summary>
          * <returns>
          * true if the given square is a chance square and false if not.
          * </returns>
          */
        public bool IsChance()
        {
            return Type == SquareType.Chance;
        }
    }
}

