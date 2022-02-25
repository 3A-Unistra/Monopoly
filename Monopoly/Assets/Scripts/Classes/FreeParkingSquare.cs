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
         * <param name="type">
         * The new type of the free parking square.
         * </param>
         * <param name="id">
         * The new id of the free parking square.
         * </param>
         * <param name="name">
         * The new name of the free parking square on the board.
         * </param>
         * <param name="image">
         * The new image of the free parking square on the board.
         * </param>
         * <returns>
         * Returns an instance of the FreeParkingSquare object with the given
         * type, id, name and image.
         * </returns>
         * <exception cref="WrongIdException">
         * Throws an exception if the given id is not 20.
         * </exception>
         * <exception cref="WrongTypeException">
         * Throws an exception if the given type is different from
         * SquareType.Parking.
         * </exception>
         */
        public FreeParkingSquare(SquareType type, int id, string name, 
            Material image) : base(type,id,name,image)
        {
        }
    }
}