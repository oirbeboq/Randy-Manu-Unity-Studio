using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStates : MonoBehaviour
{
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        bool runPressed = Input.GetKey("left shift");
        bool fowardPressed = Input.GetKey("w");
        bool jumpPressed = Input.GetKey("space");
        bool isWalking = animator.GetBool("isWalking");
        bool isRunning = animator.GetBool("isRunning");

        if (!isWalking && fowardPressed)
        {
            animator.SetBool("isWalking", true);
        }

        if(isWalking && !fowardPressed) 
        {
            animator.SetBool("isWalking", false);
        }

        if (!isRunning && (runPressed && fowardPressed))
        {
            animator.SetBool("isRunning", true);
        }
        
        if (isRunning && !fowardPressed || !runPressed)
        {
            animator.SetBool("isRunning", false);
        }

    }

}
