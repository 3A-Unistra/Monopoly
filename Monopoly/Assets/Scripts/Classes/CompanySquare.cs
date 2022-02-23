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
    /**
      * <summary>
      * Constructor of the class <c>OwnableSquare</c>.
      * </summary>
      * <param name="type">
      * The new type of the company (should be Comapany).
      * </param>
      * <param name="id">
      * The new id of the company (should be 12 or 28).
      * </param>
      * <param name="name">
      * The new name of the company.
      * </param>
      * <param name="image">
      * The new image of the company.
      * </param>
      * <param name="price">
      * The new price of the company (should be 150).
      * </param>
      * <param name="rent">
      * The new rent of the company.
      * </param>
      * <returns>
      * An instance of the company square object with the given type, id,
      * name, image, mortgaged status, price and rent.
      * </returns>
      * <exception cref="WrongIdException">
      * Throws an exception if the given id does not exist in this list
      * {12,28}.
      * </exception>
      * <exception cref="WrongTypeException">
      * Throws an exception if the given type is different than a
      * SquareType.Company.
      * </exception>
      * <exception cref="WrongPriceException">
      * Throws an exception if the given price is different than 150.
      * </exception>
      */
    public class CompanySquare : OwnableSquare
    {
        public CompanySquare(SquareType type, int id, string name, 
            Material image, int price, int rent) 
            : base(type, id, name, image, price, rent)
        { }
    }
}