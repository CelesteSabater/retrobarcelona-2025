using retrobarcelona.Managers;
using UnityEngine;

public class LanePartitura : MonoBehaviour
{
    void Start()
    {
        GameEvents.current.onSetDialogue += ChangeAlpha;
    }

    private void OnDestroy()
    {
        GameEvents.current.onSetDialogue -= ChangeAlpha;
    }

    private void ChangeAlpha(bool isDialogue)
    {
        UnityEngine.UI.Image image = GetComponent<UnityEngine.UI.Image>();
        if (image == null) return;

        Color c = image.color;
        if (isDialogue) c.a = 0; else c.a = 1;
        image.color = c;
    }
}
