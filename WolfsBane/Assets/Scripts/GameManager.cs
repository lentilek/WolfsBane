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
    [SerializeField] private GameObject nextDayButton;

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
        currentAIActionPoints = 0;
        nextDayButton.SetActive(false);
        GetCurrentFill();
    }
    // update chyba b�dzie mo�na usun�� i zostawi� tylko GetCurrentFill po akcjach
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
                nextDayButton.SetActive(true);
            }
            return true;
        }
        return false;
    }
}
