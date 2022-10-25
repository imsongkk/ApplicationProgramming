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
    public bool attacking = false;
    public float lastAttackTime = 0f;

    PlayerController player;

    void Start()
    {
        player = GameManager.InGameScene.player;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!player.isPlaying) return;

        var distance = player.transform.position - transform.position;
        if (distance.magnitude >= 50f)
            playerDetected = false;
        else
            playerDetected = true;

        TryMove();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            animator.SetBool("Attacking", true);
            attacking = true;
            player.UI.playerHP -= 5;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            attacking = false;
            animator.SetBool("Attacking", false);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            lastAttackTime += Time.deltaTime;
            if(lastAttackTime >= 2f)
            {
                attacking = true;
                animator.SetBool("Attacking", true);
                player.UI.playerHP -= 5;
                lastAttackTime = 0f;
            }
            else
            {
                attacking = false;
                animator.SetBool("Attacking", false);
            }
        }
    }

    private void TryMove()
    {
        if (died || attacking) return;

        if (playerDetected)
            animator.SetBool("Running", true);
        else
            animator.SetBool("Running", false);
        
        var distance = player.transform.position - transform.position;
        var move = distance.normalized;

        move = new Vector3(move.x, 0, move.z);

        transform.forward = move; // 플레이어 바라보도록 설정

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
