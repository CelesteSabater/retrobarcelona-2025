using UnityEngine;
using System;
using System.Collections;
#if UNITY_EDITOR
using System.IO;
using UnityEditor;
#endif
using System.Linq;
using System.Collections.Generic;
using retrobarcelona.Utils.Singleton;
using retrobarcelona.Utils.FileManagement;
using retrobarcelona.Config;

namespace retrobarcelona.Systems.AudioSystem
{
    public class AudioSystem : Singleton<AudioSystem>
    {
        [Range(0, 1)]
        [SerializeField] private float _musicVolume = 1;
        [Range(0, 1)]
        [SerializeField] private float _sfxVolume = 1;
        [SerializeField] private String _startingMusic;
        [SerializeField] private Sound[] _musicSounds, _sfxSounds, _npcSounds;
        [SerializeField] private AudioSource _musicSource, _sfxSource, _npcSource;

        private Sound _previousMusic, _currentMusic;

        public string GetPreviousMusic()
        {
            if (_previousMusic == null) return "";
            return _previousMusic._name;
        }
        public string GetCurrentMusic()
        {
            if (_currentMusic == null) return "";
            return _currentMusic._name;
        }

        public float GetMusicVolume() => _musicVolume; 
        public float GetSFXVolume() => _sfxVolume;

        private void Update()
        {
            if (_currentMusic != null) CheckIsPlaying();
        }

        private void CheckSources()
        {
            if (_musicSource != null && _sfxSource != null) return;

            _musicSource = GameObject.Find("_musicSource").GetComponent<AudioSource>();
            _sfxSource = GameObject.Find("_sfxSource").GetComponent<AudioSource>();
        }

        public void StartMusic()
        {
            CheckSources();

            #if UNITY_EDITOR
            CheckFiles();
            #endif

            if (_startingMusic != null && _currentMusic == null && _musicSource != null && _startingMusic != "") 
                PlayMusic(_startingMusic);
        }

        public void StopMusic()
        {
            _musicSource.Stop();
            _currentMusic = null;
        }

        public void PlayMusic(string name)
        {
            CheckSources();
            if (_musicSource == null)
                return;

            Sound music = Array.Find(_musicSounds, x => x._name == name);

            if (music == null)
            {
                Debug.LogWarning($"Music: {name} not found!");
                return;
            }

            _currentMusic = music;
            if (_currentMusic._loop) _previousMusic = _currentMusic;

            _musicSource.clip = music._clip;
            _musicSource.volume = music._volume * _musicVolume;
            _musicSource.loop = music._loop;
            _musicSource.Play();
        }

        public void PlaySFX(string name, bool randomPitch = true)
        {
            CheckSources();
            if (_sfxSource == null)
                return;

            Sound sfx = Array.Find(_sfxSounds, x => x._name == name);

            if (sfx == null)
            {
                Debug.LogWarning($"Sound: {name} not found!");
                return;
            }

            _sfxSource.volume = sfx._volume * _sfxVolume;
            _sfxSource.clip = sfx._clip;

            _sfxSource.pitch = 1;
            if (randomPitch)
                _sfxSource.pitch = UnityEngine.Random.Range(0.95f, 1.05f);
                
            _sfxSource.PlayOneShot(sfx._clip);
        }

        public void PlaySFX(string name, Vector3 location, bool randomPitch = true)
        {
            Sound sfx = Array.Find(_sfxSounds, x => x._name == name);

            if (sfx == null)
            {
                Debug.LogWarning($"Sound: {name} not found!");
                return;
            }

            PlayClipAt(sfx._clip, location, sfx._volume * _sfxVolume, randomPitch);
        }

        public void ToggleMusic() => _musicSource.mute = !_musicSource.mute;
        public void ToggleSFX() => _sfxSource.mute = !_sfxSource.mute;
        public void MusicVolume(float volume)
        {
            _musicVolume = volume;
            _musicSource.volume = _musicVolume * _currentMusic._volume;
        }
        public void SFXVolume(float volume)
        {
            _sfxVolume = volume;
            _sfxSource.volume = _sfxVolume;
        }

        private void CheckIsPlaying()
        {
            if (!_musicSource.isPlaying)
            {
                if (_previousMusic == null) return;
                PlayMusic(_previousMusic._name);
            }
        }

