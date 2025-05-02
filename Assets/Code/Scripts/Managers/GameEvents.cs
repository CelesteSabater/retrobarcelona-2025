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

        #region DIALOGUE
        public event Action<TextAsset> onStartSong;
        public void StartSong(TextAsset song) => onStartSong?.Invoke(song);
        #endregion
    }
}