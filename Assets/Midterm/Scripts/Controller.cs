using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] Camera fpsCamera;
    [SerializeField] Camera tpsCamera;
    [SerializeField] GameObject player;



    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        TryMove();
        TryLook();
        /*
        if (Input.GetKey(KeyCode.RightArrow) 
            || Input.GetKey(KeyCode.LeftArrow)
            || Input.GetKey(KeyCode.UpArrow)
            || Input.GetKey(KeyCode.DownArrow))
		{
            animator.SetBool("Running", true);
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
        */
        if (Input.GetMouseButtonDown(0))
		{
            animator.SetTrigger("Shoot");
		}
    }

    private void TryLook()
    {
        /* FPS(1ÀÎÄª) */



        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * 30f;
        Vector3 camAngle = tpsCamera.transform.rotation.eulerAngles;

        float x = camAngle.x - mouseDelta.y;
        if (x < 180f)
            x = Mathf.Clamp(x, -1f, 70f);
        else
            x = Mathf.Clamp(x, 335f, 361f);


        Debug.Log(Input.mousePosition);

        //tpsCamera.transform.rotation = Quaternion.Euler(x, camAngle.y + mouseDelta.x, camAngle.z);
        //player.transform.rotation = Quaternion.Euler(0, camAngle.y + mouseDelta.x, camAngle.z);
        player.transform.Rotate(0, mouseDelta.x * Time.deltaTime, 0);
    }

    private void TryMove()
    {
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (input.magnitude == 0)
        {
            animator.SetBool("Running", false);
            return;
        }

        animator.SetBool("Running", true);

        Vector3 lookForward = new Vector3(tpsCamera.transform.forward.x, 0f, tpsCamera.transform.forward.z).normalized;
        Vector3 lookRight = new Vector3(tpsCamera.transform.right.x, 0f, tpsCamera.transform.right.z).normalized;
        Vector3 moveDir = lookForward * input.y + lookRight * input.x;

        //transform.forward = lookForward;
        Debug.Log(tpsCamera.transform.forward);
        //player.transform.forward = tpsCamera.transform.forward;
        player.transform.forward = lookForward;

        //player.transform.forward = input.x * Vector3.right *lookForward.x + input.y * Vector3.forward *lookForward.z;
        transform.position += moveDir * Time.deltaTime * 3f;
    }
}

