using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameScene : MonoBehaviour
{
    [SerializeField] public PlayerController player;
    [SerializeField] GameObject setting;
    [SerializeField] public UIManager UI;

    public void ShowCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void HideCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Start()
    {
        GameManager.InGameScene = this;

        HideCursor();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            if (setting.activeSelf)
                HideCursor();
            else
                ShowCursor();

            player.isPlaying = setting.activeSelf;
            setting.SetActive(!setting.activeSelf);
        }
    }

    public void GoTitle()
    {
        SceneManager.LoadScene("MainScene");
    }
}
