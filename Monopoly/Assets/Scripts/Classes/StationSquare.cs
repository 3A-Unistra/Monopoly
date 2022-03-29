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
          * <param name="id">
          * The new id of the station (should be 5,15,25 or 35).
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
          * <exception cref="Monopoly.Exceptions.WrongPriceException">
          * Throws an exception if the given price is different than 200.
          * </exception>
          */
        public StationSquare(int id, int price, int rent) : base(SquareType.Station, id, price, rent)
        {
            int[] ids = {5,15,25,35};
            List<int> validIdNumbers = new List<int>(ids);
            if (!validIdNumbers.Contains(id))
                throw new Monopoly.Exceptions.WrongIdException
                    ("The id should be a valid station number.");
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
        public static bool IsStationIndex(int idx)
        { 
            return idx == 5 || idx == 15 || idx == 25 || idx == 35;
        }
    }
}
