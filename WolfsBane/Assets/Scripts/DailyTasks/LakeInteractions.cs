using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LakeInteractions : MonoBehaviour
{
    [SerializeField] private GameObject buttonMeasureWater;
    [SerializeField] private GameObject buttonTakeTrash;
    [SerializeField] private GameObject trashModels;
    [HideInInspector] public MapArea module;
    private void Awake()
    {
        buttonMeasureWater.SetActive(false);
        buttonTakeTrash.SetActive(false);
        trashModels.SetActive(false);
    }
    public void MeasureWater()
    {
        module.taskIndex = 5;
        buttonMeasureWater.SetActive(true);
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
    }
    public void MeasureWaterClear()
    {
        module.taskIndex = 0;
        buttonMeasureWater.SetActive(false);
    }
    public void TakeTrash()
    {
        module.taskIndex = 6;
        buttonTakeTrash.SetActive(true);
        trashModels.SetActive(true);
    }
    public void TakeTrashMiniGame()
    {
        AudioManager.Instance.PlaySound("uiSound");
        if (IsPlayerNear() && GameManager.Instance.UseActionPoint())
        {
            PlayerInventory.Instance.ropeAmount++;
            PlayerInventory.Instance.ropeAmountTXT.text = $"{PlayerInventory.Instance.ropeAmount}/{PlayerInventory.Instance.maxRopeAmount}";
            TakeTrashDone();
        }
    }
    public void TakeTrashDone()
    {
        trashModels.SetActive(false);
        TaskManager.Instance.TakeTrashDone();
    }
    public void TakeTrashClear()
    {
        trashModels.SetActive(false);
        module.taskIndex = 0;
        buttonTakeTrash.SetActive(false);
    }
    private bool IsPlayerNear()
    {
        foreach (MapArea ma in module.neighbours)
        {
            if (PlayerControler.Instance.row == ma.row && PlayerControler.Instance.column == ma.column)
            {
                return true;
            }
        }
        return false;
    }
}
