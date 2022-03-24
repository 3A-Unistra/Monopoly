/*
 * InvalidPlayerException.cs
 * This file contains the Invalid player Exception.
 * 
 * Date created : 27/02/2022
 * Author       : Rayan MARMAR <rayan.marmar@etu.unistra.fr>
 */

using System;

namespace Monopoly.Exceptions
{
    public class InvalidPlayerException : Exception
    {
        public InvalidPlayerException(string msg) : base(msg)
        {}
    }
}