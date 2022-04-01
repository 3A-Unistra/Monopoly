/*
 * Square.cs
 * File that models a square on the board.
 * 
 * Date created : 21/02/2022
 * Author       : Rayan MARMAR <rayan.marmar@etu.unistra.fr>
 *              : Finn RAYMENT <rayment@etu.unistra.fr>
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

        public int Id { get; set; }
 
        /**
         * <summary>
         * The image that represents the square that will be shown on the board.
         * </summary>
         */

        public Square(SquareType type, int id)
        {
            Type = type;
            Id = id;
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
            return Type == SquareType.Field;
        }

        /**
          * <summary>
          * This function is used to verify if a given square is a station.
          * </summary>
          * <returns>
          * true if the given square is a station and false if not.
          * </returns>
          */
        public bool IsStation()
        {
            return Type == SquareType.Station;
        }

        /**
          * <summary>
          * This function is used to verify if a given square is a company.
          * </summary>
          * <returns>
          * true if the given square is a company and false if not.
          * </returns>
          */
        public bool IsCompany()
        {
            return Type == SquareType.Company;
        }

        /**
          * <summary>
          * This function is used to verify if a given square is any kind of
          * ownable square.
          * </summary>
          * <returns>
          * true if the given square is ownable and false if not.
          * </returns>
          */
        public bool IsOwnable()
        {
            return IsProperty() || IsStation() || IsCompany();
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
        public bool IsGoToJail()
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

