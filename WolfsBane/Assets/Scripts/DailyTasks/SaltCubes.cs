using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaltCubes : MonoBehaviour
{
    [HideInInspector] public MapArea module;
    [SerializeField] private GameObject placedModels;
    private void Awake()
    {
        placedModels.SetActive(false);
    }
    public void SaltCubesMiniGame(MapArea area)
    {
        //PlayerInventory.Instance.woodAmount--;
        //PlayerInventory.Instance.woodAmountTXT.text = $"{PlayerInventory.Instance.woodAmount}/{PlayerInventory.Instance.maxWoodAmount}";
        SaltCubesFinish(area);
    }
    private void SaltCubesFinish(MapArea area)
    {
        placedModels.SetActive(true);
        TaskManager.Instance.SaltCubesDone(area);
    }
}
