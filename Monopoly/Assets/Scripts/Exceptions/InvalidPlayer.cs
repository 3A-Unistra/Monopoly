/*
 * InvalidPLayer.cs
 * This file contains the Invalid player Exception.
 * 
 * Date created : 27/02/2022
 * Author       : Rayan MARMAR <rayan.marmar@etu.unistra.fr>
 */

using System;

namespace Monopoly.Exceptions
{
    public class InvalidPlayer : Exception
    {
        public InvalidPlayer(string msg) : base(msg)
        {}
    }
}