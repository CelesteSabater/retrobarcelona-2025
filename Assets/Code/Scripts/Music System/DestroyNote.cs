using System.Collections;
using System.Collections.Generic;
using retrobarcelona.Systems.AudioSystem;
using UnityEngine;
using retrobarcelona.UI;

namespace retrobarcelona.MusicSystem
{
    public class DestroyNote : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            Note note = other.GetComponent<Note>();
            if (note == null)
                return;
            
            NoteHitTiming timing = NoteHitTiming.Incorrect;
            SistemaDePuntos.Instance.CalcularPuntos(timing);
            AudioSystem.Instance.PlaySFX("BrokenGuitarSound", Vector3.zero);
            Destroy(other.gameObject);
            //GameManager.Instance.MissNote();
        }
    }
}
