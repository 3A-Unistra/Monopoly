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
         * <exception cref="Monopoly.Exceptions.WrongIdException">
         * Throws an exception if the given id is not 0.
         * </exception>
         * <exception cref="Monopoly.Exceptions.WrongTypeException">
         * Throws an exception if the given type is different from SquareType.Go.
         * </exception>
         */
        public GoSquare(SquareType type, int id, string name, Material image)
            : base(type,id,name,image)
        {
            if (id!=0)
                throw new Monopoly.Exceptions.WrongIdException
                    ("The id should be 0.");
            if (type != SquareType.Go)
                throw new Monopoly.Exceptions.WrongTypeException
                    ("The type should be SquareType.Go.");
<<<<<<< HEAD
=======
        }
        
        /**
          * <summary>
          * This function is used to verify if a given index is
          * an Go square index.
          * </summary>
          * <param name="idx">
          * The index of the given square.
          * </param>
          * <returns>
          * true if the given square is Go and false if not.
          * </returns>
          */
        public bool IsGoIndex(int idx)
        {
            return idx == 0;
>>>>>>> 6409f5299c62ce0514c4d312e824592a6878c443
        }
    }
}
