using System;
using UnityEngine;

namespace retrobarcelona.MusicSystem
{
    public class Note : MonoBehaviour {
        public float _speed = 5f;
        private int _difficultyModifier;

        void Start()
        {
            _difficultyModifier = 1;
        }

        void Update() {
            transform.Translate(_speed * _difficultyModifier * Time.deltaTime * -Vector3.right);
        }
    }
}