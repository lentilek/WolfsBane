using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        HighscoreSystem.Instance.CreateSave();
    }
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
    public void LoadTutorial(int index)
    {
        SceneManager.LoadScene(index);
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
