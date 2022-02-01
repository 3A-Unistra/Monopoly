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
        Movespeed=10f;
    }
    void Update()
    {
    
    }
}
