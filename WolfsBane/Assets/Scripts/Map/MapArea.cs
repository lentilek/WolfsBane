using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapArea : MonoBehaviour
{
    public int type; // 0 - out of map, 1 - regular, 2 - resource, 3 - blocked
    public bool isAvailable;
    public bool isVisible = false;
    public GameObject cloud;
    public GameObject models;
    public GameObject buttonAction;
    public GameObject buttonGo;
    public GameObject buttonDiscover;
    [HideInInspector] public int row;
    [HideInInspector] public int column;
    [HideInInspector] public List<MapArea> neighbours;

    private void Awake()
    {
        buttonDiscover.SetActive(false);
        buttonGo.SetActive(false);
        buttonAction.SetActive(false);
        if(type == 3 || type == 0)
        {
            isAvailable = false;
            isVisible = true;
        }
        else
        {
            isAvailable = true;
        }
        if(!isVisible)
        {
            cloud.SetActive(true);
        }
        else
        {
            cloud.SetActive(false);
        }
    }
    private void Start()
    {
        AreasAround();
    }
    private void AreasAround()
    {
        neighbours.Clear();
        if ((column - 1) >= 0)
        {
            MapArea module = MapBoard.Instance.map[row].moduleRow[column - 1];
            neighbours.Add(module);
        }
        if ((column + 1) < MapBoard.Instance.map.Length)
        {
            MapArea module = MapBoard.Instance.map[row].moduleRow[column + 1];
            neighbours.Add(module);
        }
        if (row % 2 == 0)
        {
            if ((row - 1) >= 0)
            {
                MapArea module = MapBoard.Instance.map[row - 1].moduleRow[column];
                neighbours.Add(module);
            }
            if ((row - 1) >= 0 && (column + 1) < MapBoard.Instance.map.Length)
            {
                MapArea module = MapBoard.Instance.map[row - 1].moduleRow[column + 1];
                neighbours.Add(module);
            }
            if ((row + 1) < MapBoard.Instance.map.Length)
            {
                MapArea module = MapBoard.Instance.map[row + 1].moduleRow[column];
                neighbours.Add(module);
            }
            if ((row + 1) < MapBoard.Instance.map.Length && (column + 1) < MapBoard.Instance.map.Length)
            {
                MapArea module = MapBoard.Instance.map[row + 1].moduleRow[column + 1];
                neighbours.Add(module);
            }
        }
        else
        {
            if ((row - 1) >= 0 && (column - 1) >= 0)
            {
                MapArea module = MapBoard.Instance.map[row - 1].moduleRow[column - 1];
                neighbours.Add(module);
            }
            if ((row - 1) >= 0)
            {
                MapArea module = MapBoard.Instance.map[row - 1].moduleRow[column];
                neighbours.Add(module);
            }
            if ((row + 1) < MapBoard.Instance.map.Length && (column - 1) >= 0)
            {
                MapArea module = MapBoard.Instance.map[row + 1].moduleRow[column - 1];
                neighbours.Add(module);
            }
            if ((row + 1) < MapBoard.Instance.map.Length)
            {
                MapArea module = MapBoard.Instance.map[row + 1].moduleRow[column];
                neighbours.Add(module);
            }
        }
    }
    public bool AreThereHiddenNeighbours()
    {
        for(int i = 0; i < neighbours.Count; i++)
        {
            if (neighbours[i].isAvailable && !neighbours[i].isVisible)
            {
                return true;
            }
        }
        return false;
    }
    public void ActionButton()
    {
        PlayerControler.Instance.ButtonsAroundHide();
        buttonAction.SetActive(false);
        if (isAvailable && isVisible && !AreThereHiddenNeighbours())
        {
            buttonGo.SetActive(true);
        }
        else if (isAvailable)
        {
            buttonDiscover.SetActive(true);
        }
    }
    public void GoHere()
    {
        PlayerControler.Instance.ButtonsAroundOff();
        PlayerControler.Instance.MovePlayer(row, column, this.transform.position);
        PlayerControler.Instance.ButtonsAround();
    }
    public void DiscoverLeft()
    {
        isVisible = true;
        cloud.SetActive(false);
        PlayerControler.Instance.ButtonsAroundOff();
        for(int i = 0; i < neighbours.Count; i++)
        {
            if(row % 2 == 0 && neighbours[i].column <= column)
            {
                neighbours[i].isVisible = true;
                neighbours[i].cloud.SetActive(false);
            }
            else if(row % 2 == 1 && neighbours[i].column < column)
            {
                neighbours[i].isVisible = true;
                neighbours[i].cloud.SetActive(false);
            }
        }
        PlayerControler.Instance.MovePlayer(row, column, this.transform.position);
        PlayerControler.Instance.ButtonsAround();
    }
    public void DiscoverRight()
    {
        isVisible = true;
        cloud.SetActive(false);
        PlayerControler.Instance.ButtonsAroundOff();
        for (int i = 0; i < neighbours.Count; i++)
        {
            if (row % 2 == 0 && neighbours[i].column > column)
            {
                neighbours[i].isVisible = true;
                neighbours[i].cloud.SetActive(false);
            }
            else if (row % 2 == 1 && neighbours[i].column >= column)
            {
                neighbours[i].isVisible = true;
                neighbours[i].cloud.SetActive(false);
            }
        }
        PlayerControler.Instance.MovePlayer(row, column, this.transform.position);
        PlayerControler.Instance.ButtonsAround();
    }
}
