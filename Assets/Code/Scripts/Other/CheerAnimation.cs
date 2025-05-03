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
        GameEvents.current.onLowCombo += LowCombo;
        GameEvents.current.onMidCombo += MidCombo;
        GameEvents.current.onHighCombo += HighCombo; 
    }

    void OnDestroy()
    {
        GameEvents.current.onLowCombo -= LowCombo;
        GameEvents.current.onMidCombo -= MidCombo;
        GameEvents.current.onHighCombo -= HighCombo;
    }

    void LowCombo()
    {
        animator.SetBool("LowCombo", true); 
        animator.SetBool("MidCombo", false); 
        animator.SetBool("HighCombo", false);
    }

    void MidCombo()
    {
        animator.SetBool("LowCombo", false); 
        animator.SetBool("MidCombo", true); 
        animator.SetBool("HighCombo", false);
    }

    void HighCombo()
    {
        animator.SetBool("LowCombo", false);
        animator.SetBool("MidCombo", false);
        animator.SetBool("HighCombo", true);
    }
}
