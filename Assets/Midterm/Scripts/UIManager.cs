using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] public Image hpbar;
    [SerializeField] public Image ammoImage;
    [SerializeField] public Image zoomImage;
    [SerializeField] public Image crossHairImage;
    [SerializeField] public Image gameOverImage;
    [SerializeField] public TextMeshProUGUI ammo;
    [SerializeField] public TextMeshProUGUI hpText;
    [SerializeField] public TextMeshProUGUI isTPS;
    [SerializeField] public TextMeshProUGUI scoreText;
    [SerializeField] public GameObject TPSCamera;
    [SerializeField] public GameObject FPSCamera;
    [SerializeField] public PlayerController playerController;

    public bool isTps = true;
    public int playerHP = 50;
    public int playerEntireHP = 100;
    public int playerAmmo = 10;
    public int playerEntireAmmo = 10;
    public int score = 0;
    public bool gameOvered = false;

    public Color color;

    private void Start()
    {
        color = ammoImage.color;
        playerController.UI = this;
    }

    // Update is called once per frame
    void Update()
    {
        ammo.text = $"???? ź?? : {playerAmmo} / {playerEntireAmmo}";
        isTPS.text = isTps ? "3??Ī" : "1??Ī";
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

        if (playerHP <= 0) // ???? ????
        {
            GameManager.InGameScene.ShowCursor();
            gameOverImage.gameObject.SetActive(true);
            playerController.isPlaying = false;
            //playerController.gameoverSound.PlayOneShot(playerController.gameoverSound.clip);
            if(!gameOvered)
            {
                gameOvered = true;
                playerController.gameoverSound.Play();
            }
            playerHP = 0;
            return;
        }
    }
}
