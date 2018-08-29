using System.Collections.Generic;
using OpenAudio.Database;
using UnityEngine;

namespace OpenAudio
{

    public class AudioManager : MonoBehaviour
    {
        #region Properties
        private static AudioManager _instance;
        public static AudioManager instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = GameObject.FindObjectOfType<AudioManager>();
                    if (_instance == null)
                    {
                        var t = new GameObject("audio-manager");
                        _instance = t.AddComponent<AudioManager>();
                    }
                }
                return _instance;
            }
        }

        #endregion

        #region Fields

        // List<AudioDatabaseItem> audioItemList;
        List<Audio> audioSourcePool;
        [SerializeField] List<RawAudioDatabase> allDataBases;

        /// <summary>
        /// List of all manual audio clips directly passed to audioService
        /// </summary>
        private List<AudioDatabaseItem> audioClipDatabaseList;

        // List<AudioClip> audioClips;

        #endregion

        #region Methods

        void Awake()
        {
            GameObject.DontDestroyOnLoad(this);
            LoadService();
        }

        private void LoadService()
        {
            if (GameObject.FindObjectOfType<AudioListener>() == null)
            {
                gameObject.AddComponent<AudioListener>();
            }

            allDataBases = new List<RawAudioDatabase>();
            audioSourcePool = new List<Audio>();
            audioClipDatabaseList = new List<AudioDatabaseItem>();
            // audioClips = new List<AudioClip>();

            AudioDatabase[] rawcollections = Resources.LoadAll<AudioDatabase>("");
            foreach (var rawDatabase in rawcollections)
            {
                var _db = new RawAudioDatabase(rawDatabase);
                allDataBases.Add(_db);
            }
            // foreach (var audioDB in allDataBases)
            // {
            //     audioItemList.AddRange(audioDB.audioDBItems);
            // }
        }

        //  Playe audio clip directly returns an ID for this audioClip so you can have access to that
        public void Play(AudioClip clip, bool loop = false)
        {
            var audioDatabaseItem = audioClipDatabaseList.Find(x => x.audioName == clip.name);
            if (audioDatabaseItem == null)
            {
                // Play audioclip and add to dictionary
                audioDatabaseItem = new AudioDatabaseItem();
                audioDatabaseItem.audioName = clip.name;
                audioDatabaseItem.audioClip = clip;
                audioClipDatabaseList.Add(audioDatabaseItem);
            }
            Audio freeAudio = GetFreeAudio();
            if (freeAudio == null)
            {
                freeAudio = new Audio(audioDatabaseItem);
                audioSourcePool.Add(freeAudio);
            }
            freeAudio.Init(audioDatabaseItem).Play().Loop(loop);
        }

        private Audio GetFreeAudio()
        {
            Audio audio = null;
            foreach (Audio item in audioSourcePool)
            {
                if (!item.isplaying())
                {
                    audio = item;
                }
            }
            return audio;
        }

        public void Play(AudioType audioType, bool loop = false)
        {
            Audio audio = null;
            foreach (Audio item in audioSourcePool)
            {
                if (!item.isplaying())
                {
                    if (item.audioDatabaseItem.type == audioType)
                    {
                        item.Play().Loop(loop);
                        return;
                    }
                    audio = item;
                }
            }

            // Find clip database item and set clip if it is not set;
            AudioDatabaseItem audioDatabaseItem = null;
            RawAudioDatabase db = null;
            foreach (var database in allDataBases)
            {
                var tmp = database.audioDBItems.Find(x => x.type == audioType);
                if (tmp != null)
                {
                    audioDatabaseItem = tmp;
                    db = database;
                    break;
                }
            }

            if (audioDatabaseItem != null && audioDatabaseItem.audioClip == null)
            {
                string clipPath = db.ResourcePath + "/" + audioDatabaseItem.audioName;
                var clip = Resources.Load<AudioClip>(clipPath);
                if (clip != null) audioDatabaseItem.audioClip = clip;
                else Debug.Log("could not find audioClip at: " + clipPath);
            }

            if (audio == null)
            {
                audio = new Audio(audioDatabaseItem);
                audioSourcePool.Add(audio);
            }
            audio.Init(audioDatabaseItem);
            audio.Play().Loop(loop);
        }

        // private AudioDatabaseItem GetAudioDatabaseItem(AudioType audioType)
        // {
        //     AudioDatabaseItem t = null;
        //     foreach (var db in allDataBases)
        //     {
        //         t = db.audioDBItems.Find(x => x.type == audioType);
        //     }
        //     return t;
        // }

        public void StopAll()
        {
            foreach (var audio in audioSourcePool)
            {
                if (audio.isplaying()) audio.Stop();
            }
        }

        public void Stop(AudioType audioType)
        {
            foreach (var audio in audioSourcePool)
            {
                if (audio.audioDatabaseItem.type == audioType)
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
                if (audio.audioDatabaseItem.type == audioType)
                {
                    audio.Pause();
                    break;
                }
            }
        }

        public void SetVolume(AudioType audioType, float vol)
        {
            foreach (var _audio in audioSourcePool)
            {
                if (_audio.audioDatabaseItem.type == audioType)
                {
                    _audio.SetVolume(vol);
                }
            }
        }
        public void SetVolume(float vol)
        {
            foreach (var _audio in audioSourcePool)
            {
                if (_audio.isplaying())
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
        #endregion
    }
}