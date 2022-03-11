/*
 * StationSquare.cs
 * File that contains the station squares class.
 * 
 * Date created : 23/02/2022
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
     * class and models a station square on the board.
     * </summary>
     */
    public class StationSquare : OwnableSquare
    {
        /**
          * <summary>
          * Constructor of the class <c>StationSquare</c>.
          * </summary>
          * <param name="type">
          * The new type of the station (should be Station).
          * </param>
          * <param name="id">
          * The new id of the station (should be 5,15,25 or 35).
          * </param>
          * <param name="name">
          * The new name of the station.
          * </param>
          * <param name="image">
          * The new image of the station.
          * </param>
          * <param name="price">
          * The new price of the station (should be 200).
          * </param>
          * <param name="rent">
          * The new rent of the station.
          * </param>
          * <returns>
          * An instance of the station square object with the given type, id,
          * name, image, mortgaged status, price and rent.
          * </returns>
          * <exception cref="Monopoly.Exceptions.WrongIdException">
          * Throws an exception if the given id does not exist in this list
          * {5,15,25,35}.
          * </exception>
          * <exception cref="Monopoly.Exceptions.WrongTypeException">
          * Throws an exception if the given type is different than a
          * SquareType.Station.
          * </exception>
          * <exception cref="Monopoly.Exceptions.WrongPriceException">
          * Throws an exception if the given price is different than 200.
          * </exception>
          */
        public StationSquare(SquareType type, int id, string name,
            Material image, int price, int rent)
            : base(type, id, name, image, price, rent)
        {
            int[] ids = {5,15,25,35};
            List<int> validIdNumbers = new List<int>(ids);
            if (!validIdNumbers.Contains(id))
                throw new Monopoly.Exceptions.WrongIdException
                    ("The id should be a valid station number.");
            if (type != SquareType.Station)
                throw new Monopoly.Exceptions.WrongTypeException
                    ("The type should be SquareType.Station.");
        }
        
        /**
          * <summary>
          * This function is used to verify if a given index is
          * an station square index.
          * </summary>
          * <param name="idx">
          * The index of the given square.
          * </param>
          * <returns>
          * true if the given square is station and false if not.
          * </returns>
          */
        public bool IsStationIndex(int idx)
        {
            SquareType type = Board.Elements[idx].Type;
            return type == SquareType.Station;
        }
    }
}
