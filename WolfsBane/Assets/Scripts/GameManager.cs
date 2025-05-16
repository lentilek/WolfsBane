using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI.Table;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [HideInInspector] public float gameIndicator; // 0-100
    [SerializeField] private int thrillHuntersAppear, policemanAppear;
    private float maxGameIndicator;
    [SerializeField] private Image gameIndicatorFill;

    public int maxActionPoints = 14;
    public int maxAIActionPoints = 12;
    public float actionWaitTimeAI = 1f;
    [HideInInspector] public int currentAIActionPoints;
    [HideInInspector] public int currentActionPoints;
    public TextMeshProUGUI actionPointsTXT;
    [SerializeField] private TextMeshProUGUI actionPointsAITXT;
    [SerializeField] public GameObject nightButton;
    [SerializeField] private GameObject nextDayButton;

    [SerializeField] private int turistPerDay, thrillHunterPerDay, policemanPerDay;
    private bool isPoliceHere;
    [SerializeField] private GameObject[] turistCampModels, thrillHunterModels, policemanModels;
    [HideInInspector] public List<GameObject> turistCamps = new List<GameObject>();
    [HideInInspector] public bool isNight;

    [SerializeField] private TextMeshProUGUI daysCounterTXT;
    public int daysToWin;
    [HideInInspector] public int daysCounter;
    [HideInInspector] public int turistEaten;
    [HideInInspector] public float highestPM;

    [SerializeField] private Light mainLight;
    [SerializeField] private Color nightLightColor;
    private bool wasThereKill;
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
        isPoliceHere = false;
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

    private void Update()
    {
        if(gameIndicator >= maxGameIndicator)
        {
            Time.timeScale = 0f;
            //HighscoreSystem.Instance.GetData();
            GameUI.Instance.gameOverScreen.SetActive(true);
            gameIndicator = 0;
            isPoliceHere = false;
        }
        if (isPoliceHere && gameIndicator < policemanAppear)
        {
            gameIndicator = policemanAppear;
            GetCurrentFillIndicator();
        }
    }
    public void GetCurrentFillIndicator()
    {
        GameUI.Instance.IndicatorPulse();
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
            GameUI.Instance.MoveTime();
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
            GameUI.Instance.MoveTime();
            /*if (currentAIActionPoints == 0)
            {
                //nextDayButton.SetActive(true);
                return false;
            }*/
            return true;
        }
        return false;
    }
    public void Night()
    {
        isNight = true;
        currentAIActionPoints -= TaskManager.Instance.lessWerewolfActions;
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
            if (currentAIActionPoints > 0) PlayerControler.Instance.AreasToGoAI();
            UseActionPointAI();
            yield return new WaitForSeconds(actionWaitTimeAI);
            //if (PlayerInventory.Instance.CheckTrap(MapBoard.Instance.map[PlayerControler.Instance.row].moduleRow[PlayerControler.Instance.column])) yield return new WaitForSeconds(actionWaitTimeAI);
            //if (CheckIfTurist()) yield return new WaitForSeconds(actionWaitTimeAI);
        } while (currentAIActionPoints > 0);

        MapArea area = MapBoard.Instance.map[PlayerControler.Instance.row].moduleRow[PlayerControler.Instance.column];

        if (area.state == 5 || area.state == 6)
        {
            GameObject turist = null;
            foreach (GameObject turistCamp in turistCamps)
            {
                if (turistCamp.GetComponent<Turist>().mapModule.row == area.row &&
                    turistCamp.GetComponent<Turist>().mapModule.column == area.column)
                {
                    turist = turistCamp;
                    break;
                }
            }
            if (turist != null)
            {
                gameIndicator += turist.GetComponent<Turist>().gameIndicatorWhenScared;
                if (gameIndicator > 100) gameIndicator = 100;
                GetCurrentFillIndicator();
            }
        }
        EndNightCheckIfWon();
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
            if (turist != null && turist.GetComponent<Turist>().type == 2 && Random.Range(1, 101) <= turist.GetComponent<Turist>().killChance)
            {
                gameIndicator = maxGameIndicator;
                GetCurrentFillIndicator();
                Time.timeScale = 0f;
                GameUI.Instance.gameOverScreen.SetActive(true);
                gameIndicator = 0;
                isPoliceHere = false;
            }
            if (turist != null && UseActionPointAI())
            {
                if (area.AreThereTuristsAround()) area.state = 4;
                else if (area.state == 6) area.state = 2;
                else if (area.state == 5) area.state = 1;
                gameIndicator += turist.GetComponent<Turist>().gameIndicatorWhenKilled;
                turistEaten++;
                wasThereKill = true;
                if (gameIndicator > 100) gameIndicator = 100;
                GetCurrentFillIndicator();
                Destroy(turist);
                turistCamps.Remove(turist);
                //Debug.Log(area.neighbours.Count);
                foreach (MapArea n in area.neighbours)
                {
                    //Debug.Log(n.AreThereTuristsAround());
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
        wasThereKill = false;
        MapBoard.Instance.map[PlayerControler.Instance.row].moduleRow[PlayerControler.Instance.column].noAPTip.SetActive(false);
        PlayerControler.Instance.ButtonsAroundOff();
        nightButton.SetActive(false);
        GameUI.Instance.Night();
        Light();
        PlayerControler.Instance.playerModel.transform.eulerAngles = new Vector3(270, 30, 0);
        PlayerControler.Instance.GetCurrentAIModule();
        Night();
    }
    public void Light()
    {
        mainLight.DOColor(nightLightColor, 1f);
        //mainLight.color = nightLightColor;
        if (House.Instance != null) House.Instance.HouseNight();
    }
    public void EndNightCheckIfWon()
    {
        if(daysCounter == daysToWin)
        {
            Time.timeScale = 0f;
            //HighscoreSystem.Instance.GetData();
            daysCounter++;
            GameUI.Instance.winScreen.SetActive(true);
        }
        else
        {
            nextDayButton.SetActive(true);
            if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                if (daysCounter == 1) GameUI.Instance.Paper(1);
                else if (!wasThereKill) GameUI.Instance.Paper(2);
                else GameUI.Instance.Paper(3);
            }
        }
    }
    public void NewDay()
    {
        GameUI.Instance.Day();
        isNight = false;
        daysCounter++;
        GameUI.Instance.MoonPhase();
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
        //mainLight.color = Color.white;
        mainLight.DOColor(Color.white, 1f);
        Dialog.Instance.talkBonusChance = 0;
        TaskManager.Instance.moreResources = false;
        TaskManager.Instance.strongerBarricades = false;
        TaskManager.Instance.lessWerewolfActions = 0;
        if (House.Instance != null) House.Instance.HouseDay();
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
        if (isPoliceHere && gameIndicator < policemanAppear)
        {
            gameIndicator = policemanAppear;
            GetCurrentFillIndicator();
        }
        int currentTuristCounter = turistPerDay;
        int i = 0;
        if (gameIndicator >= policemanAppear)
        {
            isPoliceHere = true;
            while (i < policemanPerDay)
            {
                MapArea ma = MapBoard.Instance.moduleListRegular[Random.Range(0, MapBoard.Instance.moduleListRegular.Count)];
                turistCamps.Add(Instantiate(policemanModels[Random.Range(0, policemanModels.Length)],
                    ma.gameplayObject.transform, worldPositionStays: false));
                turistCamps[i].GetComponent<Turist>().mapModule = ma;
                ma.state = 6;
                foreach (MapArea n in ma.neighbours)
                {
                    if (n.state == 2) n.state = 4;
                    else if (n.state == 1) n.state = 3;
                }
                MapBoard.Instance.moduleListRegular.Remove(ma);
                currentTuristCounter--;
                i++;
            }
        }
        if (gameIndicator >= thrillHuntersAppear)
        {
            int thrillHunterCounter = 0;
            if (gameIndicator >= policemanAppear) thrillHunterCounter = thrillHunterPerDay + policemanPerDay;
            else thrillHunterCounter = thrillHunterPerDay;
            while (i < thrillHunterCounter)
            {
                MapArea ma = MapBoard.Instance.moduleListRegular[Random.Range(0, MapBoard.Instance.moduleListRegular.Count)];
                turistCamps.Add(Instantiate(thrillHunterModels[Random.Range(0, thrillHunterModels.Length)],
                    ma.gameplayObject.transform, worldPositionStays: false));
                turistCamps[i].GetComponent<Turist>().mapModule = ma;
                ma.state = 6;
                foreach (MapArea n in ma.neighbours)
                {
                    if (n.state == 2) n.state = 4;
                    else if (n.state == 1) n.state = 3;
                }
                MapBoard.Instance.moduleListRegular.Remove(ma);
                currentTuristCounter--;
                i++;
            }
        }
        while (i < turistPerDay)
        {
            MapArea ma = MapBoard.Instance.moduleListRegular[Random.Range(0, MapBoard.Instance.moduleListRegular.Count)];
            turistCamps.Add(Instantiate(turistCampModels[Random.Range(0, turistCampModels.Length)], 
                ma.gameplayObject.transform, worldPositionStays: false));
            turistCamps[i].GetComponent<Turist>().mapModule = ma;
            ma.state = 6;
            foreach(MapArea n in ma.neighbours)
            {
                if (n.state == 2) n.state = 4;
                else if (n.state == 1) n.state = 3;
            }
            MapBoard.Instance.moduleListRegular.Remove(ma);
            i++;
        }
        TaskManager.Instance.RandomTasks();
        PlayerControler.Instance.ButtonsAround();
    }
}
