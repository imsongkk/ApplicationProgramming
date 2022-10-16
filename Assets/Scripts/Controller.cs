using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    Animator animator;
    

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.RightArrow) 
            || Input.GetKey(KeyCode.LeftArrow)
            || Input.GetKey(KeyCode.UpArrow)
            || Input.GetKey(KeyCode.DownArrow))
		{
            animator.SetBool("Running", true);
        }
        else if(Input.GetKeyDown(KeyCode.LeftControl))
		{
            var current = animator.GetBool("Sitting");
            animator.SetBool("Sitting", !current);
		}
        else if(Input.GetKeyDown(KeyCode.Space))
		{
            animator.SetBool("Jumping", true);
		}
        else
		{
            animator.SetBool("Jumping", false);
            animator.SetBool("Running", false);
            animator.SetBool("Idle", true);
        }
        if (Input.GetMouseButtonDown(0))
		{
            animator.SetTrigger("Shoot");
		}
    }
}

