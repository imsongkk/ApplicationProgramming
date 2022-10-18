using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] public Image hpbar;
    [SerializeField] public Image ammoImage;
    [SerializeField] public TextMeshProUGUI ammo;
    [SerializeField] public TextMeshProUGUI hpText;
    [SerializeField] public TextMeshProUGUI isTPS;
    [SerializeField] public TextMeshProUGUI scoreText;
    [SerializeField] public GameObject TPSCamera;
    [SerializeField] public GameObject FPSCamera;

    public bool isTps = true;
    public int playerHP = 50;
    public int playerEntireHP = 100;
    public int playerAmmo = 10;
    public int playerEntireAmmo = 10;
    public int score = 0;

    public Color color;

    private void Start()
    {
        color = ammoImage.color;
    }

    // Update is called once per frame
    void Update()
    {
        ammo.text = $"³²Àº Åº¾à : {playerAmmo} / {playerEntireAmmo}";
        isTPS.text = isTps ? "3ÀÎÄª" : "1ÀÎÄª";
        hpbar.fillAmount = playerHP / (float)playerEntireHP;
        hpText.text = $"{playerHP} / {playerEntireHP}";
        if (playerAmmo == 0)
            ammoImage.color = Color.red;
        else
            ammoImage.color = color;
        scoreText.text = $"Score : {score}";
        if (GameManager.Instance.highscore < score)
            GameManager.Instance.highscore = score;
        if (isTps)
        {
            TPSCamera.SetActive(true);
            FPSCamera.SetActive(false);
        }
        else
        {
            FPSCamera.SetActive(true);
            TPSCamera.SetActive(false);
        }
    }
}
