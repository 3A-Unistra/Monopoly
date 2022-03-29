/*
 * ChanceSquare.cs
 * File that models a chance square on the board.
 * 
 * Date created : 21/02/2022
 * Author       : Rayan MARMAR <rayan.marmar@etu.unistra.fr>
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monopoly.Classes
{
    public class ChanceSquare : Square
    {
        /**
         * <summary>
         * Constructor of the class <c>ChanceSquare</c>.
         * </summary>
         * <param name="id">
         * The new id of the chance square.
         * </param>
         * <returns>
         * Returns an instance of the ChanceSquare object with the given 
         * type, id, name and image.
         * </returns>
         * <exception cref="WrongIdException">
         * Throws an exception if the given id does not belong to this list
         * {7,12,36}.
         * </exception>
         */
        public ChanceSquare(int id) : base(SquareType.Chance, id)
        {
        }
        
        /**
          * <summary>
          * This function is used to verify if a given index is
          * an chance square index.
          * </summary>
          * <param name="idx">
          * The index of the given square.
          * </param>
          * <returns>
          * true if the given square is chance and false if not.
          * </returns>
          */
        public static bool IsChanceIndex(int idx)
        {
            return idx == 7 || idx == 22 || idx == 36;
        }
    }
}
