using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public static TaskManager Instance;

    public List<DailyTaskSO> allTasksListResource = new List<DailyTaskSO>();
    public List<DailyTaskSO> allTasksListRegular = new List<DailyTaskSO>();
    private List<DailyTaskSO> randomTasksList = new List<DailyTaskSO>();
    [HideInInspector] public List<DailyTaskSO> todayTasksList = new List<DailyTaskSO>();
    [SerializeField] private DailyTasksUI[] tasksUI;
    [SerializeField] private DailyTasksUI[] rewardsUI;

    public List<DailyTaskSO> regularTaskRewards = new List<DailyTaskSO>();
    public List<DailyTaskSO> specialTaskRewards = new List<DailyTaskSO>();
    private List<DailyTaskSO> currentTaskRewards = new List<DailyTaskSO>();
    [SerializeField] private int resourceRewardAmount;
    [SerializeField] private int indicatorRewardAmount;

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
    // clearing the path
    public int woodToSpawn;
    public GameObject[] woodPrefabs;
    [HideInInspector] public List<ClearPath> allClearPath = new List<ClearPath>();
    // lake
    public int waterToMeasure;
    [HideInInspector] public int waterMeasured;
    // trail cams
    public int trailCamToInstall;
    [HideInInspector] public int trailCamInstalled;
    public int trailCamToSpawn;
    public GameObject[] trailCamPrefabs;
    [HideInInspector] public List<TrailCam> allTrailCam = new List<TrailCam>();
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

        currentTaskRewards.Clear();
        currentTaskRewards.Add(regularTaskRewards[Random.Range(0, regularTaskRewards.Count)]);
        currentTaskRewards.Add(regularTaskRewards[Random.Range(0, regularTaskRewards.Count)]);
        currentTaskRewards.Add(specialTaskRewards[Random.Range(0, specialTaskRewards.Count)]);

        UISetUp();
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
                case 3:
                    ClearingThePathSetUp();
                    break;
                case 4:
                    PetrographicResearchSetUp();
                    break;
                case 5:
                    MeasuringWaterSetUp();
                    break;
                case 6:
                    TakeTrashSetUp();
                    break;
                case 7:
                    TrailCamSetUp();
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
                case 3:
                    ClearingThePathDelete();
                    break;
                case 4:
                    PetrographicResearchDelete();
                    break;
                case 5:
                    MeasuringWaterDelete();
                    break;
                case 6:
                    TakeTrashDelete();
                    break;
                case 7:
                    TrailCamDelete();
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
        CheckTaskCounter(1);
        if (leavesCleaned == leavesToClean)
        {
            CheckTask(1);
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
        CheckTask(2);
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
    private void ClearingThePathSetUp()
    {
        allClearPath.Clear();
        MapBoard.Instance.EmptyModuleList();
        for (int i = 0; i < woodToSpawn; i++)
        {
            int index = Random.Range(0, MapBoard.Instance.moduleListEmpty.Count);
            MapArea area = MapBoard.Instance.moduleListEmpty[index];
            area.taskIndex = 3;
            GameObject gm = Instantiate(woodPrefabs[Random.Range(0, saltCubePlacePrefabs.Length)],
                area.gameplayObject.transform, worldPositionStays: false) as GameObject;
            gm.GetComponent<ClearPath>().module = area;
            allClearPath.Add(gm.GetComponent<ClearPath>());
            MapBoard.Instance.moduleListEmpty.Remove(MapBoard.Instance.moduleListEmpty[index]);
        }
    }
    public void ClearingThePathDone(MapArea area)
    {
        area.taskIndex = 0;
        CheckTask(3);
        completeTasksCount++;
        CheckTaskComplition();
    }
    private void ClearingThePathDelete()
    {
        foreach (ClearPath sc in allClearPath)
        {
            sc.module.taskIndex = 0;
            Destroy(sc.gameObject);
        }
    }
    private void PetrographicResearchSetUp()
    {
        foreach(MapArea ma in MapBoard.Instance.moduleListBlocked)
        {
            RockResearch rr = ma.gameplayObject.GetComponentInChildren<RockResearch>();
            if(rr != null)
            {
                rr.Prepare();
            }
        }
    }
    public void PetrograpthicResearchDone()
    {
        CheckTask(4);
        completeTasksCount++;
        CheckTaskComplition();
        PetrographicResearchDelete();
    }
    private void PetrographicResearchDelete()
    {
        foreach (MapArea ma in MapBoard.Instance.moduleListBlocked)
        {
            RockResearch rr = ma.gameplayObject.GetComponentInChildren<RockResearch>();
            if (rr != null)
            {
                rr.Clear();
            }
        }
    }
    private void MeasuringWaterSetUp()
    {
        waterMeasured = 0;
        foreach (MapArea ma in MapBoard.Instance.moduleListBlocked)
        {
            LakeInteractions li = ma.gameplayObject.GetComponentInChildren<LakeInteractions>();
            if (li != null)
            {
                li.MeasureWater();
            }
        }
    }
    public void MeasuringWaterDone()
    {
        waterMeasured++;
        CheckTaskCounter(5);
        if (waterMeasured == waterToMeasure)
        {
            CheckTask(5);
            completeTasksCount++;
            CheckTaskComplition();
            MeasuringWaterDelete();
        }
    }
    private void MeasuringWaterDelete()
    {
        foreach (MapArea ma in MapBoard.Instance.moduleListBlocked)
        {
            LakeInteractions li = ma.gameplayObject.GetComponentInChildren<LakeInteractions>();
            if (li != null)
            {
                li.MeasureWaterClear();
            }
        }
    }
    private void TakeTrashSetUp()
    {
        foreach (MapArea ma in MapBoard.Instance.moduleListBlocked)
        {
            LakeInteractions li = ma.gameplayObject.GetComponentInChildren<LakeInteractions>();
            if (li != null)
            {
                li.TakeTrash();
            }
        }
    }
    public void TakeTrashDone()
    {
        CheckTask(6);
        completeTasksCount++;
        CheckTaskComplition();
        TakeTrashDelete();
    }
    private void TakeTrashDelete()
    {
        foreach (MapArea ma in MapBoard.Instance.moduleListBlocked)
        {
            LakeInteractions li = ma.gameplayObject.GetComponentInChildren<LakeInteractions>();
            if (li != null)
            {
                li.TakeTrashClear();
            }
        }
    }
    private void TrailCamSetUp()
    {
        allTrailCam.Clear();
        MapBoard.Instance.EmptyModuleList();
        trailCamInstalled = 0;
        for (int i = 0; i < trailCamToSpawn; i++)
        {
            int index = Random.Range(0, MapBoard.Instance.moduleListEmpty.Count);
            MapArea area = MapBoard.Instance.moduleListEmpty[index];
            area.taskIndex = 7;
            GameObject gm = Instantiate(trailCamPrefabs[Random.Range(0, trailCamPrefabs.Length)],
                area.gameplayObject.transform, worldPositionStays: false) as GameObject;
            gm.GetComponent<TrailCam>().module = area;
            allTrailCam.Add(gm.GetComponent<TrailCam>());
            MapBoard.Instance.moduleListEmpty.Remove(MapBoard.Instance.moduleListEmpty[index]);
        }
    }
    public void TrailCamDone(MapArea area)
    {
        trailCamInstalled++;
        area.taskIndex = 0;
        CheckTaskCounter(7);
        if (trailCamInstalled == trailCamToInstall)
        {
            CheckTask(7);
            completeTasksCount++;
            CheckTaskComplition();
        }
    }
    private void TrailCamDelete()
    {
        foreach (TrailCam tc in allTrailCam)
        {
            tc.module.taskIndex = 0;
            Destroy(tc.gameObject);
        }
    }
    public void CheckTaskComplition()
    {
        switch (completeTasksCount)
        {
            case 1:
                GetReward(currentTaskRewards[0].taskIndex);
                break;
            case 2:
                GetReward(currentTaskRewards[1].taskIndex);
                break;
            case 3:
                GetReward(currentTaskRewards[2].taskIndex);
                break;
            default: break;
        }
    }
    private void GetReward(int rewardIndex)
    {
        switch(rewardIndex)
        {
            case 1:
                RewardResource(1);
                break;
            case 2:
                RewardResource(2);
                break;
            case 3:
                RewardResource(3);
                break;
            case 4:
                GameManager.Instance.gameIndicator -= indicatorRewardAmount;
                if(GameManager.Instance.gameIndicator < 0) GameManager.Instance.gameIndicator = 0;
                GameManager.Instance.GetCurrentFillIndicator();
                break;
            default: break;
        }
    }
    private void RewardResource(int type)
    {
        switch (type)
        {
            case 1:
                PlayerInventory.Instance.woodAmount += resourceRewardAmount;
                PlayerInventory.Instance.woodAmountTXT.text = $"{PlayerInventory.Instance.woodAmount}/{PlayerInventory.Instance.maxWoodAmount}";
                break;
            case 2:
                PlayerInventory.Instance.stoneAmount += resourceRewardAmount;
                PlayerInventory.Instance.stoneAmountTXT.text = $"{PlayerInventory.Instance.stoneAmount}/{PlayerInventory.Instance.maxStoneAmount}";
                break;
            case 3:
                PlayerInventory.Instance.ropeAmount += resourceRewardAmount;
                PlayerInventory.Instance.ropeAmountTXT.text = $"{PlayerInventory.Instance.ropeAmount}/{PlayerInventory.Instance.maxRopeAmount}";
                break;
            case 4:
                //PlayerInventory.Instance.meatAmount += resourceRewardAmount;
                //PlayerInventory.Instance.meatAmountTXT.text = $"{PlayerInventory.Instance.meatAmount}/{PlayerInventory.Instance.maxMeatAmount}";
                break;
            default: break;
        }
    }

    ///////////////////////   UI
    private void UISetUp()
    {
        for(int i=0; i<todayTasksList.Count; i++)
        {
            TaskUISet(todayTasksList[i], tasksUI[i]);
            RewardUISet(currentTaskRewards[i], rewardsUI[i]);
        }
    }
    private void TaskUISet(DailyTaskSO task, DailyTasksUI ui)
    {
        ui.title.text = task.taskName;
        ui.description.text = task.taskDescription;
        ui.icon.sprite = task.taskIcon;
        ui.tick.SetActive(false);
        ui.currentTaskIndex = task.taskIndex;
        TaskCounter(task.taskIndex, ui);
    }
    private void RewardUISet(DailyTaskSO reward, DailyTasksUI ui)
    {
        ui.title.text = "Reward:";
        ui.description.text = reward.taskDescription;
        if(reward.taskIcon != null) ui.icon.sprite = reward.taskIcon;
    }
    private void TaskCounter(int taskIndex, DailyTasksUI ui)
    {
        switch (taskIndex)
        {
            case 1: // leaves
                ui.counter.text = $"[{leavesCleaned}/{leavesToClean}]";
                break;
            case 5: // water level
                ui.counter.text = $"[{waterMeasured}/{waterToMeasure}]";
                break;
            case 7: // cameras
                ui.counter.text = $"[{trailCamInstalled}/{trailCamToInstall}]";
                break;
            default:
                ui.counter.text = "";
                break;
        }
    }
    private void CheckTaskCounter(int index)
    {
        foreach(DailyTasksUI ui in tasksUI)
        {
            if(index == ui.currentTaskIndex)
            {
                TaskCounter(index, ui);
            }
        }
    }
    private void CheckTask(int index)
    {
        foreach(DailyTasksUI ui in tasksUI)
        {
            if(ui.currentTaskIndex == index)
            {
                ui.tick.SetActive(true);
            }
        }
    }
}
