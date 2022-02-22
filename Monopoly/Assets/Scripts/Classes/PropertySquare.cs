/*
 * PropertySquare.cs
 * File that contains all the functions used for all the field squares on the
 * board.
 * 
 * Date created : 22/02/2022
 * Author       : Rayan MARMAR <rayan.marmar@etu.unistra.fr>
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monopoly.Classes
{
    public class PropertySquare : OwnableSquare
    {
        public Color Col { get; set; }
        
        public PropertySquare(SquareType typeCons, int idCons, string nameCons,
            Material imageCons, int priceCons, int rentCons, Color colorCons)
            : base(typeCons, idCons, nameCons, imageCons, priceCons, rentCons)
        {
            Col = colorCons;
        }
    }
}

