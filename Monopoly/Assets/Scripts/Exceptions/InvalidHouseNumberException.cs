/*
 * InvalidHouseNumberException.cs
 * This file contains the Invalid House number Exception.
 * 
 * Date created : 27/02/2022
 * Author       : Rayan MARMAR <rayan.marmar@etu.unistra.fr>
 */


using System;

namespace Monopoly.Exceptions
{
    public class InvalidHouseNumberException : Exception
    {
        public InvalidHouseNumberException(string msg) : base(msg)
        {}
    }
}