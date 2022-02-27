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

namespace Monopoly.Classes
{
    /**
    * <summary>
    * Class Card defining the cards of a monopoly
    * type telling if the card is CHANCE or COMMUNITY
    * a different id for each cards of a deck
    * desc is a string telling what the card does
    * </summary>
    */
    public class Card
    {
        /**
        * <summary>
        * type tells if the card is chance or community
        * </summary>
        */
        public string type;
        /**
        * <summary>
        * a unique id for each cards in the card list deck
        * </summary>
        */        
        public int id;
        /**
        * <summary>
        * desc shows the player what the card does
        * </summary>
        */        
        public string desc;
        /**
        * <summary>
        * Card constructor 
        * </summary>
        */         
        public Card(string type, int id, string desc)
        {
            this.id = id;
            this.type = type;
            this.desc = desc;
        }
    }
}
