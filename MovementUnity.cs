using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Monopoly.MovementTest
{    
 public class MovementUnity : MonoBehaviour 
 {
    public float Movespeed=2f;
    void Start ()
    {
    }
    void Update()
    {
        if(transform.position.z<8.0)
        {
            transform.Translate(Vector3.forward*Time.deltaTime*Movespeed);
        }
    }
 }
}