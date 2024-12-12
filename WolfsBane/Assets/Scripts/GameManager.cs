using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float gameIndicator; // 0-100 // hideInInspector public
    private float maxGameIndicator;
    [SerializeField] private Image gameIndicatorFill;

    private void Awake()
    {
        gameIndicator = 0;
        maxGameIndicator = 100f;
        GetCurrentFill();
    }
    // update chyba bêdzie mo¿na usun¹æ i zostawiæ tylko GetCurrentFill po akcjach
    private void Update()
    {
        GetCurrentFill();
        if(gameIndicator >= maxGameIndicator)
        {
            Debug.Log("Game Over");
        }
    }
    public void GetCurrentFill()
    {
        float fill = gameIndicator / maxGameIndicator;
        gameIndicatorFill.fillAmount = fill;
    }
}
