using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LakeInteractions : MonoBehaviour
{
    [SerializeField] private GameObject measureWater, buttonMeasureWater;
    [SerializeField] private GameObject takeTrash, buttonTakeTrash;
    [SerializeField] private GameObject trashModels;
    [HideInInspector] public MapArea module;
    private void Awake()
    {
        measureWater.SetActive(false);
        takeTrash.SetActive(false);
        trashModels.SetActive(false);
    }
    private void Update()
    {
        IsPlayerNear();
    }
    public void MeasureWater()
    {
        module.taskIndex = 5;
        measureWater.SetActive(true);
        buttonMeasureWater.SetActive(false);
    }
    public void MeasureWaterMiniGame()
    {
        AudioManager.Instance.PlaySound("uiSound");
        if (IsPlayerNear())
        {
            MeasureWaterDone();
        }
    }
    public void MeasureWaterDone()
    {
        MeasureWaterClear();
        TaskManager.Instance.MeasuringWaterDone();
        PlayerControler.Instance.ButtonsAroundOff();
        PlayerControler.Instance.ButtonsAround();
    }
    public void MeasureWaterClear()
    {
        module.taskIndex = 0;
        measureWater.SetActive(false);
    }
    public void TakeTrash()
    {
        module.taskIndex = 6;
        takeTrash.SetActive(true);
        buttonTakeTrash.SetActive(false);
        trashModels.SetActive(true);
    }
    public void TakeTrashMiniGame()
    {
        AudioManager.Instance.PlaySound("uiSound");
        if (IsPlayerNear())
        {
            if (PlayerInventory.Instance.ropeAmount < PlayerInventory.Instance.maxRopeAmount)
            {
                PlayerInventory.Instance.ropeAmount++;
                PlayerInventory.Instance.ropeAmountTXT.text = $"{PlayerInventory.Instance.ropeAmount}/{PlayerInventory.Instance.maxRopeAmount}";
            }
            TakeTrashDone();
        }
    }
    public void TakeTrashDone()
    {
        trashModels.SetActive(false);
        TaskManager.Instance.TakeTrashDone();
        PlayerControler.Instance.ButtonsAroundOff();
        PlayerControler.Instance.ButtonsAround();
    }
    public void TakeTrashClear()
    {
        trashModels.SetActive(false);
        module.taskIndex = 0;
        takeTrash.SetActive(false);
    }
    private bool IsPlayerNear()
    {
        foreach (MapArea ma in module.neighbours)
        {
            if (PlayerControler.Instance.row == ma.row && PlayerControler.Instance.column == ma.column && !GameManager.Instance.isNight)
            {
                if(measureWater.activeSelf) buttonMeasureWater.SetActive(true);
                if(takeTrash.activeSelf) buttonTakeTrash.SetActive(true);
                return true;
            }
        }
        buttonTakeTrash.SetActive(false);
        buttonMeasureWater.SetActive(false);
        return false;
    }
}
