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

    public HighscoreSO data;
    public string saveName;

    public HighscoreData saveData;

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
    }
    public void CreateSave()
    {
        string savesFolderPath = Application.persistentDataPath + "/saves";
        if (!File.Exists($"{savesFolderPath}/{saveName}.save"))
        {
            ClearSavedData();
        }
    }
    public void SaveData()
    {
        saveData.saveName = saveName;

        saveData.id1 = data.id1;
        saveData.id2 = data.id2;
        saveData.id3 = data.id3;
        saveData.id4 = data.id4;
        saveData.id5 = data.id5;

        saveData.days1 = data.days1;
        saveData.days2 = data.days2;
        saveData.days3 = data.days3;
        saveData.days4 = data.days4;
        saveData.days5 = data.days5;

        saveData.turistseaten1 = data.turistseaten1;
        saveData.turistseaten2 = data.turistseaten2;
        saveData.turistseaten3 = data.turistseaten3;
        saveData.turistseaten4 = data.turistseaten4;
        saveData.turistseaten5 = data.turistseaten5;

        saveData.panicmeter1 = data.panicmeter1;
        saveData.panicmeter2 = data.panicmeter2;
        saveData.panicmeter3 = data.panicmeter3;
        saveData.panicmeter4 = data.panicmeter4;
        saveData.panicmeter5 = data.panicmeter5;

        SerializationManager.Save(saveName, saveData);
    }
    public void LoadData()
    {
        saveData = (HighscoreData)SerializationManager.Load(saveName);
        if(saveData != null )
        {
            data.id1 = saveData.id1;
            data.id2 = saveData.id2;
            data.id3 = saveData.id3;
            data.id4 = saveData.id4;
            data.id5 = saveData.id5;

            data.days1 = saveData.days1;
            data.days2 = saveData.days2;
            data.days3 = saveData.days3;
            data.days4 = saveData.days4;
            data.days5 = saveData.days5;

            data.turistseaten1 = saveData.turistseaten1;
            data.turistseaten2 = saveData.turistseaten2;
            data.turistseaten3 = saveData.turistseaten3;
            data.turistseaten4 = saveData.turistseaten4;
            data.turistseaten5 = saveData.turistseaten5;

            data.panicmeter1 = saveData.panicmeter1;
            data.panicmeter2 = saveData.panicmeter2;
            data.panicmeter3 = saveData.panicmeter3;
            data.panicmeter4 = saveData.panicmeter4;
            data.panicmeter5 = saveData.panicmeter5;
        }
    }
    public void GetData()
    {
        LoadData();
        if (data != null)
        {
            string id = "Player";
            int days = GameManager.Instance.daysCounter;
            int turist = GameManager.Instance.turistEaten;
            float pm = GameManager.Instance.highestPM;
            if((days > data.days1) || (days == data.days1 && turist < data.turistseaten1) ||
                (days == data.days1 && turist == data.turistseaten1 && pm <= data.panicmeter1))
            {
                data.id5 = data.id4;
                data.days5 = data.days4;
                data.turistseaten5 = data.turistseaten4;
                data.panicmeter5 = data.panicmeter4;

                data.id4 = data.id3;
                data.days4 = data.days3;
                data.turistseaten4 = data.turistseaten3;
                data.panicmeter4 = data.panicmeter3;

                data.id3 = data.id2;
                data.days3 = data.days2;
                data.turistseaten3 = data.turistseaten2;
                data.panicmeter3 = data.panicmeter2;

                data.id2 = data.id1;
                data.days2 = data.days1;
                data.turistseaten2 = data.turistseaten1;
                data.panicmeter2 = data.panicmeter1;

                data.id1 = id;
                data.days1 = days;
                data.turistseaten1 = turist;
                data.panicmeter1 = pm;
            }
            else if((days > data.days2) || (days == data.days2 && turist < data.turistseaten2) ||
                (days == data.days2 && turist == data.turistseaten2 && pm <= data.panicmeter2))
            {
                data.id5 = data.id4;
                data.days5 = data.days4;
                data.turistseaten5 = data.turistseaten4;
                data.panicmeter5 = data.panicmeter4;

                data.id4 = data.id3;
                data.days4 = data.days3;
                data.turistseaten4 = data.turistseaten3;
                data.panicmeter4 = data.panicmeter3;

                data.id3 = data.id2;
                data.days3 = data.days2;
                data.turistseaten3 = data.turistseaten2;
                data.panicmeter3 = data.panicmeter2;

                data.id2 = id;
                data.days2 = days;
                data.turistseaten2 = turist;
                data.panicmeter2 = pm;
            }
            else if ((days > data.days3) || (days == data.days3 && turist < data.turistseaten3) ||
                (days == data.days3 && turist == data.turistseaten3 && pm <= data.panicmeter3))
            {
                data.id5 = data.id4;
                data.days5 = data.days4;
                data.turistseaten5 = data.turistseaten4;
                data.panicmeter5 = data.panicmeter4;

                data.id4 = data.id3;
                data.days4 = data.days3;
                data.turistseaten4 = data.turistseaten3;
                data.panicmeter4 = data.panicmeter3;

                data.id3 = id;
                data.days3 = days;
                data.turistseaten3 = turist;
                data.panicmeter3 = pm;
            }
            else if ((days > data.days4) || (days == data.days4 && turist < data.turistseaten4) ||
                (days == data.days4 && turist == data.turistseaten4 && pm <= data.panicmeter4))
            {
                data.id5 = data.id4;
                data.days5 = data.days4;
                data.turistseaten5 = data.turistseaten4;
                data.panicmeter5 = data.panicmeter4;

                data.id4 = id;
                data.days4 = days;
                data.turistseaten4 = turist;
                data.panicmeter4 = pm;
            } 
            else if((days > data.days5) || (days == data.days5 && turist < data.turistseaten5) ||
                (days == data.days5 && turist == data.turistseaten5 && pm <= data.panicmeter5))
            {
                data.id5 = id;
                data.days5 = days;
                data.turistseaten5 = turist;
                data.panicmeter5 = pm;
            }
        }
        SaveData();
    }
    public void ClearSavedData()
    {
        data.id1 = "Player 1";
        data.id2 = "Player 2";
        data.id3 = "Player 3";
        data.id4 = "Player 4";
        data.id5 = "Player 5";

        data.days1 = 0;
        data.days2 = 0;
        data.days3 = 0;
        data.days4 = 0;
        data.days5 = 0;

        data.turistseaten1 = 0;
        data.turistseaten2 = 0;
        data.turistseaten3 = 0;
        data.turistseaten4 = 0;
        data.turistseaten5 = 0;

        data.panicmeter1 = 0;
        data.panicmeter2 = 0;
        data.panicmeter3 = 0;
        data.panicmeter4 = 0;
        data.panicmeter5 = 0;
        
        SaveData();
    }
}
