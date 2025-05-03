using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceBar : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }
    void Update() 
    { 
        if (Input.GetKeyDown(KeyCode.Space))  
        { 
            animator.Play("Pulsar"); 
        }
        if (Input.GetKeyUp(KeyCode.Space))  
        { 
            animator.Play("Despulsar"); 
        }
    }
}
