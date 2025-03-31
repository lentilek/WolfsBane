using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapArea : MonoBehaviour
{
    public int type; // 0 - out of map, 1 - regular, 2 - resource, 3 - blocked, 4 - house
    public int resourceType = 0; // 0 - nothing, 1 - wood, 2 - stone, 3 - rope, 4 - meat
    public int state = 2; // 0 - not avaiable, 1 - empty and trap, 2 - empty, 3 - smell&trap,
                          // 4 - smell, 5 - turist&trap, 6 - turist, 7 - meat
    public int taskIndex = 0; // 0 - nothing, 1-7 tasks
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
    [SerializeField] private GameObject[] emptyModels;
    [SerializeField] private GameObject[] resourceModelsWood;
    [SerializeField] private GameObject[] resourceModelsStone;
    [SerializeField] private GameObject[] resourceModelsRope;
    public GameObject smellVFX;
    public GameObject noActionTip;
    public GameObject noAPTip;

    private void Awake()
    {
        buttonDiscover.SetActive(false);
        buttonGo.SetActive(false);
        buttonAction.SetActive(false);
        smellVFX.SetActive(false);
        noActionTip.SetActive(false);
        noAPTip.SetActive(false);
    }
    private void Update()
    {
        if(isVisible && (state == 3 || state == 4) && !smellVFX.activeSelf)
        {
            smellVFX.SetActive(true);
        }else if(!isVisible || (state != 3 && state != 4))
        {
            smellVFX.SetActive(false);
        }
    }
    public void AddEnviro(int blockedType)
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
            case 1:
                Instantiate(emptyModels[Random.Range(0, emptyModels.Length)], decorations.transform, worldPositionStays: false); 
                break;
            case 2:
                AddResources();
                break;
            case 3:
                if (blockedType == 1) Instantiate(blockedModels[Random.Range(0, 3)], gameplayObject.transform, worldPositionStays: false);
                else if (blockedType == 2) Instantiate(blockedModels[Random.Range(3, 7)], gameplayObject.transform, worldPositionStays: false);
                else
                {
                    Instantiate(blockedModels[MapBoard.Instance._random.NextInt(0, blockedModels.Length)],
                        gameplayObject.transform, worldPositionStays: false);
                }
                break;
            default:
                break;
        }
    }
    private void AddResources()
    {
        switch (resourceType)
        {
            case 1:
                Instantiate(resourceModelsWood[MapBoard.Instance._random.NextInt(0, resourceModelsWood.Length)],
                    gameplayObject.transform, worldPositionStays: false);
                break;
            case 2:
                Instantiate(resourceModelsStone[MapBoard.Instance._random.NextInt(0, resourceModelsStone.Length)],
                    gameplayObject.transform, worldPositionStays: false);
                break;
            case 3:
                Instantiate(resourceModelsRope[MapBoard.Instance._random.NextInt(0, resourceModelsRope.Length)],
                    gameplayObject.transform, worldPositionStays: false);
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
            //Debug.Log(n.state);
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
            if(((type == 2 || state == 5 || state == 6 || taskIndex != 0) && GameManager.Instance.currentActionPoints > 0) || resourceType == 4)
            {
                buttonInteract.SetActive(true);
            }
            else
            {
                buttonInteract.SetActive(false);
            }
            if((((type == 1 || type == 2) && (state == 2 || state == 4 || state == 6) && 
                PlayerInventory.Instance.trapPrefab.GetComponent<Trap>().CanUBuild()) || 
                (type == 4 && PlayerInventory.Instance.woodAmount > 0 && !PlayerInventory.Instance.fenceTrap)) && taskIndex != 3 &&
                (GameManager.Instance.currentActionPoints > 0 || type == 4))
            {
                buttonSetTrap.SetActive(true);
            }
            else
            {
                buttonSetTrap.SetActive(false);
            }
            if (GameManager.Instance.currentActionPoints == 0 && type != 4)
            {
                noAPTip.SetActive(true);
            }
            else if (!buttonInteract.activeSelf && !buttonSetTrap.activeSelf)
            {
                noAPTip.SetActive(false);
                noActionTip.SetActive(true);
            }
            else
            {
                noActionTip.SetActive(false);
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
        if(type == 2)
        {
            ResourceRegeneration rr = gameplayObject.GetComponentInChildren<ResourceRegeneration>();
            switch (resourceType)
            {
                case 1:
                    if(PlayerInventory.Instance.IsThereInventorySpace(1) && rr.roundsToRegenerateLeft == 0 && GameManager.Instance.UseActionPoint())
                    {
                        AudioManager.Instance.PlaySound("collect");
                        PlayerInventory.Instance.CollectWood();
                        rr.StartRegeneration();
                    }
                    else
                    {
                        Debug.Log("No inventory space");
                    }
                    break;
                case 2:
                    if (PlayerInventory.Instance.IsThereInventorySpace(2) && rr.roundsToRegenerateLeft == 0 && GameManager.Instance.UseActionPoint())
                    {
                        AudioManager.Instance.PlaySound("collect");
                        PlayerInventory.Instance.CollectStone();
                        rr.StartRegeneration();
                    }
                    else
                    {
                        Debug.Log("No inventory space");
                    }
                    break;
                case 3:
                    if (PlayerInventory.Instance.IsThereInventorySpace(3) && rr.roundsToRegenerateLeft == 0 && GameManager.Instance.UseActionPoint())
                    {
                        AudioManager.Instance.PlaySound("collect");
                        PlayerInventory.Instance.CollectRope();
                        rr.StartRegeneration();
                    }
                    else
                    {
                        Debug.Log("No inventory space");
                    }
                    break;
                default:
                    break;
            }
        }
        else if((state == 6 || state == 5))
        {
            Dialog.Instance.TuristInteract(this);
        }
        else if(taskIndex != 0)
        {
            switch(taskIndex)
            {
                case 1:
                    gameplayObject.GetComponentInChildren<Leaves>().LeavesMiniGame(this);
                    break;
                case 2:
                    if(PlayerInventory.Instance.woodAmount > 0)
                    {
                        gameplayObject.GetComponentInChildren<SaltCubes>().SaltCubesMiniGame(this);
                    }
                    break;
                case 3:
                    if(GameManager.Instance.UseActionPoint())
                    {
                        gameplayObject.GetComponentInChildren<ClearPath>().ClearPathMiniGame(this);
                    }
                    break;
                case 7:
                    if(PlayerInventory.Instance.ropeAmount > 0)
                    {
                        gameplayObject.GetComponentInChildren<TrailCam>().TrailCamMiniGame(this);
                    }
                    break;
                default: break;
            }
        }
        else if(resourceType == 4)
        {
            if(PlayerInventory.Instance.chickensAmount > 0)
            {
                PlayerInventory.Instance.CollectMeat();
            }
        }
        buttonInteract.SetActive(false);
        buttonSetTrap.SetActive(false);
        buttonAction.SetActive(true);
        PlayerControler.Instance.ButtonsAroundOff();
        PlayerControler.Instance.ButtonsAround();
    }
    public void SetTrapButton()
    {
        if((type == 1 || type == 2) && (state == 2 || state == 4 || state == 6) && 
            PlayerInventory.Instance.trapPrefab.GetComponent<Trap>().CanUBuild() && 
            GameManager.Instance.UseActionPoint())
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
        PlayerControler.Instance.ButtonsAroundOff();
        PlayerControler.Instance.ButtonsAround();
    }
    public void GoHereButton()
    {
        if (type == 4 || GameManager.Instance.UseActionPoint())
        {
            PlayerControler.Instance.ButtonsAroundOff();
            PlayerControler.Instance.MovePlayer(row, column, this.transform.position);
            PlayerControler.Instance.ButtonsAround();
        }
        buttonGo.SetActive(false);
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
        buttonDiscover.SetActive(false);
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
        buttonDiscover.SetActive(false);
    }
    public void UISound()
    {
        AudioManager.Instance.PlaySound("uiSound");
    }
}
