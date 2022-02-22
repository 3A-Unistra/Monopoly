
public class Board
 {
    private Square board[];
    private List<Card> deckCommunity;
    private List<Card> deckChance;
    private int prisonSquare;
    private int boardMoney;
    public Square getSquare(int pos)
    {
        return board[pos];
    }
    public List<Square> squareOwned(Player p)
    {
        List<square> tempList = new List<square>()
        for(int i = 0;i < 40;i++)
        {
            if(board[i].owner == p)
                tempList.Add(board[i]);
        }
        return tempList;
    }
    public static List<property> getPropertySet(color c)
    {

    }
}
