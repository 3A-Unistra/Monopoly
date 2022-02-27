/*
 * WrongIdException .cs
 * This file contains the Wrong Id Exception.
 * 
 * Date created : 27/02/2022
 * Author       : Rayan MARMAR <rayan.marmar@etu.unistra.fr>
 */

using System;

namespace Monopoly.Exceptions
{
    public class WrongIdException : Exception
    {
        public  WrongIdException(string msg) : base(msg)
        {}
    }
}