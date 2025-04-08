using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private void Awake()
    {
        /*if (!(PlayerPrefs.HasKey("save") && PlayerPrefs.GetInt("save") == 1))
        {
            HighscoreSystem.Instance.CreateSave();
            PlayerPrefs.SetInt("save", 1);
            //Debug.Log(PlayerPrefs.GetInt("save"));
        }*/
        HighscoreSystem.Instance.CreateSave();
    }
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void UISound()
    {
        AudioManager.Instance.PlaySound("uiSound");
    }
}
