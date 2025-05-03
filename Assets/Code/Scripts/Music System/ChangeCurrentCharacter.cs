using retrobarcelona.Managers;
using UnityEngine;

public class ChangeCurrentCharacter : MonoBehaviour
{
    public bool _isMainCharacter;

    void Start()
    {
        GameEvents.current.onSetDialogue += ChangeActive;
    }

    private void OnDestroy()
    {
        GameEvents.current.onSetDialogue -= ChangeActive;
    }

    private void ChangeActive(bool isDialogue)
    {
        UnityEngine.UI.Image image = GetComponent<UnityEngine.UI.Image>();
        if (image == null) return;

        Color c = image.color;
        if ((isDialogue && _isMainCharacter) || (!isDialogue && !_isMainCharacter)) c.a = 0; else c.a = 1;
        image.color = c;
    }
}
