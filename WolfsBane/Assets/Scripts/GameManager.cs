using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public float gameIndicator; // 0-100 // hideInInspector public
    private float maxGameIndicator;
    [SerializeField] private Image gameIndicatorFill;

    [SerializeField] private int maxActionPoints = 14;
    public int maxAIActionPoints = 12;
    [HideInInspector] public int currentAIActionPoints;
    [HideInInspector] public int currentActionPoints;
    [SerializeField] private TextMeshProUGUI actionPointsTXT;
    [SerializeField] private TextMeshProUGUI actionPointsAITXT;
    [SerializeField] private GameObject nightButton;
    [SerializeField] private GameObject nextDayButton;

    [SerializeField] private int turistPerDay;
    [SerializeField] private GameObject[] turistCampModel;
    private List<GameObject> turistCamps = new List<GameObject>();

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
        gameIndicator = 0;
        maxGameIndicator = 100f;
        currentActionPoints = 0;        
        actionPointsTXT.text = $"{currentActionPoints}/{maxActionPoints}";
        actionPointsAITXT.text = $"{currentAIActionPoints}/{maxAIActionPoints}";
        currentAIActionPoints = 0;
        nextDayButton.SetActive(false);
        GetCurrentFill();
    }
    private void Start()
    {
        MapBoard.Instance.RegularModuleList();
        NewDay();
    }
    // update chyba bêdzie mo¿na usun¹æ i zostawiæ tylko GetCurrentFill po akcjach
    private void Update()
    {
        GetCurrentFill();
        if(gameIndicator >= maxGameIndicator)
        {
            Debug.Log("Game Over");
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
    }
    public void GetCurrentFill()
    {
        float fill = gameIndicator / maxGameIndicator;
        gameIndicatorFill.fillAmount = fill;
    }
    public bool UseActionPoint()
    {
        if (currentActionPoints < maxActionPoints)
        {
            currentActionPoints++;
            actionPointsTXT.text = $"{currentActionPoints}/{maxActionPoints}";
            if (currentActionPoints == maxActionPoints)
            {
                nightButton.SetActive(true);
            }
            return true;
        }
        return false;
    }
    public bool UseActionPointAI()
    {
        if (currentAIActionPoints < maxAIActionPoints)
        {
            currentAIActionPoints++;
            actionPointsAITXT.text = $"{currentAIActionPoints}/{maxAIActionPoints}";
            if (currentAIActionPoints == maxAIActionPoints)
            {
                EndNight();
                nextDayButton.SetActive(true);
            }
            return true;
        }
        return false;
    }
    public void Night()
    {
        while (UseActionPointAI())
        {
            //
        }
    }
    public void FinishDayStartNight()
    {
        nightButton.SetActive(false);
        Night();
    }
    public void EndNight()
    {
        foreach(GameObject turist in turistCamps)
        {
            if (turist.gameObject.activeSelf)
            {
                gameIndicator -= turist.GetComponent<Turist>().gameIndicatorWhenLived;
                if(gameIndicator < 0) gameIndicator = 0;
            }
        }
    }
    public void NewDay()
    {
        nextDayButton.SetActive(false);
        currentActionPoints = 0;        
        currentAIActionPoints = 0;
        actionPointsTXT.text = $"{currentActionPoints}/{maxActionPoints}";
        actionPointsAITXT.text = $"{currentAIActionPoints}/{maxAIActionPoints}";
        foreach (GameObject turist in turistCamps)
        {
            Destroy(turist);
        }
        turistCamps.Clear();
        for (int i = 0; i < turistPerDay; i++)
        {
            MapArea ma = MapBoard.Instance.moduleListRegular[Random.Range(0, MapBoard.Instance.moduleListRegular.Count)];
            turistCamps.Add(Instantiate(turistCampModel[Random.Range(0, turistCampModel.Length)], ma.gameplayObject.transform, worldPositionStays: false));
        }
    }
}
