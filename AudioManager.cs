using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace OpenAudio
{

    public class AudioManager<TEnum> where TEnum : struct
    {
        private List<Audio> audioSourcePool;
        /// List of all manual audio clips
        private List<AudioClip> audioClipPool;
        private AudioListener audioListener;
        private static GameObject audioManagerGameObject;
        private string resourcePath;

        public AudioManager(string resourcePath)
        {
            this.resourcePath = resourcePath;
            audioListener = GameObject.FindObjectOfType<AudioListener>();
            audioManagerGameObject = new GameObject("audio-manager");
            if (audioListener == null) audioListener = audioManagerGameObject.AddComponent<AudioListener>();
            GameObject.DontDestroyOnLoad(audioManagerGameObject);

            audioSourcePool = new List<Audio>();
            audioClipPool = new List<AudioClip>();
        }

        private Audio GetFreeAudio()
        {
            Audio audio = null;
            foreach (Audio _audio in audioSourcePool)
            {
                if (!_audio.isplaying && !_audio.isPaused) audio = _audio;
            }
            if (audio != null) return audio;
            audio = new Audio(audioManagerGameObject.transform);
            audioSourcePool.Add(audio);
            return audio;
        }

        private Audio GetAudioByName(string type)
        {
            foreach (Audio _audio in audioSourcePool)
            {
                if (_audio.audioClip.name == type) return _audio;
            }
            return null;
        }

        private AudioClip LoadAudioClip(string clipName)
        {
            var audioClip = audioClipPool.Find(x => x.name == clipName);
            if (audioClip != null) return audioClip;
            // load the audioclip
            string path = resourcePath + "/" + clipName;
            AudioClip audioClip1 = Resources.Load<AudioClip>(path);
            Assert.IsNotNull(audioClip1, "could not find audio clip at " + path);
            return audioClip1;
        }

        //  Playe audio clip directly returns an ID for this audioClip so you can have access to that
        public void Play(AudioClip clip, bool loop = false)
        {
            // if we have an audio with this clip, just play it
            Audio _audio = GetAudioByName(clip.name);
            if (_audio != null)
            {
                _audio.Play().Loop(loop);
                return;
            }
            // if we dont have an audio with this clip, create a new free audio and set this clip and play it
            GetFreeAudio().SetClip(clip).Play().Loop(loop);
            if (audioClipPool.Contains(clip)) audioClipPool.Add(clip);
        }

        public void Play(TEnum audioType, bool loop = false)
        {
            Audio _audio = GetAudioByName(audioType.ToString());
            if (_audio != null)
            {
                _audio.Play().Loop(loop);
                return;
            }
            var audioClip = LoadAudioClip(audioType.ToString());
            Play(audioClip, loop);
        }

        public void StopAll()
        {
            foreach (var audio in audioSourcePool)
            {
                if (audio.isplaying) audio.Stop();
            }
        }

        public void Stop(AudioType audioType)
        {
            foreach (var audio in audioSourcePool)
            {
                if (audio.audioClip.name == audioType.ToString())
                {
                    audio.Stop();
                    break;
                }
            }
        }
        public void Pause(AudioType audioType)
        {
            foreach (var audio in audioSourcePool)
            {
                if (audio.audioClip.name == audioType.ToString())
                {
                    audio.Pause();
                    break;
                }
            }
        }

        public void SetVolume(AudioType audioType, float vol)
        {
            foreach (var audio in audioSourcePool)
            {
                if (audio.audioClip.name == audioType.ToString())
                {
                    audio.SetVolume(vol);
                }
            }
        }
        public void SetVolume(float vol)
        {
            foreach (var _audio in audioSourcePool)
            {
                if (_audio.isplaying)
                {
                    _audio.SetVolume(vol);
                }
            }
        }

        public void Mute()
        {
            foreach (var _audio in audioSourcePool)
            {
                _audio.Mute();
            }
        }

        public void UnMute()
        {
            foreach (var _audio in audioSourcePool)
            {
                _audio.UnMute();
            }
        }
    }
}