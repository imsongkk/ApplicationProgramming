using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    Animator animator;

    public int hp = 100;
    public bool died = false;

    PlayerController player;

    void Start()
    {
        player = GameManager.InGameScene.player;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        TryMove();
    }

    private void TryMove()
    {
        var distance = player.transform.position - transform.position;
        if (distance.magnitude >= 50f)
            return;
        var move = (player.transform.position - transform.position).normalized;
        move = new Vector3(move.x, 0, move.z);

        var speed = 3f;
        transform.position += move * Time.deltaTime * speed;


        /*
        var move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if (move.magnitude == 0) // ¿Œ«≤¿Ã æ¯¿Ω
        {
            animator.SetBool("Running", false);
            return;
        }
        animator.SetBool("Running", true);

        var newMove = new Vector3(transform.TransformDirection(move).x, 0, transform.TransformDirection(move).z);
        float speed = 5f;
        if (Input.GetKey(KeyCode.LeftShift))
            speed = 10f;
        transform.position += newMove * Time.deltaTime * speed;
        */
    }

    public void OnDamage(int damage)
    {
        hp -= damage;
        if (hp <= 0 && !died)
        {
            OnDie();
            return;
        }
    }

    private void OnDie()
    {
        died = true;
        GameManager.InGameScene.UI.score += 1;
        animator.SetTrigger("Die");
    }
}
