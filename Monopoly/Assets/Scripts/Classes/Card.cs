/*
 * Fichier.cs
 * Fichier définissant la classe Carte
 * 
 * Date created : 22/02/2022
 * Author       : Christophe Pierson <christophe.pierson@etu.unistra.fr>
 */


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
* <summary>
* Class Card defining the cards of a monopoly
* type telling if the card is CHANCE or COMMUNITY
* a different id for each cards of a deck
* desc is a string telling what the card does
* </summary>
*/

namespace Monopoly.Classes
{
    public class Card
    {
        public string type;
        public int id;
        public string desc;
        public Card(string type, int id, string desc)
        {
            this.id = id;
            this.type = type;
            this.desc = desc;
        }
    }
}
