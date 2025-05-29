using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class RockResearch : MonoBehaviour
{
    [SerializeField] public GameObject rockSample, buttonRockSample;
    [HideInInspector] public MapArea module;
    [SerializeField] private GameObject rockIcon;
    private void Awake()
    {
        rockSample.SetActive(false);
        rockIcon.SetActive(false);
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
        if (IsPlayerNear())
        {
            if (PlayerInventory.Instance.stoneAmount < PlayerInventory.Instance.maxStoneAmount)
            {
                PlayerInventory.Instance.stoneAmount++;
                PlayerInventory.Instance.stoneAmountTXT.text = $"{PlayerInventory.Instance.stoneAmount}/{PlayerInventory.Instance.maxStoneAmount}";
                GameUI.Instance.InventoryAnimation(2, $"+1");
            }
            AudioManager.Instance.PlaySound("taskRockResearch");
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
                rockIcon.SetActive(false);
                if (rockSample.activeSelf) buttonRockSample.SetActive(true);
                return true;
            }
        }
        if (rockSample.activeSelf) rockIcon.SetActive(true);
        else rockIcon.SetActive(false);
        buttonRockSample.SetActive(false);
        return false;
    }
    public void UISound()
    {
        AudioManager.Instance.PlaySound("uiSound");
    }
    public void UIHover()
    {
        //AudioManager.Instance.PlaySound("uiHover");
    }
}
