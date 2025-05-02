using System;
using UnityEngine;
using retrobarcelona.Utils.Singleton;
using retrobarcelona.Managers;
using retrobarcelona.Systems.AudioSystem;
using UnityEngine.SocialPlatforms.Impl;
using Cysharp.Threading.Tasks;

namespace retrobarcelona.MusicSystem
{
    public class NoteSpawner : Singleton<NoteSpawner> 
    {
        [SerializeField] private Note[] _notePrefabs;
        [SerializeField] private Transform[] _lanes;

        private void Start()
        {
            GameEvents.current.onStartSong += StartSong;
        }

        async void StartSong(TextAsset noteMap) {
            if (noteMap == null)
            {
                Debug.LogWarning("Song not found!");
                return;
            }

            SongData data = JsonUtility.FromJson<SongData>(noteMap.text);
            foreach (NoteData note in data.notes) {
                await SpawnNote(note.lane, note.time);
            }
            AudioSystem.Instance.PlayMusic(data.songName);
        }
     
        private async UniTask SpawnNote(int lane, float time) {
            await UniTask.Delay(TimeSpan.FromSeconds(time));
            Instantiate(_notePrefabs[lane], _lanes[lane].position, Quaternion.identity);
        }
    }
}