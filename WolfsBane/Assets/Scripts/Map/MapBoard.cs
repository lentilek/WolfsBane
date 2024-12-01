using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AreaRow
{
    public MapArea[] moduleRow;
}
public class MapBoard : MonoBehaviour
{
    public static MapBoard Instance;

    public AreaRow[] map;
 
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
        for(int i = 0; i < map.Length; i++)
        {
            for(int j = 0; j < map.Length; j++)
            {
                map[i].moduleRow[j].row = i;
                map[i].moduleRow[j].column = j;
            }
        }
    }
}
