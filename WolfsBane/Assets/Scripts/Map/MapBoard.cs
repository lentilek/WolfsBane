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

    [SerializeField] private uint seed = 1;
    [HideInInspector] public Random _random;
    [HideInInspector] public List<MapArea> mapRandomBlocked = new List<MapArea>();
    [HideInInspector] public List<MapArea> mapRandomResource = new List<MapArea>();

    [SerializeField] private int resourceWoodAmount, resourceStoneAmount, resourceRopeAmount, blockedAmount;
 
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
        for(int i = 0; i < map.Length; i++)
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
        while (blockedAmount > 0)
        {
            MapArea ma = mapRandomBlocked[_random.NextInt(0, mapRandomBlocked.Count)];
            ma.type = 3;
            ma.AreasAround();
            ma.AddEnviro();
            mapRandomResource.Remove(ma);
            mapRandomBlocked.Remove(ma);
            foreach(MapArea m in ma.neighbours)
            {
                mapRandomBlocked.Remove(m);
            }
            blockedAmount--;
        }
        while(resourceWoodAmount > 0)
        {
            MapArea ma = mapRandomResource[_random.NextInt(0, mapRandomResource.Count)];
            ma.type = 2;
            ma.resourceType = 1;
            ma.AreasAround();
            ma.AddEnviro();
            mapRandomResource.Remove(ma);
            resourceWoodAmount--;
        }
        while (resourceStoneAmount > 0)
        {
            MapArea ma = mapRandomResource[_random.NextInt(0, mapRandomResource.Count)];
            ma.type = 2;
            ma.resourceType = 2;
            ma.AreasAround();
            ma.AddEnviro();
            mapRandomResource.Remove(ma);
            resourceStoneAmount--;
        }
        while (resourceRopeAmount > 0)
        {
            MapArea ma = mapRandomResource[_random.NextInt(0, mapRandomResource.Count)];
            ma.type = 2;
            ma.resourceType = 3;
            ma.AreasAround();
            ma.AddEnviro();
            mapRandomResource.Remove(ma);
            resourceRopeAmount--;
        }
        foreach (MapArea m in mapRandomResource)
        {
            m.type = 1;
            m.AreasAround();
            m.AddEnviro();
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
}
