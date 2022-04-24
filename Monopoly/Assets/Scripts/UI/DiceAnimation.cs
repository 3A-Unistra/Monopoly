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

        public Sprite[] DiceSides;

        private Image Rend1;
        private Image Rend2;
        private bool AnimationStarted = false;

        private float AnimationTime;

        private static readonly float EXPIRE_TIME = 3.0f;

        void Start()
        {
            Rend1 = Dice1.GetComponent<Image>();
            Rend2 = Dice2.GetComponent<Image>();
            Dice1.SetActive(false);
            Dice2.SetActive(false);
        }
        
        void Update()
        {
            if (AnimationStarted)
            {
                AnimationTime += Time.deltaTime;
                if (AnimationTime >= 0.05f)
                {
                    int dice1 = Random.Range(0, DiceSides.Length);
                    int dice2 = Random.Range(0, DiceSides.Length);
                    Rend1.sprite = DiceSides[dice1];
                    Rend2.sprite = DiceSides[dice2];
                    AnimationTime = 0.0f;
                }
            }
        }

        public void RollDice()
        {
            AnimationTime = 0;
            AnimationStarted = true;
            Dice1.SetActive(true);
            Dice2.SetActive(true);
        }

        public void HideDice()
        {
            AnimationStarted = false;
            Dice1.SetActive(false);
            Dice2.SetActive(false);
        }

        public void RevealDice(int dice1, int dice2)
        {
            StartCoroutine(RevealDiceEnumrator(dice1, dice2));
        }

        private IEnumerator RevealDiceEnumrator(int dice1, int dice2)
        {
            AnimationStarted = false;
            Rend1.sprite = DiceSides[dice1 - 1];
            Rend2.sprite = DiceSides[dice2 - 1];
            yield return new WaitForSeconds(EXPIRE_TIME);
            Dice1.SetActive(false);
            Dice2.SetActive(false);
        }

    }

}

