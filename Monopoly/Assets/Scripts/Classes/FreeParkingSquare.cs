/*
 * FreeParkingSquare.cs
 * File that models a free parking square on the board.
 * 
 * Date created : 21/02/2022
 * Author       : Rayan MARMAR <rayan.marmar@etu.unistra.fr>
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monopoly.Classes
{
    public class FreeParkingSquare : Square
    {
        /**
         * <summary>
         * Constructor of the class <c>FreeParkingSquare</c>.
         * </summary>
         * <param name="id">
         * The new id of the free parking square.
         * </param>
         * <returns>
         * Returns an instance of the FreeParkingSquare object with the given
         * type, id, name and image.
         * </returns>
         * <exception cref="Monopoly.Exceptions.WrongIdException">
         * Throws an exception if the given id is not 20.
         * </exception>
         */
        public FreeParkingSquare(int id) : base(SquareType.Parking, id)
        {
            if (id!=20)
                throw new Monopoly.Exceptions.WrongIdException
                    ("The id should be 20.");
        }
        
        /**
          * <summary>
          * This function is used to verify if a given index is
          * an free parking square index.
          * </summary>
          * <param name="idx">
          * The index of the given square.
          * </param>
          * <returns>
          * true if the given square is free parking and false if not.
          * </returns>
          */
        public static bool IsFreeParkingIndex(int idx)
        {
            return idx == 20;
        }
    }
}
