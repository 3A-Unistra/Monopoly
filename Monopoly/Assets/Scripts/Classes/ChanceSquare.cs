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
         * <param name="type">
         * The new type of the chance square.
         * </param>
         * <param name="id">
         * The new id of the chance square.
         * </param>
         * <param name="name">
         * The new name of the chance square on the board.
         * </param>
         * <param name="image">
         * The new image of the chance square on the board.
         * </param>
         * <returns>
         * Returns an instance of the ChanceSquare object with the given 
         * type, id, name and image.
         * </returns>
         * <exception cref="WrongIdException">
         * Throws an exception if the given id does not belong to this list
         * {7,12,36}.
         * </exception>
         * <exception cref="WrongTypeException">
         * Throws an exception if the given type is different from
         * SquareType.Chance.
         * </exception>
         */
        public ChanceSquare(SquareType type, int id, string name, 
            Material image) : base(type,id,name,image)
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
        public bool IsChanceIndex(int idx)
        {
            SquareType type = Board.Elements[idx].Type;
            return type == SquareType.Chance;
        }
    }
}
