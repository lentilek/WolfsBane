using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

[System.Serializable]
public class InventoryObject
{
    public Image invImage;
    public TextMeshProUGUI invTXT;
    public TextMeshProUGUI invCollectTXT;
}
public class GameUI : MonoBehaviour
{
    public static GameUI Instance;

    public GameObject gameOverScreen;
    public GameObject winScreen;
    public GameObject pauseScreen;

    [SerializeField] private GameObject gameIndicator;

    public GameObject timeDay;
    public GameObject timeNight;

    public GameObject apDay;
    public GameObject apNight;

    [SerializeField] private GameObject[] moonPhases;
    [SerializeField] private Sprite[] moonPhasesSprite;
    [SerializeField] private Image nightIcon;

    [SerializeField] private GameObject apTime, apTimeEnd;
    private RectTransform apTimeTrans, apTimeEndTrans;
    private Vector3 startingPosition;
    private Vector3 nightPosition;
    [SerializeField] private float moveAmount;
    [HideInInspector] public string playerName;

    [SerializeField] private GameObject day1Paper;
    [SerializeField] private GameObject[] noKillsPaper, killPaper;

    // text at the begining of the day
    [SerializeField] private Image dailyTextImage;
    [SerializeField] private TextMeshProUGUI dailyTXT;
    [SerializeField] private float dayTextTime;
    [SerializeField] private string[] dailyTextVariant;
    private bool isOnStart;

