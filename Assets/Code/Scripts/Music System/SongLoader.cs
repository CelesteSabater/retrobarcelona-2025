using UnityEngine;
using System.IO;
using retrobarcelona.Config;
using retrobarcelona.Managers;

namespace retrobarcelona.MusicSystem {
    [System.Serializable]
    public class NoteData {
        public float time;
        public int lane;
        public string text;
    }

    [System.Serializable]
    public class SongData {
        public string songName;
        public NoteData[] notes;
    }

    public class SongLoader : MonoBehaviour {

        public TextAsset test;

        public void StartTest() {
            GameEvents.current.StartSong(test);
        }

        /*
        public void LoadSong(TextAsset jsonFile) {
            if (jsonFile == null)
            {
                Debug.LogWarning($"Song not found!");
                return;
            }

            SongData songData = JsonUtility.FromJson<SongData>(jsonFile.text);
            foreach (NoteData note in songData.notes) {
                Debug.Log($"Nota en carril {note.lane} a los {note.time} segundos");
            }
        }*/
    }
}