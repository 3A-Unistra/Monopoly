using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Monopoly;

/**
 *<summary>
 * Abstract class <c>Square</c> models a square on the board
 * </summary>
 */
public abstract class Square
{
 /**
  * <summary>
  *Enumeration of the 10 possible types of squares on the board.
  * </summary>
  */
 public enum SquareType
 {
  /// <summary>
  /// First square of the board where all the player start their game and where
  /// they receive 200 each time they pass by this square.
  /// </summary>
  Go,
  /// <summary>
  /// This type identifies the two tax squares existing in the board where the
  /// player should pay the given amount each time he reaches these squares.
  /// </summary>
  Tax,
  /// <summary>
  /// This type identifies the 22 squares of fields existing on the board that
  /// can be bought, sold, mortgaged or auctioned.
  /// </summary>
  Field,
  /// <summary>
  /// This type identifies the 4 stations' squares existing on the board that
  /// can be bought, sold, mortgaged or auctioned.
  /// </summary>
  Station,
  /// <summary>
  /// This type identifies the 2 companies' squares existing on the board that
  /// can be bought, sold, mortgaged or auctioned.
  /// </summary>
  Company,
  /// <summary>
  /// This is the square where a player who is imprisoned will be until he
  /// exits the prison.
  /// </summary>
  Prison,
  /// <summary>
  /// This square sends all the players who reach it to the prison.
  /// </summary>
  GoToJail,
  /// <summary>
  /// This square is where a player can get all the money that is put in the
  /// middle of the board.
  /// </summary>
  Parking,
  /// <summary>
  /// This square is where the player gets a community card when he reaches it.
  /// </summary>
  Community,
  /// <summary>
  /// This square is where the player gets a chance card when he reaches it.
  /// </summary>
  Chance
 }
 
 /// <summary>
 /// The type of the square.
 /// </summary>
 private SquareType type{ get; set; }
 
 /// <summary>
 /// The name of the square that will be written on the board.
 /// </summary>
 private string name{ get; set; }
 
 /// <summary>
 /// The id of the square is the unique number that represents its placement
 /// on the board from 0 being the first square to 39 being the last.
 /// </summary>
 private int id{ get; set; }
 
 /// <summary>
 /// The image that represents the square that will be shown on the board.
 /// </summary>
 private Image image{ get; set; }
 
 /**
  * <summary>
  * Constructor of the class <c>Square</c>.
  * </summary>
  * <param name="typeCons">
  * The new type of the square.
  * </param>
  * <param name="idCons">
  * The new id of the square.
  * </param>
  * <param name="nameCons">
  * The new name of the square on the board.
  * </param>
  * <param name="imageCons">
  * The new image of the square on the board.
  * </param>
  * <returns>
  * Returns an instance of the Square object with the given type, id, name and
  * image.
  * </returns>
  * * <exception cref="WrongIdException">
  * Throws an exception if the given id is a negative number, or a number
  * greater than 39.
  * </exception>
  */
 protected Square(SquareType typeCons, int idCons, 
  string nameCons, Image imageCons)
 {
  type = typeCons;
  id = idCons;
  name = nameCons;
  image = imageCons;
 }

 /**
  * <summary>
  * This function is a getter of the attribute type.
  * </summary>
  * <returns>
  * The type of the square.
  * </returns>
  */
 SquareType getType()
 {
  return type;
 }

 /**
  * <summary>
  * This function is a getter of the attribute name.
  * </summary>
  * <returns>
  * The name of the square.
  * </returns>
  */
 string getName()
 {
  return name;
 }

 /**
  * <summary>
  * This function is a getter of the attribute id.
  * </summary>
  * <returns>
  * The id of the square.
  * </returns>
  */
 int getId()
 {
  return id;
 }

 /**
  * <summary>
  * This function is used to verify if a given square is a property.
  * </summary>
  * <returns>
  * true if the given square is a property and false if not.
  * </returns>
  */
 bool isProperty()
 {
  return type == SquareType.Field || 
         type == SquareType.Station || 
         type == SquareType.Company;
 }

 /**
  * <summary>
  * This function is used to verify if a given square is a tax square.
  * </summary>
  * <returns>
  * true if the given square is a tax square and false if not.
  * </returns>
  */
 bool isTax()
 {
  return type == SquareType.Tax;
 }

 /**
  * <summary>
  * This function is used to verify if a given square is the Go square.
  * </summary>
  * <returns>
  * true if the given square is is the Go square and false if not.
  * </returns>
  */
 bool isGo()
 {
  return type == SquareType.Go;
 }

 /**
  * <summary>
  * This function is used to verify if a given square is the free parking
  * square.
  * </summary>
  * <returns>
  * true if the given square is is the free parking square and false if not.
  * </returns>
  */
 bool isFreeParking()
 {
  return type == SquareType.Parking;
 }

 /**
  * <summary>
  * This function is used to verify if a given square is the go to jail square.
  * </summary>
  * <returns>
  * true if the given square is is the go to jail square and false if not.
  * </returns>
  */
 bool isGoToJail()
 {
  return type == SquareType.GoToJail;
 }

 /**
  * <summary>
  * This function is used to verify if a given square is a community square.
  * </summary>
  * <returns>
  * true if the given square is a community square and false if not.
  * </returns>
  */
 bool isCommunityChest()
 {
  return type == SquareType.Community;
 }

 /**
  * <summary>
  * This function is used to verify if a given square is a chance square.
  * </summary>
  * <returns>
  * true if the given square is a chance square and false if not.
  * </returns>
  */
 bool isChance()
 {
  return type == SquareType.Chance;
 }
}