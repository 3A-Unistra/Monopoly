/*
 * WrongTypeException .cs
 * This file contains the Wrong Type Exception.
 * 
 * Date created : 27/02/2022
 * Author       : Rayan MARMAR <rayan.marmar@etu.unistra.fr>
 */

using System;

namespace Monopoly.Exceptions
{
    public class WrongTypeException : Exception
    {
        public  WrongTypeException(string msg) : base(msg)
        {}
    }
}