        public void PlayClipAt(AudioClip clip, Vector3 pos, float volume, bool randomPitch = true) {
            GameObject tempGO = new GameObject("TempAudioGo");
            tempGO.transform.position = pos;
            AudioSource aSource = tempGO.AddComponent<AudioSource>();

            aSource.clip = clip;

            aSource.pitch = 1;
            if (randomPitch)
                aSource.pitch = UnityEngine.Random.Range(0.95f, 1.05f);

            aSource.volume = volume;    
            aSource.Play();
            Destroy(tempGO, clip.length);
        }

        public void PlayMusicWithDelay(string name, float delay)
        {
            StartCoroutine(MusicWithDelay(name, delay));
        }

        public void PlaySFXWithDelay(string name, float delay)
        {
            StartCoroutine(SFXWithDelay(name, delay));
        }
            
        public void PlaySFXWithDelay(string name, float delay, Vector3 location)
        {
            StartCoroutine(SFXWithDelay(name, delay, location));
        }

        public float GetCurrentClipLenght(AudioSource source)
        {
            if (source.clip == null) return 0;
            return source.clip.length;
        }
        public bool GetIsPlaying(AudioSource source)
        {
            if (source == null) return false;
            return source.isPlaying;
        }

        IEnumerator MusicWithDelay(string name, float delay)
        {
            yield return new WaitForSeconds(delay);
            PlayMusic(name);
        }
        IEnumerator SFXWithDelay(string name, float delay)
        {
            yield return new WaitForSeconds(delay);
            PlaySFX(name);
        }

        IEnumerator SFXWithDelay(string name, float delay, Vector3 location)
        {
            yield return new WaitForSeconds(delay);
            PlaySFX(name, location);
        }

        public void SpeakWord()
        {
            _npcSource.Stop();
            int i = UnityEngine.Random.Range(0, _npcSounds.Length - 1);

            PlayClipAt(_npcSounds[i]._clip, Vector3.zero, _npcSounds[i]._volume);
        }

        #if UNITY_EDITOR
        private void CheckFiles()
        {
            bool DONT_LOOP = false;
            bool LOOP = true;

            List<string> musicFiles = FileLoader.GetFilesInDirectory(Directories.MUSIC_DIRECTORY)
                                    .Where(file => file.ToLower().EndsWith("mp3") || file.ToLower().EndsWith("wav"))
                                    .ToList();

            List<string> sfxFiles = FileLoader.GetFilesInDirectory(Directories.SOUND_DIRECTORY)
                            .Where(file => file.ToLower().EndsWith("mp3") || file.ToLower().EndsWith("wav"))
                            .ToList();

            List<string> npcSoundsFiles = FileLoader.GetFilesInDirectory(Directories.NPC_SOUNDS_DIRECTORY)
                            .Where(file => file.ToLower().EndsWith("mp3") || file.ToLower().EndsWith("wav"))
                            .ToList();               

            foreach (string filePath in musicFiles)
                AddFileToList(filePath, ref _musicSounds, LOOP);
            
            foreach (string filePath in sfxFiles)
                AddFileToList(filePath, ref _sfxSounds, DONT_LOOP);

            foreach (string filePath in npcSoundsFiles)
                AddFileToList(filePath, ref _npcSounds, DONT_LOOP);
        }

        private void AddFileToList(string filePath,ref Sound[] list, bool loop)
        {
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
            
            Sound oldSound = Array.Find(list, x => x._name == fileNameWithoutExtension);

            if (oldSound != null)
                return;

            AudioClip clip = LoadAudioClip(filePath);

            if (clip == null)
                return;
            
            Sound newSound = new Sound()
            {
                _name = fileNameWithoutExtension,
                _clip = clip,
                _volume = 1.0f, 
                _loop = loop 
            };

            list = list.Concat(new Sound[] { newSound }).ToArray();
            SavePrefab();
        }

        private AudioClip LoadAudioClip(string path) => UnityEditor.AssetDatabase.LoadAssetAtPath<AudioClip>(path);

        private void SavePrefab()
        {
            string prefabPath = GetPrefabPath();
            GameObject existingPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            
            if (existingPrefab != null)
                PrefabUtility.SaveAsPrefabAsset(gameObject, prefabPath);
            else
                PrefabUtility.SaveAsPrefabAssetAndConnect(gameObject, prefabPath, InteractionMode.UserAction);
        }

        private string GetPrefabPath() => Directories.SYSTEM_PREFABS_DIRECTORY+"/AudioSystem.prefab";
        #endif
    }
}