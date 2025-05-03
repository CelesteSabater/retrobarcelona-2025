using System;
using UnityEngine;

namespace retrobarcelona.Managers
{
    public class GameEvents : MonoBehaviour
    {
        public static GameEvents current;

        private void Awake() => current = this;

        #region DIALOGUE
        public event Action<bool> onSetDialogue;
        public void SetDialogue(bool value) => onSetDialogue?.Invoke(value);
        #endregion

        #region SONGS
        public event Action<TextAsset> onStartSong;
        public void StartSong(TextAsset song) => onStartSong?.Invoke(song);
        public event Action onSongFinished;
        public void SongFinished() => onSongFinished?.Invoke();
        #endregion

        #region COMBOS
        public event Action onLowCombo;
        public void LowCombo() => onLowCombo?.Invoke();
        public event Action onMidCombo;
        public void MidCombo() => onMidCombo?.Invoke();
        
        public event Action onHighCombo;
        public void HighCombo() => onHighCombo?.Invoke();
        #endregion
    }
}