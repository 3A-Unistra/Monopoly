namespace Monopoly;

public class OwnableSquare : Square
{ 
 /// <summary>
 /// The owner of the property.
 /// </summary>
 private Player owner{ get; set; }
 
 /// <summary>
 /// The status of a property(mortgaged or not).
 /// </summary>
 private bool mortgaged{ get; set; }
 
 /// <summary>
 /// The price of the property that a player needs to pay if he chose to buy
 /// it.
 /// </summary>
 private int price{ get; set; }
 
 /// <summary>
 /// The rent of the property that a player has to pay if he reaches a
 /// property owned by another player.
 /// </summary>
 private int rent{ get; set; }
    
 /**
  * <summary>
  * Constructor of the class <c>OwnableSquare</c>.
  * </summary>
  * <param name="typeCons">
  * The new type of the property.
  * </param>
  * <param name="idCons">
  * The new id of the property.
  * </param>
  * <param name="nameCons">
  * The new name of the property.
  * </param>
  * <param name="imageCons">
  * The new image of the property.
  * </param>
  * <param name="priceCons">
  * The new price of the property.
  * </param>
  * <param name="rentCons">
  * The new rent of the property.
  * </param>
  * <returns>
  * An instance of the ownable square object with the given type, id, name,
  * image, mortgaged status, price and rent.
  * </returns>
  * <exception cref="WrongIdException">
  * Throws an exception if the given id is a negative number, or a number
  * greater than 39 or a number of this list {0,2,4,7,10,17,20,22,30,33,36,38}.
  * </exception>
  * <exception cref="WrongTypeException">
  * Throws an exception if the given type is different than a SquareType.Field,
  * SquareType.Station or SquareType.Company.
  * </exception>
  */
 public OwnableSquare(SquareType typeCons, int idCons, string nameCons,
    Image imageCons, int priceCons, int rentCons)
    : base(typeCons, idCons, nameCons, imageCons)
 {
  owner = null;
  mortgaged = false;
  price = priceCons;
  rent = rentCons;
 }

 /**
  * <summary>
  * This function sets a given player as the owner of the property.
  * </summary>
  * <param name="player">
  * A player of the list of the players playing the game.
  * </param>
  * <exception cref="InvalidPlayer">
  * Throws an exception if the given player is not in the list of the players
  * playing the game.
  * </exception>
  */
 void setOwner(Player player)
 {
  owner = player;
 }

 /**
  * <summary>
  * This function is a getter of the attribute owner.
  * </summary>
  * <returns>
  * The owner of the square.
  * </returns>
  */
 Player getOwner()
 {
  return owner;
 }

 /**
  * <summary>
  * This function is a getter of the attribute rent.
  * </summary>
  * <returns>
  * The rent of the square.
  * </returns>
  */
 int getRent()
 {
  return rent;
 }

 /**
  * <summary>
  * This function changes the ownership of a property and give it to the given
  * player.
  * </summary>
  * <param name="player">
  * A player of the list of the players playing the game.
  * </param>
  * <exception cref="InvalidPlayer">
  * Throws an exception if the given player is not in the list of the players
  * playing the game.
  * </exception>
  */
 void ChangeOwner(Player p)
 {
  owner = p;
 }

 /**
  * <summary>
  * This function is used to put a mortgage on a property.
  * </summary>
  */
 void mortgage()
 {
  mortgaged = true;
 }

 /**
  * <summary>
  * This function is used to remove a property from a mortgage.
  * </summary>
  */
 void unmortgage()
 {
  mortgaged = false;
 }

 /**
  * <summary>
  * This function is used to deduct the given rent from the player's money.
  * </summary>
  * <exception cref="InvalidPlayer">
  * Throws an exception if the given player is not in the list of the players
  * playing the game.
  * </exception>
  */
 void payRent(Player renter)
 {
  renter.money -= rent;
 }

 /*
 static bool isSameGroup(OwnableSquare a, OwnableSquare b)
 {
  return OwnableSquare
 }*/
}
