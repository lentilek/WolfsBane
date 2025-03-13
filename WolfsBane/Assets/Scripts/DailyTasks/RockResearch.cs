using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class RockResearch : MonoBehaviour
{
    [SerializeField] private GameObject buttonRockSample;
    [HideInInspector] public MapArea module;
    private void Awake()
    {
        buttonRockSample.SetActive(false);
    }
    public void Prepare()
    {
        module.taskIndex = 4;
        buttonRockSample.SetActive(true);
    }
    public void MiniGame()
    {
        AudioManager.Instance.PlaySound("uiSound");
        if (IsPlayerNear() && GameManager.Instance.UseActionPoint())
        {
            PlayerInventory.Instance.stoneAmount++;
            PlayerInventory.Instance.stoneAmountTXT.text = $"{PlayerInventory.Instance.stoneAmount}/{PlayerInventory.Instance.maxStoneAmount}";
            Done();
        }
    }
    public void Done()
    {
        TaskManager.Instance.PetrograpthicResearchDone();
    }
    public void Clear()
    {
        module.taskIndex = 0;
        buttonRockSample.SetActive(false);
    }

    private bool IsPlayerNear()
    {
        foreach(MapArea ma in module.neighbours)
        {
            if(PlayerControler.Instance.row == ma.row && PlayerControler.Instance.column == ma.column)
            {
                return true;
            }
        }
        return false;
    }
}
