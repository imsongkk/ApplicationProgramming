using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Setting : MonoBehaviour
{
    [SerializeField] PlayerController player;

    private void OnEnable()
    {
        Debug.Log("A");
        player.isPlaying = false;
    }

    private void OnDisable()
    {
        Debug.Log("B");
        player.isPlaying = true;
    }
}
