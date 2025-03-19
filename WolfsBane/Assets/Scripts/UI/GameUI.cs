using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    public static GameUI Instance;

    public GameObject gameOverScreen;
    public GameObject winScreen;
    public GameObject pauseScreen;

    public GameObject timeDay;
    public GameObject timeNight;

    public GameObject apDay;
    public GameObject apNight;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(Instance.gameObject);
            Instance = this;
        }
        gameOverScreen.SetActive(false);
        winScreen.SetActive(false);
        pauseScreen.SetActive(false);
        Day();
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && !pauseScreen.activeSelf)
        {
            Pause();
        }
        else if(Input.GetKeyDown(KeyCode.Escape) && pauseScreen.activeSelf)
        {
            Resume();
        }
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        pauseScreen.SetActive(true);
    }
    public void Resume()
    {
        Time.timeScale = 1f;
        pauseScreen.SetActive(false);
    }
    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Day()
    {
        timeDay.SetActive(true);
        timeNight.SetActive(false);
        apDay.SetActive(true);
        apNight.SetActive(false);
    }
    public void Night()
    {        
        timeDay.SetActive(false);
        timeNight.SetActive(true);
        apDay.SetActive(false);
        apNight.SetActive(true);
    }
    public void UISound()
    {
        AudioManager.Instance.PlaySound("uiSound");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
