/*
 * GoSquare.cs
 * File that models a Go square on the board.
 * 
 * Date created : 24/02/2022
 * Author       : Rayan MARMAR <rayan.marmar@etu.unistra.fr>
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monopoly.Classes
{
    public class GoSquare : Square
    {
        /**
         * <summary>
         * Constructor of the class <c>GoSquare</c>.
         * </summary>
         * <param name="type">
         * The new type of the Go square.
         * </param>
         * <param name="id">
         * The new id of the Go square (should be 0).
         * </param>
         * <param name="name">
         * The new name of the Go square on the board.
         * </param>
         * <param name="image">
         * The new image of the Go square on the board.
         * </param>
         * <returns>
         * Returns an instance of the Go Square object with the given type, id,
         * name and image.
         * </returns>
         * <exception cref="WrongIdException">
         * Throws an exception if the given id is not 0.
         * </exception>
         * <exception cref="WrongTypeException">
         * Throws an exception if the given type is different from SquareType.Go.
         * </exception>
         */
        public GoSquare(SquareType type, int id, string name, Material image)
            : base(type,id,name,image)
        {
        }
    }
}