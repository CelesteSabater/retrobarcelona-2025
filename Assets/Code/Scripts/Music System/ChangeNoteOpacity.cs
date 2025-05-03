using UnityEngine;
using UnityEngine.UI;

namespace retrobarcelona.MusicSystem
{
    public class ChangeNoteOpacity : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            Note note = other.GetComponent<Note>();
            if (note == null)
                return;
            
            RawImage image = GetComponentInChildren<RawImage>();
            if (image == null) 
                return;

            Color currColor = image.color;
            currColor.a = 0.5f;
            image.color = currColor;
        }
    }
}
