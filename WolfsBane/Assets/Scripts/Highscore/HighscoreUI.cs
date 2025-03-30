using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighscoreUI : MonoBehaviour
{
    [SerializeField] private List<TextMeshProUGUI> playerId;
    [SerializeField] private List<TextMeshProUGUI> days;
    [SerializeField] private List<TextMeshProUGUI> turistsEaten;
    [SerializeField] private List<TextMeshProUGUI> panicMeter;

    public void ShowHighscores()
    {
        HighscoreSystem.Instance.LoadData();
        int i = 0;
        foreach(string id in HighscoreSystem.Instance.data.playerId)
        {
            Debug.Log(HighscoreSystem.Instance.data.days[i]);
            playerId[i].text = $"ID: {id}";
            days[i].text = $"Days survived: {HighscoreSystem.Instance.data.days[i]}";
            turistsEaten[i].text = $"Turists eaten: {HighscoreSystem.Instance.data.turistsEaten[i]}";
            panicMeter[i].text = $"Panic Meter: {HighscoreSystem.Instance.data.panicMeter[i]}";
            i++;
        }
    }
}
