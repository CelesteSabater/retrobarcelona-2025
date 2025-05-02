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
            float time = 1.0f;
            if (noteMap == null)
            {
                Debug.LogWarning("Song not found!");
                return;
            }

            SongData data = JsonUtility.FromJson<SongData>(noteMap.text);
            AudioSystem.Instance.PlayMusic(data.songName);
            foreach (NoteData note in data.notes) {
                await SpawnNote(note.lane, note.time);
            }
            
            await UniTask.Delay(TimeSpan.FromSeconds(time));
            GameEvents.current.SetDialogue(true);
        }
     
        private async UniTask SpawnNote(int lane, float time) {
            await UniTask.Delay(TimeSpan.FromSeconds(time));
            GameObject note = Instantiate(_notePrefabs[lane], _lanes[lane].position, Quaternion.identity).gameObject;
            note.transform.SetParent(_lanes[lane]);
            note.transform.localPosition = Vector3.zero;
        }
    }
}