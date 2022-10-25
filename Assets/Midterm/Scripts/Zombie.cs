using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    Animator animator;

    public int hp = 100;
    public bool died = false;
    public bool playerDetected = false;

    PlayerController player;

    void Start()
    {
        player = GameManager.InGameScene.player;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        var distance = player.transform.position - transform.position;
        if (distance.magnitude >= 50f)
            playerDetected = false;
        else
            playerDetected = true;

        TryMove();
    }

    private void TryMove()
    {
        if (died) return;

        if (playerDetected)
            animator.SetBool("Running", true);
        else
            animator.SetBool("Running", false);
        
        var distance = player.transform.position - transform.position;
        var move = distance.normalized;

        transform.forward = move; // 플레이어 바라보도록 설정

        move = new Vector3(move.x, 0, move.z);

        var speed = 3f;
        transform.position += move * Time.deltaTime * speed; // 좀비 이동
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
