/*
 * JailSquare.cs
 * File that models a jail square on the board.
 * 
 * Date created : 24/02/2022
 * Author       : Rayan MARMAR <rayan.marmar@etu.unistra.fr>
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monopoly.Classes
{
    public class JailSquare : Square
    {
        /**
         * <summary>
         * Constructor of the class <c>JailSquare</c>.
         * </summary>
         * <param name="type">
         * The new type of the jail square.
         * </param>
         * <param name="id">
         * The new id of the jail square.
         * </param>
         * <param name="name">
         * The new name of the jail square on the board.
         * </param>
         * <param name="image">
         * The new image of the jail square on the board.
         * </param>
         * <returns>
         * Returns an instance of the JailSquare object with the given type, id,
         * name and image.
         * </returns>
         * <exception cref="WrongIdException">
         * Throws an exception if the given id is not 10.
         * </exception>
         * <exception cref="WrongTypeException">
         * Throws an exception if the given type is different from SquareType.Prison.
         * </exception>
         */
        public JailSquare(SquareType type, int id, string name, Material image)
            : base(type,id,name,image)
        {
        }
    }
}