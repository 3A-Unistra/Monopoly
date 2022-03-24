/*
 * InvalidStateException.cs
 * This file contains the Invalid player Exception.
 * 
 * Date created : 24/03/2022
 * Author       : Finn RAYMENT <rayment@etu.unistra.fr>
 */

using System;

namespace Monopoly.Exceptions
{
    public class InvalidStateException : Exception
    {
        public InvalidStateException(string msg) : base(msg)
        {}
    }
}