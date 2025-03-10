using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public static TaskManager Instance;

    public List<DailyTaskSO> allTasksListResource = new List<DailyTaskSO>();
    public List<DailyTaskSO> allTasksListRegular = new List<DailyTaskSO>();
    private List<DailyTaskSO> randomTasksList = new List<DailyTaskSO>();
    public List<DailyTaskSO> todayTasksList = new List<DailyTaskSO>(); // after ui hide in inspector

    [HideInInspector] public int completeTasksCount;

    // leaf cleaning
    public int leavesToClean;
    [HideInInspector] public int leavesCleaned;
    public int leavesToSpawn;
    public GameObject[] leavesPrefabs;
    [HideInInspector] public List<Leaves> allLeaves = new List<Leaves>();
    // salt cubes
    public int saltCubesPlacesToSpawn;
    public GameObject[] saltCubePlacePrefabs;
    [HideInInspector] public List<SaltCubes> allSaltCubes = new List<SaltCubes>();
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

    public void RandomTasks()
    {
        completeTasksCount = 0;
        todayTasksList.Clear();
        randomTasksList.Clear();
        foreach(DailyTaskSO task in allTasksListRegular)
        {
            randomTasksList.Add(task);
        }
        foreach(DailyTaskSO task in allTasksListResource)
        {
            randomTasksList.Add(task);
        }
        int index = Random.Range(0, allTasksListResource.Count);
        todayTasksList.Add(allTasksListResource[index]);
        randomTasksList.Remove(allTasksListResource[index]);

        index = Random.Range(0, allTasksListRegular.Count);
        todayTasksList.Add(allTasksListRegular[index]);
        randomTasksList.Remove(allTasksListRegular[index]);

        index = Random.Range(0, randomTasksList.Count);
        todayTasksList.Add(randomTasksList[index]);

        TasksSetUp();
    }
    public void TasksSetUp()
    {
        foreach(DailyTaskSO task in todayTasksList)
        {
            switch (task.taskIndex)
            {
                case 1:
                    LeafCleaningSetUp();
                    break;
                case 2:
                    SaltCubesSetUp();
                    break;
                default: break;
            }
        }
    }
    public void TasksDelete()
    {
        foreach(DailyTaskSO task in todayTasksList)
        {
            switch(task.taskIndex)
            {
                case 1:
                    LeafCleaningDelete();
                    break;
                case 2:
                    SaltCubesDelete();
                    break;
                default: break;
            }
        }
    }
    private void LeafCleaningSetUp()
    {
        allLeaves.Clear();
        MapBoard.Instance.EmptyModuleList();
        leavesCleaned = 0;
        for(int i=0; i<leavesToSpawn; i++)
        {
            int index = Random.Range(0, MapBoard.Instance.moduleListEmpty.Count);
            MapArea area = MapBoard.Instance.moduleListEmpty[index];
            area.taskIndex = 1;
            GameObject gm = Instantiate(leavesPrefabs[Random.Range(0, leavesPrefabs.Length)], 
                area.gameplayObject.transform, worldPositionStays: false) as GameObject;
            gm.GetComponent<Leaves>().module = area;
            allLeaves.Add(gm.GetComponent<Leaves>());
            MapBoard.Instance.moduleListEmpty.Remove(MapBoard.Instance.moduleListEmpty[index]);
        }
    }
    public void LeafCleaningDone(MapArea area)
    {
        leavesCleaned++;
        area.taskIndex = 0;
        if(leavesCleaned == leavesToClean)
        {
            completeTasksCount++;
            CheckTaskComplition();
        }
    }
    private void LeafCleaningDelete()
    {
        foreach(Leaves leaves in allLeaves)
        {
            leaves.module.taskIndex = 0;
            Destroy(leaves.gameObject);
        }
    }
    private void SaltCubesSetUp()
    {
        allSaltCubes.Clear();
        MapBoard.Instance.EmptyModuleList();
        for (int i = 0; i < saltCubesPlacesToSpawn; i++)
        {
            int index = Random.Range(0, MapBoard.Instance.moduleListEmpty.Count);
            MapArea area = MapBoard.Instance.moduleListEmpty[index];
            area.taskIndex = 2;
            GameObject gm = Instantiate(saltCubePlacePrefabs[Random.Range(0, saltCubePlacePrefabs.Length)],
                area.gameplayObject.transform, worldPositionStays: false) as GameObject;
            gm.GetComponent<SaltCubes>().module = area;
            allSaltCubes.Add(gm.GetComponent<SaltCubes>());
            MapBoard.Instance.moduleListEmpty.Remove(MapBoard.Instance.moduleListEmpty[index]);
        }
    }
    public void SaltCubesDone(MapArea area)
    {
        area.taskIndex = 0;
        completeTasksCount++;
        CheckTaskComplition();
    }
    private void SaltCubesDelete()
    {
        foreach (SaltCubes sc in allSaltCubes)
        {
            sc.module.taskIndex = 0;
            Destroy(sc.gameObject);
        }
    }

    public void CheckTaskComplition()
    {
        switch (completeTasksCount)
        {
            case 1:
                Debug.Log("Reward 1");
                break;
            case 2:
                Debug.Log("Reward 2");
                break;
            case 3:
                Debug.Log("Reward 3");
                break;
            default: break;
        }
    }
}
