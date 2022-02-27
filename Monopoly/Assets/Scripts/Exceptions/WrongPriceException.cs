/*
 * WrongPriceException .cs
 * This file contains the Wrong Price Exception.
 * 
 * Date created : 27/02/2022
 * Author       : Rayan MARMAR <rayan.marmar@etu.unistra.fr>
 */

using System;

namespace Monopoly.Exceptions
{
    public class WrongPriceException : Exception
    {
        public WrongPriceException(string msg) : base(msg) 
        {}
    }
}