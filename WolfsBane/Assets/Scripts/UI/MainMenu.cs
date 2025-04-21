using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject tutorialsAsk;
    private void Start()
    {
        tutorialsAsk.SetActive(false);
        HighscoreSystem.Instance.CreateSave();
        if(!PlayerPrefs.HasKey("Tutorials"))
        {
            PlayerPrefs.SetInt("Tutorials", 0);
        }
    }
    public void StartGame()
    {
        if (PlayerPrefs.GetInt("Tutorials") == 1)
        {
            SceneManager.LoadScene(1);
        }
        else
        {
            tutorialsAsk.SetActive(true);
        }
    }
    public void StartGameNoReminder(bool noReminder)
    {
        if (noReminder) PlayerPrefs.SetInt("Tutorials", 1);
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
