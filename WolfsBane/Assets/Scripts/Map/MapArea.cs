using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapArea : MonoBehaviour
{
    public int type; // 0 - out of map, 1 - regular, 2 - resource, 3 - blocked, 4 - house
    public int state = 2; // 0 - not avaiable, 1 - empty and trap, 2 - empty, 3 - smell&trap, 4 - smell, 5 - turist&trap, 6 - turist, 7 - meat
    public bool isAvailable;
    public bool isVisible;
    public GameObject cloud;
    public GameObject models;
    public GameObject gameplayObject;
    public GameObject decorations;
    public GameObject buttonAction;
    public GameObject buttonGo;
    public GameObject buttonDiscover;
    public GameObject buttonInteract;
    public GameObject buttonSetTrap;
    [HideInInspector] public int row;
    [HideInInspector] public int column;
    [HideInInspector] public List<MapArea> neighbours;

    [SerializeField] private GameObject[] blockedModels;
    [SerializeField] private GameObject[] resourceModels;

    private void Awake()
    {
        buttonDiscover.SetActive(false);
        buttonGo.SetActive(false);
        buttonAction.SetActive(false);
    }
    private void Start()
    {
        //AddEnviro();
        //AreasAround();
    }
    public void AddEnviro()
    {
        if (type == 3 || type == 0)
        {
            isAvailable = false;
            isVisible = true;
            state = 0;
            decorations.SetActive(false);
        }
        else if(type != 4)
        {
            isAvailable = true;
            state = 2;
            decorations.SetActive(true);
        }
        if (!isVisible)
        {
            cloud.SetActive(true);
            models.SetActive(false);
        }
        else
        {
            cloud.SetActive(false);
            models.SetActive(true);
        }

        switch (type)
        {
            case 2:
                Instantiate(resourceModels[MapBoard.Instance._random.NextInt(0, resourceModels.Length)], gameplayObject.transform, worldPositionStays: false);
                break;
            case 3:
                Instantiate(blockedModels[MapBoard.Instance._random.NextInt(0,blockedModels.Length)], gameplayObject.transform, worldPositionStays: false);
                break;
            default:
                break;
        }
    }
    public void AreasAround()
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
       foreach(MapArea n in neighbours)
       {
           if (n.isAvailable && !n.isVisible)
           {
               return true;
           }
       }
       return false;
    }
    public bool AreThereTuristsAround()
    {
        foreach(MapArea n in neighbours)
        {
            if(n.state == 5 || n.state == 6)
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
        if(PlayerControler.Instance.row == row && PlayerControler.Instance.column == column)
        {
            if(type == 2 || state == 5 || state == 6)
            {
                buttonInteract.SetActive(true);
            }
            if(((type == 1 || type == 2) && (state == 2 || state == 4 || state == 6) && PlayerInventory.Instance.woodAmount >= PlayerInventory.Instance.trapPrefab.GetComponent<Trap>().buildConst) || (type == 4 && PlayerInventory.Instance.woodAmount > 0))
            {
                buttonSetTrap.SetActive(true);
            }
        }
        else if (isAvailable && isVisible && !AreThereHiddenNeighbours())
        {
            buttonGo.SetActive(true);
        }
        else if (isAvailable)
        {
            buttonDiscover.SetActive(true);
        }
    }
    public void InteractButton()
    {
        if(type == 2 && GameManager.Instance.UseActionPoint())
        {
            PlayerInventory.Instance.CollectWood();
        }
        else if((state == 6 || state == 5))
        {
            Dialog.Instance.TuristInteract(this);
        }
        buttonInteract.SetActive(false);
        buttonSetTrap.SetActive(false);
        buttonAction.SetActive(true);
    }
    public void SetTrapButton()
    {
        if((type == 1 || type == 2) && (state == 2 || state == 4 || state == 6) && PlayerInventory.Instance.woodAmount >= PlayerInventory.Instance.trapPrefab.GetComponent<Trap>().buildConst && GameManager.Instance.UseActionPoint())
        {
            PlayerInventory.Instance.BuildTrap(MapBoard.Instance.map[row].moduleRow[column]);
        }
        else if(type == 4 && PlayerInventory.Instance.woodAmount > 0)
        {
            PlayerInventory.Instance.BuildHouseTrap();
        }
        buttonInteract.SetActive(false);
        buttonSetTrap.SetActive(false);
        buttonAction.SetActive(true);
    }
    public void GoHereButton()
    {
        if (type == 4 || GameManager.Instance.UseActionPoint())
        {
            PlayerControler.Instance.ButtonsAroundOff();
            PlayerControler.Instance.MovePlayer(row, column, this.transform.position);
            PlayerControler.Instance.ButtonsAround();
        }
    }
    public void DiscoverLeftButton()
    {
        if(type == 4 || GameManager.Instance.UseActionPoint())
        {
            isVisible = true;
            cloud.SetActive(false);
            models.SetActive(true);
            PlayerControler.Instance.ButtonsAroundOff();
            for(int i = 0; i < neighbours.Count; i++)
            {
                if(row % 2 == 0 && neighbours[i].column <= column)
                {
                    neighbours[i].isVisible = true;
                    neighbours[i].cloud.SetActive(false);
                    neighbours[i].models.SetActive(true);
                }
                else if(row % 2 == 1 && neighbours[i].column < column)
                {
                    neighbours[i].isVisible = true;
                    neighbours[i].cloud.SetActive(false);
                    neighbours[i].models.SetActive(true);
                }
            }
            PlayerControler.Instance.MovePlayer(row, column, this.transform.position);
            PlayerControler.Instance.ButtonsAround();
        }
    }
    public void DiscoverRightButton()
    {
        if(type == 4 || GameManager.Instance.UseActionPoint())
        {
            isVisible = true;
            cloud.SetActive(false);
            models.SetActive(true);
            PlayerControler.Instance.ButtonsAroundOff();
            for (int i = 0; i < neighbours.Count; i++)
            {
                if (row % 2 == 0 && neighbours[i].column > column)
                {
                    neighbours[i].isVisible = true;
                    neighbours[i].cloud.SetActive(false);
                    neighbours[i].models.SetActive(true);
                }
                else if (row % 2 == 1 && neighbours[i].column >= column)
                {
                    neighbours[i].isVisible = true;
                    neighbours[i].cloud.SetActive(false);
                    neighbours[i].models.SetActive(true);
                }
            }
            PlayerControler.Instance.MovePlayer(row, column, this.transform.position);
            PlayerControler.Instance.ButtonsAround();
        }
    }
}
