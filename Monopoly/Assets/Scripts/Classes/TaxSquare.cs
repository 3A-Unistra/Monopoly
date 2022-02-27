/*
 * TaxSquare.cs
 * File that models a Tax square on the board.
 * 
 * Date created : 24/02/2022
 * Author       : Rayan MARMAR <rayan.marmar@etu.unistra.fr>
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monopoly.Classes
{
    public class TaxSquare : Square
    {
        public int TaxPrice { get; set; }

        /**
         * <summary>
         * Constructor of the class <c>TaxSquare</c>.
         * </summary>
         * <param name="type">
         * The new type of the Tax square.
         * </param>
         * <param name="id">
         * The new id of the Tax square.
         * </param>
         * <param name="name">
         * The new name of the Tax square on the board.
         * </param>
         * <param name="image">
         * The new image of the Tax square on the board.
         * </param>
         * <param name="taxPrice">
         * The new price of the tax that player who lands on this square has to pay.
         * </param>
         * <returns>
         * Returns an instance of the TaxSquare object with the given type, id, name and
         * image.
         * </returns>
         * * <exception cref="Monopoly.Exceptions.WrongIdException">
         * Throws an exception if the given id is different from 4 and 38.
         * </exception>
         * <exception cref="Monopoly.Exceptions.WrongTypeException">
         * Throws an exception if the given type is different than a
         * SquareType.Tax.
         * </exception>
         */
        public TaxSquare(SquareType type, int id, string name,
            Material image, int taxPrice) : base(type,id,name,image)
        {
            TaxPrice = taxPrice;
            if (id!=4 && id!=38)
                throw new Monopoly.Exceptions.WrongIdException
                ("The id should be a valid tax square number (4 or 38).");
            if (type != SquareType.Tax)
                throw new Monopoly.Exceptions.WrongTypeException
                ("The type should be SquareType.Tax.");
        }
    }
}