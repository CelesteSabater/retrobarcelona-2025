using UnityEngine;
using UnityEngine.UI;

namespace retrobarcelona.MusicSystem
{
    public class ChangeNoteOpacity : MonoBehaviour
    {
        private float fadeDuration = 0.5f; // Time in seconds for the fade
        private void OnTriggerEnter2D(Collider2D other)
        {
            Note note = other.GetComponent<Note>();
            if (note == null)
                return;

            RawImage image = other.GetComponentInChildren<RawImage>();
            if (image == null) 
                return;

            Color finalColor = image.color;
            finalColor = Color.red;
            image.color = finalColor;

            StartCoroutine(FadeOut(image));
        }

        private System.Collections.IEnumerator FadeOut(RawImage image)
        {
            float startAlpha = image.color.a;
            float targetAlpha = 0f;
            float elapsedTime = 0f;
            
            while (elapsedTime < fadeDuration)
            {
                // Calculate new alpha value
                float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
                
                // Apply the new alpha
                Color newColor = image.color;
                newColor.a = newAlpha;
                image.color = newColor;
                
                // Increment time
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            
            // Ensure final alpha is set exactly
            Color finalColor = image.color;
            finalColor.a = targetAlpha;
            image.color = finalColor;
        }
    }
}
