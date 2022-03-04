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
         * <param name="type">
         * The new type of the community square.
         * </param>
         * <param name="id">
         * The new id of the community square.
         * </param>
         * <param name="name">
         * The new name of the community square on the board.
         * </param>
         * <param name="image">
         * The new image of the community square on the board.
         * </param>
         * <returns>
         * Returns an instance of the CommunitySquare object with the given 
         * type, id, name and image.
         * </returns>
         * <exception cref="Monopoly.Exceptions.WrongIdException">
         * Throws an exception if the given id does not belong to this list
         * {2,17,33}.
         * </exception>
         * <exception cref="Monopoly.Exceptions.WrongTypeException">
         * Throws an exception if the given type is different from
         * SquareType.Community.
         * </exception>
         */
        public CommunitySquare(SquareType type, int id, string name, 
            Material image) : base(type,id,name,image)
        {
            if (id!=2 && id!=17 && id!=33)
                throw new Monopoly.Exceptions.WrongIdException
                    ("The id should be 2,17 or 33.");
            if (type != SquareType.Community)
                throw new Monopoly.Exceptions.WrongTypeException
                    ("The type should be SquareType.Community.");
        }
    }
}