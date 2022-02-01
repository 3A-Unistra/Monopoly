using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSocket
{
    public int x;
    public int y;
    public BoardSocket(int x, int y)
    {
        this.x=x;
        this.y=y;
    }
}

public class PawnMovement : MonoBehaviour 
{
    public float Movespeed;
    void Start ()
    {
        RG2D=GetComponent<Rigidbody2D>();
        BoardSocket b1=new BoardSocket(0,0);
        BoardSocket b2=new BoardSocket(4,0);
        Movespeed=10f;
    }
    void Update()
    {
        if(RG2D.x != b2.x)
        {
            RG2D.velocity=new Vector(Movespeed,0)
        }
    }
}
