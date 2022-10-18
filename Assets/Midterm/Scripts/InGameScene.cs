using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameScene : MonoBehaviour
{
    [SerializeField] PlayerController player;
    [SerializeField] GameObject setting;
    [SerializeField] public UIManager UI;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            if(setting.activeSelf)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            player.isPlaying = setting.activeSelf;
            setting.SetActive(!setting.activeSelf);
        }
    }

    public void GoTitle()
    {
        SceneManager.LoadScene("MainScene");
    }
}
