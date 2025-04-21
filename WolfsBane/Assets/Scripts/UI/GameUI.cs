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

    [SerializeField] private GameObject[] moonPhases;

    [SerializeField] private GameObject apTime, apTimeEnd;
    private RectTransform apTimeTrans, apTimeEndTrans;
    private Vector3 startingPosition;
    private Vector3 nightPosition;
    [SerializeField] private float moveAmount;
    [HideInInspector] public string playerName;

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
        apTimeTrans = apTime.GetComponent<RectTransform>();
        apTimeEndTrans = apTimeEnd.GetComponent<RectTransform>();

    }
    private void Start()
    {        
        startingPosition = new Vector3(apTimeTrans.transform.position.x, apTimeTrans.transform.position.y, apTimeTrans.transform.position.z);
        moveAmount = apTimeTrans.transform.position.x - apTimeEndTrans.transform.position.x;
        float all = GameManager.Instance.maxActionPoints + GameManager.Instance.maxAIActionPoints;
        nightPosition = new Vector3(startingPosition.x - (moveAmount / all * GameManager.Instance.maxActionPoints), startingPosition.y, startingPosition.z);
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
    public void MoonPhase()
    {
        foreach(GameObject go in moonPhases)
        {
            go.SetActive(false);
        }
        moonPhases[GameManager.Instance.daysCounter - 1].gameObject.SetActive(true);
    }
    public void Day()
    {
        timeDay.SetActive(true);
        timeNight.SetActive(false);
        apDay.SetActive(true);
        apNight.SetActive(false);
        StartCoroutine(SyncTime());
    }
    IEnumerator SyncTime()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        apTimeTrans.transform.position = startingPosition;
    }
    public void MoveTime()
    {
        float all = GameManager.Instance.maxActionPoints + GameManager.Instance.maxAIActionPoints;
        apTimeTrans.transform.position = new Vector3(apTimeTrans.transform.position.x - (moveAmount / all), apTimeTrans.transform.position.y, apTimeTrans.transform.position.z);
    }
    public void Night()
    {        
        timeDay.SetActive(false);
        timeNight.SetActive(true);
        apDay.SetActive(false);
        apNight.SetActive(true);
        NightAPImage();
    }
    public void NightAPImage()
    {
        apTimeTrans.transform.position = new Vector3(nightPosition.x, apTimeTrans.transform.position.y, apTimeTrans.transform.position.z);
    }
    public void ReadStringInput(string name)
    {
        playerName = name;
    }
    public void SaveData()
    {
        HighscoreSystem.Instance.GetData();
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
