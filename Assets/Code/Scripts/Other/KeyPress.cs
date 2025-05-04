using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceBar : MonoBehaviour
{
    private Animator animator;
    public KeyCode keyToPress;

    void Start()
    {
        animator = GetComponent<Animator>();
    }
    void Update() 
    { 
        if (animator == null) return; 
        if (Input.GetKeyDown(keyToPress))  
        { 
            animator.Play("Pulsar"); 
        }
        if (Input.GetKeyUp(keyToPress))  
        { 
            animator.Play("Despulsar"); 
        }
    }
}
