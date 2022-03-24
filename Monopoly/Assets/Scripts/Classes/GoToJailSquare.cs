/*
 * GoToJailSquare.cs
 * File that models a go to jail square on the board.
 * 
 * Date created : 24/02/2022
 * Author       : Rayan MARMAR <rayan.marmar@etu.unistra.fr>
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monopoly.Classes
{
    public class GoToJailSquare : Square
    {
        /**
         * <summary>
         * Constructor of the class <c>GoToJailSquare</c>.
         * </summary>
         * <param name="type">
         * The new type of the got to jail square.
         * </param>
         * <param name="id">
         * The new id of the got to jail square.
         * </param>
         * <param name="name">
         * The new name of the got to jail square on the board.
         * </param>
         * <param name="image">
         * The new image of the got to jail square on the board.
         * </param>
         * <returns>
         * Returns an instance of the JailSquare object with the given type, id,
         * name and image.
         * </returns>
         * <exception cref="Monopoly.Exceptions.WrongIdException">
         * Throws an exception if the given id is not 30.
         * </exception>
         * <exception cref="Monopoly.Exceptions.WrongTypeException">
         * Throws an exception if the given type is different from SquareType.GoToJail.
         * </exception>
         */
        public GoToJailSquare(SquareType type, int id, string name, Material image)
            : base(type,id,name,image)
        {
            if (id!=30)
                throw new Monopoly.Exceptions.WrongIdException
                    ("The id should be 30.");
            if (type != SquareType.GoToJail)
                throw new Monopoly.Exceptions.WrongTypeException
                    ("The type should be SquareType.GoToJail.");
        }
        /**
          * <summary>
          * This function is used to verify if a given index is
          * an go to jail square index.
          * </summary>
          * <param name="idx">
          * The index of the given square.
          * </param>
          * <returns>
          * true if the given square is go to jail and false if not.
          * </returns>
          */
        public static bool IsGoToJailIndex(int idx)
        {
            return idx == 30;
        }
    }
}
