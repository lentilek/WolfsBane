using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AreaRow
{
    public MapArea[] moduleColumn;
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
    }
}
