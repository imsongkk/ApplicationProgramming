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
            if(spawnedZombie == null) // ������ ���� ������
                spawnedZombie = Instantiate(zombiePrefab, transform.parent); // ���� ����
            elapsedTime = 0f;
        }
    }
}
