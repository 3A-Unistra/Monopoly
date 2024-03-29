/*
 * CommunitySquare.cs
 * File that models a community square on the board.
 * 
 * Date created : 24/02/2022
 * Author       : Rayan MARMAR <rayan.marmar@etu.unistra.fr>
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monopoly.Classes
{
    public class CommunitySquare : Square
    {
        /**
         * <summary>
         * Constructor of the class <c>CommunitySquare</c>.
         * </summary>
         * <param name="id">
         * The new id of the community square.
         * </param>
         * <returns>
         * Returns an instance of the CommunitySquare object with the given 
         * type, id, name and image.
         * </returns>
         * <exception cref="Monopoly.Exceptions.WrongIdException">
         * Throws an exception if the given id does not belong to this list
         * {2,17,33}.
         * </exception>
         */
        public CommunitySquare(int id) : base(SquareType.Community, id)
        {
            if (id!=2 && id!=17 && id!=33)
                throw new Monopoly.Exceptions.WrongIdException
                    ("The id should be 2,17 or 33.");
        }
        
        /**
          * <summary>
          * This function is used to verify if a given index is
          * an community square index.
          * </summary>
          * <param name="idx">
          * The index of the given square.
          * </param>
          * <returns>
          * true if the given square is community and false if not.
          * </returns>
          */
        public static bool IsCommunityIndex(int idx)
        {
            return idx == 2 || idx == 17 || idx == 33;
        }
    }
}
