/*
 * SquareType.cs
 * File that contains all the possible types of the squares on the board.
 * 
 * Date created : 22/02/2022
 * Author       : Rayan MARMAR <rayan.marmar@etu.unistra.fr>
 *              : Finn RAYMENT <rayment@etu.unistra.fr>
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monopoly.Classes
{
    /**
      *<summary>
      *Enumeration of the 10 possible types of squares on the board.
      *</summary>
      */
     public enum SquareType
     {
         /**
          * <summary>
          * First square of the board where all the player start their game and where
          * they receive 200 each time they pass by this square.
          * </summary>
          */
         Go = 0,
         /**
          * <summary>
          * This type identifies the two tax squares existing in the board where the
          * player should pay the given amount each time he reaches these squares.
          * </summary>
          */
         Tax = 1,
         /**
          * <summary>
          * This type identifies the 22 squares of fields existing on the board that
          * can be bought, sold, mortgaged or auctioned.
          * </summary>
          */
         Field = 2,
         /**
          * <summary>
          * This type identifies the 4 stations' squares existing on the board that
          * can be bought, sold, mortgaged or auctioned.
          * </summary>
          */
         Station = 3,
         /**
          * <summary>
          * This type identifies the 2 companies' squares existing on the board that
          * can be bought, sold, mortgaged or auctioned.
          * </summary>
          */
         Company = 4,
         /**
          * <summary>
          * This is the square where a player who is imprisoned will be until he
          * exits the prison.
          * </summary>
          */
         Prison = 5,
         /**
          * <summary>
          * This square sends all the players who reach it to the prison.
          * </summary>
          */
         GoToJail = 6,
         /**
          * <summary>
          * This square is where a player can get all the money that is put in the
          * middle of the board.
          * </summary>
          */
         Parking = 7,
         /**
          * <summary>
          * This square is where the player gets a community card when he reaches it.
          * </summary>
          */
         Community = 8,
         /**
          * <summary>
          * This square is where the player gets a chance card when he reaches it.
          * </summary>
          */
         Chance = 9
     }
}
