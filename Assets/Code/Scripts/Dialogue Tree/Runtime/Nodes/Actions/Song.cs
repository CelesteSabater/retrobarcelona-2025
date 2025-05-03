using System.Collections.Generic;
using UnityEngine;
using retrobarcelona.DialogueTree.Runtime;
using retrobarcelona.MusicSystem;
using retrobarcelona.Managers;
using retrobarcelona.UI;

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
            float scoreForNote = _maxScore / data.notes.Length;

            GameEvents.current.SetDialogue(false);
            GameEvents.current.StartSong(_jsonFile);
            SistemaDePuntos.Instance.InitializeSong(scoreForNote, _karma);
        }

        protected override void EndAction() { }
    }
}