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
        * a unique id for each cards in the card list deck
        * </summary>
        */       
        public int id;

        /**
         * <summary>
         * if true the card is community card
         * else it is a chance card
         * </summary>
         */
        public bool community;

        /**
        * <summary>
        * Card constructor 
        * </summary>
        */
        public Card(int id, bool community)
        {
            this.id = id;
            this.community = community;
        }

        public static bool IsChanceCardIndex(int idx)
        {
            return idx >= 1 && idx <= 16;
        }

        public static bool IsCommunityCardIndex(int idx)
        {
            return idx >= 17 && idx <= 32;
        }

        public enum CardType
        {
            RECEIVE_BANK = 0,
            GIVE_BOARD = 1,
            MOVE_BACKWARD = 2,
            GOTO_POSITION = 3,
            GOTO_JAIL = 4,
            GIVE_ALL = 5,
            RECEIVE_ALL = 6,
            LEAVE_JAIL = 7,
            CLOSEST_STATION = 8,
            CLOSEST_COMPANY = 9,
            GIVE_BOARD_HOUSES = 10
        }

    }

}