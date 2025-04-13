using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = Unity.Mathematics.Random;

[System.Serializable]
public class AreaRow
{
    public MapArea[] moduleRow;
}
public class MapBoard : MonoBehaviour
{
    public static MapBoard Instance;

    public AreaRow[] map;

    [HideInInspector] public List<MapArea> moduleListRegular= new List<MapArea>();
    [HideInInspector] public List<MapArea> moduleListResource = new List<MapArea>();
    [HideInInspector] public List<MapArea> moduleListEmpty = new List<MapArea>();
    [HideInInspector] public List<MapArea> moduleListBlocked = new List<MapArea>();

    [SerializeField] private uint seed = 1;
    [HideInInspector] public Random _random;
    [HideInInspector] public List<MapArea> mapRandomBlocked = new List<MapArea>();
    [HideInInspector] public List<MapArea> mapRandomResource = new List<MapArea>();

    [SerializeField] private int resourceWoodAmount, resourceStoneAmount, resourceRopeAmount, blockedLakesAmount, blockedRocksAmount; // 3 lakes, 4 rocks prefabs
 
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
        _random = new Random(seed);
        mapRandomBlocked.Clear();
        mapRandomResource.Clear();
        moduleListResource.Clear();
        for (int i = 0; i < map.Length; i++)
        {
            for(int j = 0; j < map.Length; j++)
            {
                map[i].moduleRow[j].row = i;
                map[i].moduleRow[j].column = j;
                if (map[i].moduleRow[j].type != 0 && map[i].moduleRow[j].type != 4)
                {
                    mapRandomBlocked.Add(map[i].moduleRow[j]);
                    mapRandomResource.Add(map[i].moduleRow[j]);
                }
            }
        }
    }
    private void Start()
    {
        _random.InitState(seed);
        RandomMap();
    }
    public void RandomMap()
    {
        moduleListBlocked.Clear();
        while (blockedLakesAmount > 0)
        {
            MapArea ma = mapRandomBlocked[_random.NextInt(0, mapRandomBlocked.Count)];
            ma.type = 3;
            ma.AreasAround();
            ma.AddEnviro(1);
            moduleListBlocked.Add(ma);
            if(ma.gameplayObject.GetComponentInChildren<RockResearch>() != null)
            {
                ma.gameplayObject.GetComponentInChildren<RockResearch>().module = ma;
            }
            if(ma.gameplayObject.GetComponentInChildren<LakeInteractions>() != null)
            {
                ma.gameplayObject.GetComponentInChildren<LakeInteractions>().module = ma;
            }
            mapRandomResource.Remove(ma);
            mapRandomBlocked.Remove(ma);
            foreach(MapArea m in ma.neighbours)
            {
                mapRandomBlocked.Remove(m);
            }
            blockedLakesAmount--;
        }
        while (blockedRocksAmount > 0)
        {
            MapArea ma = mapRandomBlocked[_random.NextInt(0, mapRandomBlocked.Count)];
            ma.type = 3;
            ma.AreasAround();
            ma.AddEnviro(2);
            moduleListBlocked.Add(ma);
            if (ma.gameplayObject.GetComponentInChildren<RockResearch>() != null)
            {
                ma.gameplayObject.GetComponentInChildren<RockResearch>().module = ma;
            }
            if (ma.gameplayObject.GetComponentInChildren<LakeInteractions>() != null)
            {
                ma.gameplayObject.GetComponentInChildren<LakeInteractions>().module = ma;
            }
            mapRandomResource.Remove(ma);
            mapRandomBlocked.Remove(ma);
            foreach (MapArea m in ma.neighbours)
            {
                mapRandomBlocked.Remove(m);
            }
            blockedRocksAmount--;
        }
        while (resourceWoodAmount > 0)
        {
            MapArea ma = mapRandomResource[_random.NextInt(0, mapRandomResource.Count)];
            ma.type = 2;
            ma.resourceType = 1;
            ma.AreasAround();
            ma.AddEnviro(0);
            mapRandomResource.Remove(ma);
            resourceWoodAmount--;
            moduleListResource.Add(ma);
        }
        while (resourceStoneAmount > 0)
        {
            MapArea ma = mapRandomResource[_random.NextInt(0, mapRandomResource.Count)];
            ma.type = 2;
            ma.resourceType = 2;
            ma.AreasAround();
            ma.AddEnviro(0);
            mapRandomResource.Remove(ma);
            resourceStoneAmount--;
            moduleListResource.Add(ma);
        }
        while (resourceRopeAmount > 0)
        {
            MapArea ma = mapRandomResource[_random.NextInt(0, mapRandomResource.Count)];
            ma.type = 2;
            ma.resourceType = 3;
            ma.AreasAround();
            ma.AddEnviro(0);
            mapRandomResource.Remove(ma);
            resourceRopeAmount--;
            moduleListResource.Add(ma);
        }
        foreach (MapArea m in mapRandomResource)
        {
            m.type = 1;
            m.AreasAround();
            m.AddEnviro(0);
        }
        GameManager.Instance.NewDay();
    }
    public void RegularModuleList()
    {
        moduleListRegular.Clear();
        for(int i = 0; i < map.Length; i++)
        {
            for(int j = 0; j < map.Length; j++)
            {
                if (map[i].moduleRow[j].type == 1)
                {
                    moduleListRegular.Add(map[i].moduleRow[j]);
                }
            }
        }
    }

    public void EmptyModuleList()
    {
        moduleListEmpty.Clear();
        for (int i = 0; i < map.Length; i++)
        {
            for (int j = 0; j < map.Length; j++)
            {
                if (map[i].moduleRow[j].type == 1 && map[i].moduleRow[j].state != 0 &&
                    map[i].moduleRow[j].state != 5 && map[i].moduleRow[j].state != 6 && 
                    map[i].moduleRow[j].taskIndex == 0)
                {
                    moduleListEmpty.Add(map[i].moduleRow[j]);
                }
            }
        }
    }
}
