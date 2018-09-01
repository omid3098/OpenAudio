using System;
using UnityEngine;

namespace OpenAudio
{
    public class Audio
    //  : IAudio
    {
        AudioSource audioSource;
        GameObject _gameObject;
        public AudioClip audioClip
        {
            get { return audioSource.clip; }
            private set
            {
                audioSource.clip = value;
            }
        }

        public bool isPaused { get; private set; }
        public bool isplaying { get { return audioSource.isPlaying; } }

        public Audio(Transform parent)
        {
            isPaused = false;
            if (_gameObject == null)
            {
                _gameObject = new GameObject("audioSource");
                _gameObject.transform.SetParent(parent, false);
                audioSource = _gameObject.AddComponent<AudioSource>();
            }
        }
        public Audio SetClip(AudioClip clip) { this.audioClip = clip; return this; }

        /// <summary>
        /// Set Audio volume
        /// </summary>
        /// <param name="volume">between 0 and 1</param>
        public Audio SetVolume(float vol)
        {
            audioSource.volume = vol;
            return this;
        }


        public Audio Play()
        {
            isPaused = false;
            audioSource.Play();
            return this;
        }

        public Audio Resume()
        {
            isPaused = false;
            audioSource.UnPause();
            return this;
        }

        public Audio Loop(bool loop)
        {
            audioSource.loop = loop;
            return this;
        }

        public Audio Stop()
        {
            audioSource.Stop();
            return this;
        }


        public Audio SetParent(Transform parent)
        {
            _gameObject.transform.SetParent(parent, false);
            return this;
        }

        public void Pause()
        {
            isPaused = true;
            audioSource.Pause();
        }

        public void Mute()
        {
            audioSource.mute = true;
        }

        public void UnMute()
        {
            audioSource.mute = false;
        }
    }
}