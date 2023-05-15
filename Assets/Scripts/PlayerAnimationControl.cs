using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationControl : MonoBehaviour
{
    Animator animator;
    void Awake()
    {
        animator = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        bool run = GetComponent<RightLeft>().move.x != 0;
        //bool dash = GetComponent<Dash>().isDashing;
        bool jump = !GetComponent<Jump>().grounded;
        if(!jump)
            animator.SetBool("Run", run);
        animator.SetBool("Jump", jump);

        //animator.SetBool("Dash",dash);
    }
}
