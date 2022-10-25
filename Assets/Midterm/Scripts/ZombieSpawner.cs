using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    [SerializeField] GameObject zombiePrefab;

    GameObject spawnedZombie;

    float elapsedTime = 0f;

    private void Start()
    {
        spawnedZombie = Instantiate(zombiePrefab,transform.parent);
    }
    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= 5f)
        {
            if(spawnedZombie == null) // 스폰한 좀비가 죽으면
                spawnedZombie = Instantiate(zombiePrefab, transform.parent); // 좀비 생성
            elapsedTime = 0f;
        }
    }
}
