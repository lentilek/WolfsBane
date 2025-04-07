using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    public static PlayerControler Instance;

    [HideInInspector] public Vector3 playerPosition;
    [HideInInspector] public int row;
    [HideInInspector] public int column;
    public Vector3 homePosition;
    public int homeRow;
    public int homeColumn;
    public GameObject playerModel;
    [HideInInspector] public List<MapArea> areasToGo;
    private int rowLastAI, columnLastAI;
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
        playerModel.SetActive(true);
        playerPosition = homePosition;
        row = homeRow;
        column = homeColumn;
    }
    private void Start()
    {
        this.gameObject.transform.position = playerPosition;
        ButtonsAround();
    }
    private void Update()
    {
        if (MapBoard.Instance.map[row].moduleRow[column].type == 4 && !GameManager.Instance.isNight)
        {
            GameManager.Instance.nightButton.SetActive(true);
        }
        else if(GameManager.Instance.isNight || GameManager.Instance.currentActionPoints != 0)
        {
            GameManager.Instance.nightButton.SetActive(false);
        }
    }
    public void ButtonsAround()
    {
        areasToGo.Clear();
        if(GameManager.Instance.currentActionPoints > 0 || 
            MapBoard.Instance.map[row].moduleRow[column].type == 4)
        {
            MapBoard.Instance.map[row].moduleRow[column].buttonAction.SetActive(true);
        }
        else
        {
            MapBoard.Instance.map[row].moduleRow[column].noAPTip.SetActive(true);
        }
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
        if(areasToGo.Count > 1)
        {
            areasToGo.Remove(MapBoard.Instance.map[rowLastAI].moduleRow[columnLastAI]);
        }
        GetCurrentAIModule();
        CheckAreasUI();
    }
    public void GetCurrentAIModule()
    {
        rowLastAI = row;
        columnLastAI = column;
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
    private void VisibleButton(MapArea module)
    {
        if (module.isAvailable && 
            (GameManager.Instance.currentActionPoints > 0 || module.type == 4))
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
        MapBoard.Instance.map[row].moduleRow[column].buttonAction.SetActive(false);
        ButtonHide();
    }
    public void ButtonsAroundHide()
    {
        for (int i = 0; i < areasToGo.Count; i++)
        {
            areasToGo[i].buttonAction.SetActive(true);
            areasToGo[i].buttonDiscover.SetActive(false);
            areasToGo[i].buttonGo.SetActive(false);
        }
        MapBoard.Instance.map[row].moduleRow[column].buttonAction.SetActive(true);
        ButtonHide();
    }
    public void ButtonHide()
    {
        MapBoard.Instance.map[row].moduleRow[column].buttonsTraps.SetActive(false);
        MapBoard.Instance.map[row].moduleRow[column].buttonInteract.SetActive(false);
        MapBoard.Instance.map[row].moduleRow[column].buttonSetTrap.SetActive(false);
        MapBoard.Instance.map[row].moduleRow[column].noActionTip.SetActive(false);
        MapBoard.Instance.map[row].moduleRow[column].noAPTip.SetActive(false);
    }
    public void MovePlayer(int rowNew, int columnNew, Vector3 position)
    {
        row = rowNew; 
        column = columnNew;
        playerPosition.x = position.x;
        playerPosition.z = position.z;

        PlayerMoveAnimation.Instance.MoveAnimation(playerPosition);
        //this.transform.position = playerPosition;
    }

    public void PlayerGoHome()
    {
        //MovePlayer(homeRow, homeColumn, homePosition);
        row = homeRow;
        column = homeColumn;
        playerPosition.x = homePosition.x;
        playerPosition.z = homePosition.z;
        transform.position = playerPosition;
    }
}
