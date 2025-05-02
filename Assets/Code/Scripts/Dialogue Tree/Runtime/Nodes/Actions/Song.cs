using System.Collections.Generic;
using UnityEngine;
using retrobarcelona.DialogueTree.Runtime;
using retrobarcelona.MusicSystem;
using retrobarcelona.Managers;

namespace DialogueTree.Runtime
{
    public class Song : ActionNode
    {
        [SerializeField] private TextAsset _jsonFile;
        [SerializeField] private int _maxScore;
        [SerializeField] private int _karma;

        protected override void StartAction()
        {
            SongData data = JsonUtility.FromJson<SongData>(_jsonFile.text);
            float numberOfNotes = _maxScore / data.notes.Length;

            Debug.Log($"Number of notes: {numberOfNotes}");

            GameEvents.current.SetDialogue(false);
            GameEvents.current.StartSong(_jsonFile);
        }

        protected override void EndAction() { }
    }
}