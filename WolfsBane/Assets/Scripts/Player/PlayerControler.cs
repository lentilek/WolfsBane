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
    public void AreasToGoAI()
    {
        areasToGo.Clear();
        if ((column - 1) >= 0)
        {
            MapArea module = MapBoard.Instance.map[row].moduleRow[column - 1];
            if (module.isAvailable) areasToGo.Add(module);
        }
        if ((column + 1) < MapBoard.Instance.map.Length)
        {
            MapArea module = MapBoard.Instance.map[row].moduleRow[column + 1];
            if (module.isAvailable) areasToGo.Add(module);
        }
        if (row % 2 == 0)
        {
            if ((row - 1) >= 0)
            {
                MapArea module = MapBoard.Instance.map[row - 1].moduleRow[column];
                if (module.isAvailable) areasToGo.Add(module);
            }
            if ((row - 1) >= 0 && (column + 1) < MapBoard.Instance.map.Length)
            {
                MapArea module = MapBoard.Instance.map[row - 1].moduleRow[column + 1];
                if (module.isAvailable) areasToGo.Add(module);
            }
            if ((row + 1) < MapBoard.Instance.map.Length)
            {
                MapArea module = MapBoard.Instance.map[row + 1].moduleRow[column];
                if (module.isAvailable) areasToGo.Add(module);
            }
            if ((row + 1) < MapBoard.Instance.map.Length && (column + 1) < MapBoard.Instance.map.Length)
            {
                MapArea module = MapBoard.Instance.map[row + 1].moduleRow[column + 1];
                if (module.isAvailable) areasToGo.Add(module);
            }
        }
        else
        {
            if ((row - 1) >= 0 && (column - 1) >= 0)
            {
                MapArea module = MapBoard.Instance.map[row - 1].moduleRow[column - 1];
                if (module.isAvailable) areasToGo.Add(module);
            }
            if ((row - 1) >= 0)
            {
                MapArea module = MapBoard.Instance.map[row - 1].moduleRow[column];
                if (module.isAvailable) areasToGo.Add(module);
            }
            if ((row + 1) < MapBoard.Instance.map.Length && (column - 1) >= 0)
            {
                MapArea module = MapBoard.Instance.map[row + 1].moduleRow[column - 1];
                if (module.isAvailable) areasToGo.Add(module);
            }
            if ((row + 1) < MapBoard.Instance.map.Length)
            {
                MapArea module = MapBoard.Instance.map[row + 1].moduleRow[column];
                if(module.isAvailable)areasToGo.Add(module);
            }
        }
        CheckAreasUI();
    }
    public void CheckAreasUI()
    {
        List<MapArea> areasToChoose = new List<MapArea>();
        areasToChoose.Clear();
        int topState = 0;
        foreach(MapArea module in areasToGo)
        {
            if(module.state == topState)
            {
                areasToChoose.Add(module);
            }else if(module.state > topState)
            {
                areasToChoose.Clear();
                areasToChoose.Add(module);
                topState = module.state;
            }
        }
        MapArea area = areasToChoose[Random.Range(0, areasToChoose.Count)];
        MovePlayer(area.row, area.column, area.gameObject.transform.position);
        area.isVisible = true;
        area.cloud.SetActive(false);
        area.models.SetActive(true);
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
