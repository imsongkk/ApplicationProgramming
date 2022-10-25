using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainScene : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI highscore;
    [SerializeField] Image tutrorialImage;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Init();
        highscore.text = $"High Score : {GameManager.Instance.highscore}";
    }

    public void OnClickTutorial()
    {
        tutrorialImage.gameObject.SetActive(true);
    }

    public void CloseTutorial()
    {
        tutrorialImage.gameObject.SetActive(false);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("InGameScene");
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
