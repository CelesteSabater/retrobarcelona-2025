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
        GameEvents.current.onLowCombo += () => animator.Play("LowCombo"); animator.SetBool("LowCombo", true); animator.SetBool("MidCombo", false); animator.SetBool("HighCombo", false);
        GameEvents.current.onMidCombo += () => animator.Play("MidCombo"); animator.SetBool("LowCombo", false); animator.SetBool("MidCombo", true); animator.SetBool("HighCombo", false);
        GameEvents.current.onHighCombo += () => animator.Play("HighCombo"); animator.SetBool("LowCombo", false); animator.SetBool("MidCombo", false); animator.SetBool("HighCombo", true);
    }

    void OnDestroy()
    {
        GameEvents.current.onLowCombo -= () => animator.Play("LowCombo");
        GameEvents.current.onMidCombo -= () => animator.Play("MidCombo");
        GameEvents.current.onHighCombo -= () => animator.Play("HighCombo");
    }
}
