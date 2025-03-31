using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI.Table;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public float gameIndicator; // 0-100 // hideInInspector public
    private float maxGameIndicator;
    [SerializeField] private Image gameIndicatorFill;

    public int maxActionPoints = 14;
    public int maxAIActionPoints = 12;
    public float actionWaitTimeAI = 1f;
    [HideInInspector] public int currentAIActionPoints;
    [HideInInspector] public int currentActionPoints;
    [SerializeField] private TextMeshProUGUI actionPointsTXT;
    [SerializeField] private TextMeshProUGUI actionPointsAITXT;
    [SerializeField] public GameObject nightButton;
    [SerializeField] private GameObject nextDayButton;

    [SerializeField] private int turistPerDay;
    [SerializeField] private GameObject[] turistCampModel;
    [HideInInspector] public List<GameObject> turistCamps = new List<GameObject>();
    [HideInInspector] public bool isNight;

    [SerializeField] private TextMeshProUGUI daysCounterTXT;
    public int daysToWin;
    [HideInInspector] public int daysCounter;
    [HideInInspector] public int turistEaten;
    [HideInInspector] public float highestPM;

    [SerializeField] private Light mainLight;
    [SerializeField] private Color nightLightColor;
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
        Time.timeScale = 1f;
        daysCounter = 0;
        turistEaten = 0;
        gameIndicator = 0;
        maxGameIndicator = 100f;
        currentActionPoints = maxActionPoints;
        currentAIActionPoints = maxAIActionPoints;
        actionPointsTXT.text = $"{currentActionPoints}/{maxActionPoints}";
        actionPointsAITXT.text = $"{currentAIActionPoints}/{maxAIActionPoints}";
        daysCounterTXT.text = $"Day: {daysCounter}";
        nextDayButton.SetActive(false);
        GetCurrentFillIndicator();
        isNight = false;
        mainLight.color = Color.white;
    }

    // update chyba bêdzie mo¿na usun¹æ i zostawiæ tylko GetCurrentFill po akcjach
    private void Update()
    {
        //GetCurrentFillIndicator();
        if(gameIndicator >= maxGameIndicator)
        {
            Time.timeScale = 0f;
            HighscoreSystem.Instance.GetData();
            GameUI.Instance.gameOverScreen.SetActive(true);
            gameIndicator = 0;
        }
    }
    public void GetCurrentFillIndicator()
    {
        float fill = gameIndicator / maxGameIndicator;
        gameIndicatorFill.fillAmount = fill;
        if (gameIndicator > highestPM) highestPM = gameIndicator;
    }
    public bool UseActionPoint()
    {
        if (currentActionPoints > 0)
        {
            currentActionPoints--;
            actionPointsTXT.text = $"{currentActionPoints}/{maxActionPoints}";
            if (currentActionPoints == 0)
            {
                nightButton.SetActive(true);
            }
            return true;
        }
        return false;
    }
    public bool UseActionPointAI()
    {
        if (currentAIActionPoints > 0)
        {
            currentAIActionPoints--;
            actionPointsAITXT.text = $"{currentAIActionPoints}/{maxAIActionPoints}";
            if (currentAIActionPoints == 0)
            {
                //nextDayButton.SetActive(true);
                return false;
            }
            return true;
        }
        return false;
    }
    public void Night()
    {
        isNight = true;
        PlayerInventory.Instance.CheckHouseForTrap();
    }
    public void StartNightWait()
    {
        StartCoroutine(NightWait());
    }
    IEnumerator NightWait()
    {
        yield return new WaitForSeconds(actionWaitTimeAI);
        do
        {
            if(PlayerInventory.Instance.CheckTrap(MapBoard.Instance.map[PlayerControler.Instance.row].moduleRow[PlayerControler.Instance.column])) yield return new WaitForSeconds(actionWaitTimeAI);
            if (CheckIfTurist()) yield return new WaitForSeconds(actionWaitTimeAI);
            PlayerControler.Instance.AreasToGoAI();
            UseActionPointAI();
            yield return new WaitForSeconds(actionWaitTimeAI);
            if (PlayerInventory.Instance.CheckTrap(MapBoard.Instance.map[PlayerControler.Instance.row].moduleRow[PlayerControler.Instance.column])) yield return new WaitForSeconds(actionWaitTimeAI);
            if (CheckIfTurist()) yield return new WaitForSeconds(actionWaitTimeAI);
        } while (currentAIActionPoints > 0);
        EndNightCheckIfWon();
        nextDayButton.SetActive(true);
    }
    public bool CheckIfTurist()
    {
        MapArea area = MapBoard.Instance.map[PlayerControler.Instance.row].moduleRow[PlayerControler.Instance.column];

        if(area.state == 5 || area.state == 6)
        {
            GameObject turist = null;
            foreach(GameObject turistCamp in turistCamps)
            {
                if (turistCamp.GetComponent<Turist>().mapModule.row == area.row && 
                    turistCamp.GetComponent<Turist>().mapModule.column == area.column)
                {
                    turist = turistCamp; 
                    break;
                }
            }
            if (turist != null && UseActionPointAI())
            {
                if (area.AreThereTuristsAround()) area.state = 4;
                else if (area.state == 6) area.state = 2;
                else if (area.state == 5) area.state = 1;
                gameIndicator += turist.GetComponent<Turist>().gameIndicatorWhenKilled;
                turistEaten++;
                if (gameIndicator > 100) gameIndicator = 100;
                GetCurrentFillIndicator();
                Destroy(turist);
                turistCamps.Remove(turist);
                foreach (MapArea n in area.neighbours)
                {
                    if (n.state == 4 && !n.AreThereTuristsAround()) n.state = 2;
                    else if (n.state == 3 && !n.AreThereTuristsAround()) n.state = 1;
                }
                return true;
            }
        }
        return false;
    }
    public void FinishDayStartNight()
    {
        MapBoard.Instance.map[PlayerControler.Instance.row].moduleRow[PlayerControler.Instance.column].noAPTip.SetActive(false);
        PlayerControler.Instance.ButtonsAroundOff();
        nightButton.SetActive(false);
        GameUI.Instance.Night();
        mainLight.color = nightLightColor;
        PlayerControler.Instance.playerModel.transform.eulerAngles = new Vector3(270, 30, 0);
        PlayerControler.Instance.GetCurrentAIModule();
        Night();
    }
    public void EndNightCheckIfWon()
    {
        if(daysCounter == daysToWin)
        {
            Time.timeScale = 0f;
            HighscoreSystem.Instance.GetData();
            GameUI.Instance.winScreen.SetActive(true);
        }
    }
    public void NewDay()
    {
        GameUI.Instance.Day();
        isNight = false;
        daysCounter++;
        daysCounterTXT.text = $"Day: {daysCounter}";     
        PlayerInventory.Instance.DestroyAllTraps();
        TaskManager.Instance.TasksDelete();
        nextDayButton.SetActive(false);
        currentActionPoints = maxActionPoints;        
        currentAIActionPoints = maxAIActionPoints;
        actionPointsTXT.text = $"{currentActionPoints}/{maxActionPoints}";
        actionPointsAITXT.text = $"{currentAIActionPoints}/{maxAIActionPoints}";
        PlayerControler.Instance.PlayerGoHome();
        PlayerControler.Instance.playerModel.transform.eulerAngles = new Vector3(270, 210, 0);
        mainLight.color = Color.white;
        foreach (MapArea ma in MapBoard.Instance.moduleListResource)
        {
            ma.gameplayObject.GetComponentInChildren<ResourceRegeneration>().Regenerate();
        }
        foreach (GameObject turist in turistCamps)
        {
            if (turist.gameObject.activeSelf)
            {
                gameIndicator -= turist.GetComponent<Turist>().gameIndicatorWhenLived;
                if (gameIndicator < 0) gameIndicator = 0;
                GetCurrentFillIndicator();
            }
            turist.GetComponent<Turist>().mapModule.state = 2;
            foreach (MapArea n in turist.GetComponent<Turist>().mapModule.neighbours)
            {
                if (n.state == 4 && !n.AreThereTuristsAround()) n.state = 2;
                else if (n.state == 3 && !n.AreThereTuristsAround()) n.state = 1;
            }
            Destroy(turist);
        }
        turistCamps.Clear();
        MapBoard.Instance.RegularModuleList();
        for (int i = 0; i < turistPerDay; i++)
        {
            MapArea ma = MapBoard.Instance.moduleListRegular[Random.Range(0, MapBoard.Instance.moduleListRegular.Count)];
            turistCamps.Add(Instantiate(turistCampModel[Random.Range(0, turistCampModel.Length)], 
                ma.gameplayObject.transform, worldPositionStays: false));
            turistCamps[i].GetComponent<Turist>().mapModule = ma;
            ma.state = 6;
            foreach(MapArea n in ma.neighbours)
            {
                if (n.state == 2) n.state = 4;
                else if (n.state == 1) n.state = 3;
            }
            MapBoard.Instance.moduleListRegular.Remove(ma);
        }
        TaskManager.Instance.RandomTasks();
        PlayerControler.Instance.ButtonsAround();
    }
}
