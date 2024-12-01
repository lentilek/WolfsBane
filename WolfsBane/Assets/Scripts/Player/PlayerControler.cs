using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    public static PlayerControler Instance;

    public Vector3 playerPosition;
    public int row;
    public int column;
    public GameObject playerModelDay;
    public GameObject playerModelNight;
    [HideInInspector] public List<MapArea> areasToGo;
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
        playerModelDay.SetActive(true);
        playerModelNight.SetActive(false);
    }
    private void Start()
    {
        this.gameObject.transform.position = playerPosition;
        ButtonsAround();
    }

    public void ButtonsAround()
    {
        areasToGo.Clear();
        if ((column - 1) >= 0)
        {
            MapArea module = MapBoard.Instance.map[row].moduleRow[column - 1];        
            VisibleButton(module);
        }
        if ((column + 1) < MapBoard.Instance.map.Length)
        {
            MapArea module = MapBoard.Instance.map[row].moduleRow[column + 1];
            VisibleButton(module);  
        }
        if(row % 2 == 0)
        {
            if ((row - 1) >= 0)
            {
                MapArea module = MapBoard.Instance.map[row - 1].moduleRow[column];
                VisibleButton(module);
            }
            if ((row - 1) >= 0 && (column + 1) < MapBoard.Instance.map.Length)
            {
                MapArea module = MapBoard.Instance.map[row - 1].moduleRow[column + 1];
                VisibleButton(module);
            }
            if ((row + 1) < MapBoard.Instance.map.Length)
            {
                MapArea module = MapBoard.Instance.map[row + 1].moduleRow[column];
                VisibleButton(module);
            }
            if ((row + 1) < MapBoard.Instance.map.Length && (column + 1) < MapBoard.Instance.map.Length)
            {
                MapArea module = MapBoard.Instance.map[row + 1].moduleRow[column + 1];
                VisibleButton(module);
            }
        }
        else
        {
            if ((row - 1) >= 0 && (column - 1) >= 0)
            {
                MapArea module = MapBoard.Instance.map[row - 1].moduleRow[column - 1];
                VisibleButton(module);
            }
            if ((row - 1) >= 0)
            {
                MapArea module = MapBoard.Instance.map[row - 1].moduleRow[column];
                VisibleButton(module);
            }
            if ((row + 1) < MapBoard.Instance.map.Length && (column - 1) >= 0)
            {
                MapArea module = MapBoard.Instance.map[row + 1].moduleRow[column - 1];
                VisibleButton(module);
            }
            if ((row + 1) < MapBoard.Instance.map.Length)
            {
                MapArea module = MapBoard.Instance.map[row + 1].moduleRow[column];
                VisibleButton(module);
            }
        }
    }
    /*private void VisibleButton(MapArea module)
    {
        if(module.isAvailable && module.isVisible && !module.AreThereHiddenNeighbours())
        {
            module.buttonGo.SetActive(true);
            areasToGo.Add(module);
        }else if(module.isAvailable)
        {
            module.buttonDiscover.SetActive(true);
            areasToGo.Add(module);
        }
    }*/
    private void VisibleButton(MapArea module)
    {
        if (module.isAvailable)
        {
            module.buttonAction.SetActive(true);
            areasToGo.Add(module);
        }
    }
    public void ButtonsAroundOff()
    {
        for (int i = 0; i < areasToGo.Count; i++)
        {
            areasToGo[i].buttonAction.SetActive(false);
            areasToGo[i].buttonDiscover.SetActive(false);
            areasToGo[i].buttonGo.SetActive(false);
            //Debug.Log("off");
        }
    }
    public void ButtonsAroundHide()
    {
        for (int i = 0; i < areasToGo.Count; i++)
        {
            areasToGo[i].buttonAction.SetActive(true);
            areasToGo[i].buttonDiscover.SetActive(false);
            areasToGo[i].buttonGo.SetActive(false);
        }
    }
    public void MovePlayer(int rowNew, int columnNew, Vector3 position)
    {
        row = rowNew; 
        column = columnNew;
        playerPosition.x = position.x;
        playerPosition.z = position.z;
        this.transform.position = playerPosition;
    }
}
