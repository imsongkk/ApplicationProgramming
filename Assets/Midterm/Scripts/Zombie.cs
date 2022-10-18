using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    Animator animator;

    public int hp = 100;
    public bool died = false;

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
        GameManager.Instance.InGameScene().UI.score += 1;
        animator.SetTrigger("Die");
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
