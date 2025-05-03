using System.Collections;
using System.Collections.Generic;
using retrobarcelona.Managers;
using UnityEngine;

public class CheerAnimation : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        GameEvents.current.onLowCombo += () => animator.Play("LowCombo");
        GameEvents.current.onMidCombo += () => animator.Play("MidCombo");
        GameEvents.current.onHighCombo += () => animator.Play("HighCombo");
    }

    void OnDestroy()
    {
        GameEvents.current.onLowCombo -= () => animator.Play("LowCombo");
        GameEvents.current.onMidCombo -= () => animator.Play("MidCombo");
        GameEvents.current.onHighCombo -= () => animator.Play("HighCombo");
    }
}
