using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Setting : MonoBehaviour
{
    [SerializeField] PlayerController player;
    [SerializeField] Slider soundSlider;
    [SerializeField] Slider sensitivitySlider;

    private void Start()
    {
        soundSlider.onValueChanged.AddListener((sound)=>
        {
            player.shotSound.volume = sound;
            player.reloadSound.volume = sound;
        });
        sensitivitySlider.onValueChanged.AddListener((sensitivity) =>
        {
            player.sensitivity = sensitivity * 10;
        });
    }

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