    // inventory animation
    [SerializeField] private Color invAnimColor, invAnimTXTColor, invAnimImageColor;
    [SerializeField] private InventoryObject wood, stone, rope, meat;
    [SerializeField] private float invAnimLenght, invAnimScale;

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
        isOnStart = true;
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
    public void InventoryAnimation(int resourceType, string amount)
    {
        switch (resourceType)
        {
            case 1:
                StopCoroutine(InvAnim(wood, amount));
                wood.invImage.gameObject.transform.localScale = Vector3.one;
                wood.invTXT.gameObject.transform.localScale = Vector3.one;
                wood.invCollectTXT.gameObject.transform.localScale = Vector3.one;
                StartCoroutine(InvAnim(wood, amount));
                break;
            case 2:
                StopCoroutine(InvAnim(stone, amount));
                stone.invImage.gameObject.transform.localScale = Vector3.one;
                stone.invTXT.gameObject.transform.localScale = Vector3.one;
                stone.invCollectTXT.gameObject.transform.localScale = Vector3.one;
                StartCoroutine(InvAnim(stone, amount));
                break;
            case 3:
                StopCoroutine(InvAnim(rope, amount));
                rope.invImage.gameObject.transform.localScale = Vector3.one;
                rope.invTXT.gameObject.transform.localScale = Vector3.one;
                rope.invCollectTXT.gameObject.transform.localScale = Vector3.one;
                StartCoroutine(InvAnim(rope, amount));
                break;
            case 4:
                StopCoroutine(InvAnim(meat, amount));
                meat.invImage.gameObject.transform.localScale = Vector3.one;
                meat.invTXT.gameObject.transform.localScale = Vector3.one;
                meat.invCollectTXT.gameObject.transform.localScale = Vector3.one;
                StartCoroutine(InvAnim(meat, amount));
                break;
            default: break;
        }
    }
    private IEnumerator InvAnim(InventoryObject invObj, string amount)
    {
        invObj.invImage.color = invAnimColor;
        invObj.invImage.gameObject.transform.DOScale(invAnimScale, invAnimLenght);
        invObj.invTXT.color = invAnimColor;
        invObj.invTXT.gameObject.transform.DOScale(invAnimScale, invAnimLenght);
        invObj.invCollectTXT.text = amount;
        invObj.invCollectTXT.gameObject.SetActive(true);
        invObj.invCollectTXT.gameObject.transform.DOScale(invAnimScale, invAnimLenght);
        yield return new WaitForSeconds(invAnimLenght);
        invObj.invImage.gameObject.transform.DOScale(1f, invAnimLenght);
        invObj.invTXT.gameObject.transform.DOScale(1f, invAnimLenght);
        invObj.invCollectTXT.gameObject.transform.DOScale(1f, invAnimLenght);
        yield return new WaitForSeconds(invAnimLenght);
        if (invObj != wood) invObj.invImage.color = invAnimImageColor;
        else invObj.invImage.color = Color.white;
        invObj.invTXT.color = invAnimTXTColor;
        invObj.invCollectTXT.gameObject.SetActive(false);
    }
    public void IndicatorPulse()
    {
        StopCoroutine(GameIndicatorPulse());
        gameIndicator.transform.localScale = Vector3.one;
        StartCoroutine(GameIndicatorPulse());
    }
    private IEnumerator GameIndicatorPulse()
    {
        AudioManager.Instance.PlaySound("pm");
        gameIndicator.transform.DOScale(1.1f, .15f);
        yield return new WaitForSeconds(.15f);
        gameIndicator.transform.DOScale(.9f, .3f);
        yield return new WaitForSeconds(.3f);
        gameIndicator.transform.DOScale(1f, .15f);
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
        float all = GameManager.Instance.maxActionPoints + GameManager.Instance.maxAIActionPoints;
        nightPosition = new Vector3(startingPosition.x - (moveAmount / all * GameManager.Instance.maxActionPoints), startingPosition.y, startingPosition.z);
        timeDay.SetActive(true);
        timeNight.SetActive(false);
        apDay.SetActive(true);
        apNight.SetActive(false);
        StartCoroutine(SyncTime());
        if (GameManager.Instance.daysCounter != 1) StartDayTXT();
    }
    public void StartDayTXT()
    {
        StartCoroutine(DayTXT());
    }
    public void TutorialStartDay()
    {
        if (isOnStart) StartCoroutine(DayTXT());
    }
    IEnumerator SyncTime()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        apTimeTrans.transform.position = startingPosition;
    }
    IEnumerator DayTXT()
    {
        isOnStart = false;
        if (GameManager.Instance.daysCounter == 1) dailyTXT.text = dailyTextVariant[0];
        else if (GameManager.Instance.gameIndicator >= GameManager.Instance.policemanAppear) dailyTXT.text = $"{dailyTextVariant[2]}\n{dailyTextVariant[3]}";
        else if (GameManager.Instance.gameIndicator >= GameManager.Instance.thrillHuntersAppear) dailyTXT.text = dailyTextVariant[1];
        else dailyTXT.text = dailyTextVariant[4];
        dailyTXT.gameObject.SetActive(true);
        dailyTXT.DOFade(1f, dayTextTime / 2);
        dailyTextImage.gameObject.SetActive(true);
        dailyTextImage.DOFade(.9f, dayTextTime / 2);
        yield return new WaitForSeconds(dayTextTime / 2);
        dailyTXT.DOFade(0f, dayTextTime / 2);
        dailyTextImage.DOFade(0f, dayTextTime / 2);
        yield return new WaitForSeconds(dayTextTime / 2);
        dailyTXT.gameObject.SetActive(false);
        dailyTextImage.gameObject.SetActive(false);
    }
    public void MoveTime()
    {
        float all = GameManager.Instance.maxActionPoints + GameManager.Instance.maxAIActionPoints;
        apTimeTrans.transform.position = new Vector3(apTimeTrans.transform.position.x - (moveAmount / all), apTimeTrans.transform.position.y, apTimeTrans.transform.position.z);
    }
    public void Night()
    {
        StopCoroutine(DayTXT());
        dailyTXT.alpha = 0f;
        dailyTXT.gameObject.SetActive(false);
        nightIcon.sprite = moonPhasesSprite[GameManager.Instance.daysCounter - 1];
        timeDay.SetActive(false);
        timeNight.SetActive(true);
        apDay.SetActive(false);
        apNight.SetActive(true);
        NightAPImage();
    }
    public void Paper(int paperType) // 1 - first day,  2 - no kill,   3 - kill
    {
        PaperTextOff();
        switch (paperType)
        {
            case 1:
                day1Paper.SetActive(true);
                break;
            case 2:
                noKillsPaper[Random.Range(0, noKillsPaper.Length)].SetActive(true);
                break;
            case 3:
                killPaper[Random.Range(0, killPaper.Length)].SetActive(true);
                break;
            default: break;
        }
    }
    private void PaperTextOff()
    {
        day1Paper.SetActive(false);
        foreach(GameObject go in killPaper)
        {
            go.SetActive(false);
        }
        foreach(GameObject go in noKillsPaper)
        {
            go.SetActive(false);
        }
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
    public void UIHover()
    {
        AudioManager.Instance.PlaySound("uiHover");
    }
    public void UIClose()
    {
        AudioManager.Instance.PlaySound("uiClose");
    }
    public void TaskUISound()
    {
        AudioManager.Instance.PlaySound("taskMenu");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
