using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Setting : MonoBehaviour
{
    [SerializeField] PlayerController player;
    [SerializeField] Slider soundSlider;
    [SerializeField] Slider sensitivitySlider;
    [SerializeField] TextMeshProUGUI sensitivityText;

    private void Start()
    {
        sensitivityText.text = $"현재 감도 : {(sensitivitySlider.value*100).ToString("F1")}";
        soundSlider.onValueChanged.AddListener((sound)=>
        {
            player.shotSound.volume = sound;
            player.reloadSound.volume = sound;
        });
        sensitivitySlider.onValueChanged.AddListener((_) =>
        {
            float sensitivity = _ * 10;
            (sensitivity*10).ToString("F1"); // 1.2
            sensitivityText.text = $"현재 감도 : {(sensitivity * 10).ToString("F1")}";
            player.sensitivity = sensitivity;
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
