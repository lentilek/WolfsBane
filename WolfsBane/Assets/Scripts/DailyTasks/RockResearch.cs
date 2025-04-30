using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class RockResearch : MonoBehaviour
{
    [SerializeField] public GameObject rockSample, buttonRockSample;
    [HideInInspector] public MapArea module;
    private void Awake()
    {
        rockSample.SetActive(false);
    }
    private void Update()
    {
        IsPlayerNear();
    }
    public void Prepare()
    {
        module.taskIndex = 4;
        rockSample.SetActive(true);
        buttonRockSample.SetActive(false);
    }
    public void MiniGame()
    {
        AudioManager.Instance.PlaySound("uiSound");
        if (IsPlayerNear() && GameManager.Instance.UseActionPoint())
        {
            if (PlayerInventory.Instance.stoneAmount < PlayerInventory.Instance.maxStoneAmount)
            {
                PlayerInventory.Instance.stoneAmount++;
                PlayerInventory.Instance.stoneAmountTXT.text = $"{PlayerInventory.Instance.stoneAmount}/{PlayerInventory.Instance.maxStoneAmount}";
            }
            Done();
        }
    }
    public void Done()
    {
        TaskManager.Instance.PetrograpthicResearchDone();
        PlayerControler.Instance.ButtonsAroundOff();
        PlayerControler.Instance.ButtonsAround();
    }
    public void Clear()
    {
        module.taskIndex = 0;
        rockSample.SetActive(false);
    }

    private bool IsPlayerNear()
    {
        foreach(MapArea ma in module.neighbours)
        {
            if(PlayerControler.Instance.row == ma.row && PlayerControler.Instance.column == ma.column && !GameManager.Instance.isNight)
            {
                if (rockSample.activeSelf) buttonRockSample.SetActive(true);
                return true;
            }
        }
        buttonRockSample.SetActive(false);
        return false;
    }
}
