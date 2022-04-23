/*
 * DiceAnimation.cs
 * Dice animations and stuff.
 * 
 * Date created : 23/04/2022
 * Author       : Rayan MARMAR <rayan.marmar@etu.unistra.fr>
 *              : Finn RAYMENT <rayment@etu.unistra.fr>
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Monopoly.UI
{

    public class DiceAnimation : MonoBehaviour
    {

        public GameObject Dice1;
        public GameObject Dice2;
        public Button RollButton;

        public Sprite[] DiceSides;

        private Image Rend1;
        private Image Rend2;
        private bool AnimationStarted = false;
        private bool PacketReceived = false;
        private float AnimationTime;
        private float DisactivationTimer;
        private int Dice1Results;
        private int Dice2Results;

        void Start()
        {
            RollButton.onClick.AddListener(RollDice);
            Rend1 = Dice1.GetComponent<Image>();
            Rend2 = Dice2.GetComponent<Image>();
            Dice1.SetActive(AnimationStarted);
            Dice2.SetActive(AnimationStarted);
        }
        
        void Update()
        {
            if (AnimationStarted && !PacketReceived)
            {
                AnimationTime += Time.deltaTime;
                if (AnimationTime >= 0.05f)
                {
                    int dice1 = Random.Range(0, DiceSides.Length);
                    int dice2 = Random.Range(0, DiceSides.Length);
                    Rend1.sprite = DiceSides[dice1];
                    Rend2.sprite = DiceSides[dice2];
                    AnimationTime = 0;
                }
            }
            else if (PacketReceived)
            {
                DisactivationTimer += Time.deltaTime;
                if (DisactivationTimer > 1.5f)
                {
                    Dice1.SetActive(false);
                    Dice2.SetActive(false);
                }
            }
        }

        private void RollDice()
        {
            if (!AnimationStarted)
            {
                Dice1Results = Random.Range(0, 5); // we won't need this variable when we receive the packet
                Dice2Results = Random.Range(0, 5); // we won't need this variable when we receive the packet
                AnimationTime = 0;
                AnimationStarted = true;
                PacketReceived = false;
                Dice1.SetActive(AnimationStarted);
                Dice2.SetActive(AnimationStarted);
            }
            // FinalDiceSide() should be called when we receive the result
            // packet from the server not on the second button click
            else
            {
                FinalDiceSide();
            }
        }

        private void FinalDiceSide()
        {
            PacketReceived = true;
            AnimationStarted = false;
            Rend1.sprite = DiceSides[Dice1Results]; 
            Rend2.sprite = DiceSides[Dice2Results];
            DisactivationTimer = 0;
            // DiceResults should be replaced by the dice number sent by the server
        }
    }

}

