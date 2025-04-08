using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using System.IO.Pipes;
using static UnityEngine.ParticleSystem;

public class HighscoreSystem : MonoBehaviour
{
    public static HighscoreSystem Instance;

    [HideInInspector] public HighscoreData data;

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
        data.playerId = new string[5];
        data.days = new int[5];
        data.turistsEaten = new int[5];
        data.panicMeter = new float[5];
    }
    public void CreateSave()
    {
        if(!File.Exists(Application.persistentDataPath + "/HighscoreData.json"))
        {
            File.Create(Application.persistentDataPath + "/HighscoreData.json"); 
            ClearSavedData();
        }
    }
    public void SaveData()
    {
        string json = JsonUtility.ToJson(data, true);
        //File.WriteAllText(Application.persistentDataPath + "/HighscoreData.json", json);

        string path = Path.Combine(Application.persistentDataPath, "/HighscoreData.json");
        System.IO.File.WriteAllText(path, json);

        /* PlayerData playerData = new PlayerData();
         foreach (SceneConfigsSO s in sceneConfigs)
         {
             playerData.currentHighScores.Add(s.currentHighScore);
             playerData.islocked.Add(s.islocked);
             playerData.dialogStates.Add(s.isDialogueDone);
         }

         string json = JsonUtility.ToJson(playerData);
         string path = Path.Combine(Application.persistentDataPath, "playerData");
         System.IO.File.WriteAllText(path, json);*/
    }
    public void LoadData()
    {
        //string path = Path.Combine(Application.persistentDataPath, "HighscoreData");
        //string json = File.ReadAllText(Application.persistentDataPath + "/HighscoreData.json");
        //data = JsonUtility.FromJson<HighscoreData>(json);

        string path = Path.Combine(Application.persistentDataPath, "/HighscoreData.json");

        if (File.Exists(path))
        {
            string json = System.IO.File.ReadAllText(path);
            data = JsonUtility.FromJson<HighscoreData>(json);
        }
            /*if (File.Exists(path))
            {
                string json = System.IO.File.ReadAllText(path);
                PlayerData loadedData = JsonUtility.FromJson<PlayerData>(json);

                for (int i = 0; i < sceneConfigs.Count; i++)
                {
                    sceneConfigs[i].currentHighScore = loadedData.currentHighScores[i];
                    sceneConfigs[i].islocked = loadedData.islocked[i];
                    sceneConfigs[i].isDialogueDone = loadedData.dialogStates[i];
                }
            }
            else
            {
                Debug.Log("File not found");
            }*/
        }
        public void GetData()
    {
        LoadData();
        string id = "Player";
        int days = GameManager.Instance.daysCounter;
        int turist = GameManager.Instance.turistEaten;
        float pm = GameManager.Instance.highestPM;
        int i = 0;
        while(i < 5)
        {
            if (data.days[i] < days || (data.days[i] == days && data.turistsEaten[i] > turist) ||
                (data.days[i] == days && data.turistsEaten[i] == turist && data.panicMeter[i] > pm))
            {
                ChangeData(i, id, days, turist, pm);
                i = 6;
            }
            i++;
        }
        SaveData();
    }
    private void ChangeData(int index, string id, int days, int turist, float pm)
    {
        for(int i = 4; i > index; i--)
        {
            data.playerId[i] = data.playerId[i - 1];
            data.days[i] = data.days[i - 1];
            data.turistsEaten[i] = data.turistsEaten[i - 1];
            data.panicMeter[i] = data.panicMeter[i - 1];
        }
        data.playerId[index] = id;
        data.days[index] = days;
        data.turistsEaten[index] = turist;
        data.panicMeter[index] = pm;
    }
    public void ClearSavedData()
    {
        int i = 0;
        while (i < 5)
        {
            data.playerId[i] = $"Player {i}";
            data.days[i] = 0;
            data.turistsEaten[i] = 0;
            data.panicMeter[i] = 0;
            i++;
        }
        SaveData();
    }
}
