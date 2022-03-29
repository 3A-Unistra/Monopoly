/*
 * CompanySquare.cs
 * File that contains the company squares class.
 * 
 * Date created : 23/02/2022
 * Author       : Rayan MARMAR <rayan.marmar@etu.unistra.fr>
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monopoly.Classes
{
    public class CompanySquare : OwnableSquare
    {
        /**
          * <summary>
          * Constructor of the class <c>CompanySquare</c>.
          * </summary>
          * <param name="id">
          * The new id of the company (should be 12 or 28).
          * </param>
          * <param name="price">
          * The new price of the company (should be 150).
          * </param>
          * <returns>
          * An instance of the company square object with the given type, id,
          * name, image, mortgaged status, price and rent.
          * </returns>
          * <exception cref="Monopoly.Exceptions.WrongIdException">
          * Throws an exception if the given id does not exist in this list
          * {12,28}.
          * </exception>
          * <exception cref="Monopoly.Exceptions.WrongPriceException">
          * Throws an exception if the given price is different than 150.
          * </exception>
          */
        public CompanySquare( int id, int price) : base(SquareType.Company, id, price, -1)
        {
            if (id!=12 && id!=28)
                throw new Monopoly.Exceptions.WrongIdException
                    ("The id should be 12 or 28.");
            if (price != 150)
                throw new Monopoly.Exceptions.WrongPriceException
                    ("The company's price should be 150");
        }
        
        /**
          * <summary>
          * This function is used to verify if a given index is
          * an company square index.
          * </summary>
          * <param name="idx">
          * The index of the given square.
          * </param>
          * <returns>
          * true if the given square is company and false if not.
          * </returns>
          */
        public static bool IsCompanyIndex(int idx)
        {
            return idx == 12 || idx == 28;
        }
    }
}
