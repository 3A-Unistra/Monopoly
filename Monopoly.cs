using System;
namespace Monopoly
{
class Terrain
 {
    public string color;
    public int owner;
    public int price;
    public int house_cost;
    public int nb_house;
    public int base_rent;
    public int house1_rent;
    public int house2_rent;
    public int house3_rent;
    public int house4_rent;
    public int hotel_rent;
    public string name;
    public Terrain(int position,string color,string name,int price,int house_cost,int base_rent,
    int house1_rent,int house2_rent,int house3_rent,int house4_rent, int hotel_rent)
    {
        this.name=name;
        this.color=color;
        this.price=price;
        this.house_cost=house_cost;
        this.base_rent=base_rent;
        this.house1_rent=house1_rent;
        this.house2_rent=house2_rent;
        this.house3_rent=house3_rent;
        this.house4_rent=house4_rent;
        this.hotel_rent=hotel_rent;
        this.owner=0;
        this.nb_house=0;

    }
    public void change_owner(int id)
    {
        owner=id;
    }
 }

 class Player
 {
    public string name;
    public int id;
    public int money;
    public int position;
    public int order;
    public bool did_double;
    public int doubles;
    public int jail_turns;    
    public List<int> card_owned= new List<int>();
    public Player (string name,int id,int order)
    {

        this.name=name;
        this.id=id;
        this.position=0;
        this.doubles=0;
        this.jail_turns=0;
        this.money=1500;
        this.order=order;
        this.did_double=false;
        card_owned= new List<int>();

    }
    public void move(int roll)
    {

        if(position+roll>=40)
        {
            money += 200;
        }
        position=(position+roll)%40;
    
    }
    public void in_jail()
    {
        doubles=0;
         if(did_double)
        {
             jail_turns=0;
             Console.WriteLine(name+" s'est évadé");
        }
        else if(jail_turns==3)
         {
            Console.WriteLine(name+" paie sa caution et sort de prison");
            money -=50;
            jail_turns=0;
        }
        else
        {
            Console.WriteLine(name+" passe son tour en prison");            
            jail_turns++;
        }
    }
    public void buy_house(Terrain t)
    {
        money -= t.house_cost;
    }
 }
 class Card
 {
     public string type;
     public int id;
     public string desc;
     public Card(string type,int id,string desc)
     {
        this.id=id;
        this.type=type;
        this.desc=desc;

     }
 }
 class Bank 
 {
     public int nb_house;
     public int nb_hotel;
     public int stored_money;
     public Bank(int nb_house,int nb_hotel,int stored_money)
     {
         this.nb_house=nb_house;
         this.nb_hotel=nb_hotel;
         this.stored_money=stored_money;

     }
 }
 class Train_Station
 {
    public int owner;
    public int price;
    public int base_rent;     
    public Train_Station()
    {
        this.owner=0;
        this.price=200;
        this.base_rent=50;
    }
 }



  class Program
  {
   public static string[] Board={"Start","Terrain","Community","Terrain","Tax","Train_Station","Terrain","Chance","Terrain","Terrain"
    ,"Jail","Terrain","Electric_Company","Terrain","Terrain","Train_Station","Terrain","Community","Terrain","Terrain","Free_Parking","Terrain","Chance","Terrain"
    ,"Terrain","Train_Station","Terrain","Terrain","Water_Works","Terrain","Go_To_Jail","Terrain","Terrain","Community","Terrain","Train_Station",
    "Chance","Terrain","Super_Tax","Terrain"};
    public static Player[] init_player(int nb_player)
    {
        Player[] opponents = new Player[nb_player];
        for(int i=0;i<nb_player;i++)
        {
           Console.WriteLine("Entrez le nom du joueur");
           string p_name= Console.ReadLine();
           opponents[i]=new Player(p_name,i+1, i);
        }
        return opponents;
    }

    public static void turn(Player p)
    {
        Random rnd=new Random();
        int dice1=rnd.Next(1, 6);
        int dice2=rnd.Next(1, 6);
        p.did_double=(dice1==dice2);
        int res=dice1+dice2;
        if(p.did_double && (p.doubles==2))
        {
            Console.WriteLine(p.name+" a fait un excès de vitesse !\nDirection la prison !");
            p.position=10;
            p.jail_turns=1;
            p.doubles=0;            
        }
        else if(p.jail_turns>0 )
        {
            p.in_jail();
            
        }
        else{
        if(p.did_double)
        {   
            Console.WriteLine(dice1+"  "+dice1+"  " +p.name+ " a fait un double ! \n    " + p.name  + "avance de "+res+" cases et gagne un tour!");
            p.doubles++;
        }
        else
        {
           Console.WriteLine(p.name+ " avance de "+res+" cases.");
           p.doubles=0; 
        }
        p.move(res);
        Console.WriteLine("La position de "+p.name +" est "+p.position); 
        Console.WriteLine(p.name+" est sur "+Board[p.position]);
        if(Board[p.position]=="Go_To_Jail")
        {
            p.position=10;
            p.jail_turns=1;
        }
        }
    }
      static void Main(string[] args)
    {
        Console.WriteLine("Entrez le nombre de joueurs");
        int nb_player=Convert.ToInt32(Console.ReadLine());
         Player[] opponents  = init_player(nb_player) ;
        if((nb_player>8) ||(nb_player<2) )
        {
            Console.WriteLine("Erreur nombre de joueurs hors limite");
        }
        else
        {
            Console.WriteLine("il y a "+ nb_player+" joueurs.");
        }
        Console.WriteLine("Entrez le nombre de tours que vous voulez jouer");
        int tours=Convert.ToInt32(Console.ReadLine());
        for(int i=0;i<tours;i++)
        {
            for (int j=0;j<nb_player;j++)
            {
                Console.ReadKey();            
                turn(opponents[j]);
                if(opponents[j].did_double)
                {
                    j--;
                }  
            }
          

            
        }
        
        Console.ReadKey();
    }
  }  
}