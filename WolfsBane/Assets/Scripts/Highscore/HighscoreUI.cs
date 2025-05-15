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

    [SerializeField] private HighscoreSO score;

    public void ShowHighscores()
    {
        HighscoreSystem.Instance.LoadData();
        playerId[0].text = $"ID: {score.id1}";
        playerId[1].text = $"ID: {score.id2}";
        playerId[2].text = $"ID: {score.id3}";
        playerId[3].text = $"ID: {score.id4}";
        playerId[4].text = $"ID: {score.id5}";

        days[0].text = $"Nights survived: {score.days1}";
        days[1].text = $"Nights survived: {score.days2}";
        days[2].text = $"Nights survived: {score.days3}";
        days[3].text = $"Nights survived: {score.days4}";
        days[4].text = $"Nights survived: {score.days5}";

        turistsEaten[0].text = $"Turists eaten: {score.turistseaten1}";
        turistsEaten[1].text = $"Turists eaten: {score.turistseaten2}";
        turistsEaten[2].text = $"Turists eaten: {score.turistseaten3}";
        turistsEaten[3].text = $"Turists eaten: {score.turistseaten4}";
        turistsEaten[4].text = $"Turists eaten: {score.turistseaten5}";

        panicMeter[0].text = $"Panic Meter: {score.panicmeter1}";
        panicMeter[1].text = $"Panic Meter: {score.panicmeter2}";
        panicMeter[2].text = $"Panic Meter: {score.panicmeter3}";
        panicMeter[3].text = $"Panic Meter: {score.panicmeter4}";
        panicMeter[4].text = $"Panic Meter: {score.panicmeter5}";
    }
}
