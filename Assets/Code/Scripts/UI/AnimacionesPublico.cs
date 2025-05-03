using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimacionesPublico : MonoBehaviour
{
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        this.animator = GetComponent<Animator>();
        animator.SetInteger("Int", Random.Range(0, 3));    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
