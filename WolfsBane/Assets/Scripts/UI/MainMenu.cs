using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject tutorialsAsk, settings, credits;
    private void Start()
    {
        Time.timeScale = 1f;
        MusicPlayer.Instance.ChangeMusic(0);
        tutorialsAsk.SetActive(false);
        CloseUI();
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
    public void SettingsOpen()
    {
        CloseUI();
        settings.SetActive(true);
        Settings.Instance.Open();
    }
    public void Credits()
    {
        CloseUI();
        credits.SetActive(true);
    }
    public void CloseUI()
    {
        settings.SetActive(false);
        credits.SetActive(false);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void UISound()
    {
        AudioManager.Instance.PlaySound("uiSound");
    }
    public void UIHover()
    {
        AudioManager.Instance.PlaySound("uiHover");
    }
    public void UIClose()
    {
        AudioManager.Instance.PlaySound("uiClose");
    }
}
