/*
 * SoundHandler.cs
 * Sound and music player.
 * 
 * Date created : 03/05/2022
 * Author       : Finn Rayment <rayment@etu.unistra.fr>
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

using Monopoly.Util;

namespace Monopoly.Runtime
{

    public class SoundHandler : MonoBehaviour
    {

        public AudioSource soundPlayer;
        public AudioSource musicPlayer;

        public AudioMixer mixer;

        public AudioClip soundPieceMove;
        public AudioClip soundDiceShake;
        public AudioClip soundDiceRoll;
        public AudioClip soundAuctionBid;
        public AudioClip soundPropertyBuy;
        public AudioClip soundBalanceUpdate;
        public AudioClip soundHouseBuy;
        public AudioClip soundHouseSell;
        public AudioClip soundMessageBlip;
        public AudioClip soundButtonBlip;

        void Start()
        {
            SetSoundLevel(PreferenceApply.Sound);
            SetMusicLevel(PreferenceApply.Music);
        }

        private static float ToDecibels(float level)
        {
            // need to clamp it to stop it turning right off and overflowing
            return Mathf.Log10(Mathf.Clamp(level, 0.0001f, 1.0f)) * 20;
        }

        public void SetSoundLevel(float level)
        {
            mixer.SetFloat("Volume_SFX", ToDecibels(level));
        }

        public void SetMusicLevel(float level)
        {
            mixer.SetFloat("Volume_Music", ToDecibels(level));
        }

        public void PlaySound(AudioClip clip)
        {
            soundPlayer.PlayOneShot(clip);
        }

        public void PlayPieceMove()
        {
            PlaySound(soundPieceMove);
        }

        public void PlayDiceShake()
        {
            PlaySound(soundDiceShake);
        }

        public void PlayDiceRoll()
        {
            PlaySound(soundDiceRoll);
        }

        public void PlayAuctionBid()
        {
            PlaySound(soundAuctionBid);
        }

        public void PlayPropertyBuy()
        {
            PlaySound(soundPropertyBuy);
        }

        public void PlayBalanceUpdate()
        {
            PlaySound(soundBalanceUpdate);
        }

        public void PlayHouseBuy()
        {
            PlaySound(soundHouseBuy);
        }

        public void PlayHouseSell()
        {
            PlaySound(soundHouseSell);
        }

        public void PlayMessageBlip()
        {
            PlaySound(soundMessageBlip);
        }

        public void PlayButtonBlip()
        {
            PlaySound(soundButtonBlip);
        }

    }

}
