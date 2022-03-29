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
         * </summary>>
         * <param name="id">
         * The new id of the Tax square.
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
         */
        public TaxSquare(int id, int taxPrice) : base(SquareType.Tax, id)
        {
            TaxPrice = taxPrice;
            if (id!=4 && id!=38)
                throw new Monopoly.Exceptions.WrongIdException
                    ("The id should be 4 or 38.");
        }
        
        /**
          * <summary>
          * This function is used to verify if a given index is
          * an tax square index.
          * </summary>
          * <param name="idx">
          * The index of the given square.
          * </param>
          * <returns>
          * true if the given square is tax and false if not.
          * </returns>
          */
        public static bool IsTaxIndex(int idx)
        {
            return idx == 4 || idx == 38;
        }
    }
}
