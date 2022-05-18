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
        public AudioClip soundRoomJoin;
        public AudioClip soundRoomLeave;
        public AudioClip soundButtonBlip;

        public AudioClip soundMessageBlipAlt;
        public AudioClip soundRoomJoinAlt;
        public AudioClip soundRoomLeaveAlt;
        public AudioClip soundButtonBlipAlt;

        public AudioClip[] musicTracks;
        private int currentMusicTrack = 0;

        void Start()
        {
            SetSoundLevel(PreferenceApply.Sound);
            SetMusicLevel(PreferenceApply.Music);
            soundPlayer.loop = false;
            musicPlayer.loop = false;
        }

        private static float ToDecibels(float level)
        {
            // need to clamp it to stop it turning right off and overflowing
            return Mathf.Log10(Mathf.Clamp(level, 0.0001f, 1.0f)) * 20;
        }

        public void SwapAltSound()
        {
            soundMessageBlip = soundMessageBlipAlt;
            soundRoomJoin = soundRoomJoinAlt;
            soundRoomLeave = soundRoomLeaveAlt;
            soundButtonBlip = soundButtonBlipAlt;
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
            if (clip != null)
                soundPlayer.PlayOneShot(clip);
        }

        public void PlayRoomJoin()
        {
            PlaySound(soundRoomJoin);
        }

        public void PlayRoomLeave()
        {
            PlaySound(soundRoomLeave);
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

        void Update()
        {
            if (!musicPlayer.isPlaying)
            {
                AudioClip clip = musicTracks[currentMusicTrack++];
                if (clip != null)
                {
                    musicPlayer.clip = clip;
                    musicPlayer.Play();
                }
                currentMusicTrack =
                    Mathf.Clamp(currentMusicTrack, 0, musicTracks.Length - 1);
            }
        }

    }

}
