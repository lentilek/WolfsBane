using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailCam : MonoBehaviour
{
    [HideInInspector] public MapArea module;
    [SerializeField] private GameObject trails;
    [SerializeField] private GameObject placedModels;
    private void Awake()
    {
        trails.SetActive(true);
        placedModels.SetActive(false);
    }
    public void TrailCamMiniGame(MapArea area)
    {
        PlayerInventory.Instance.ropeAmount--;
        PlayerInventory.Instance.ropeAmountTXT.text = $"{PlayerInventory.Instance.ropeAmount}/{PlayerInventory.Instance.maxRopeAmount}";
        TrailCamFinish(area);
    }
    private void TrailCamFinish(MapArea area)
    {
        placedModels.SetActive(true);
        TaskManager.Instance.TrailCamDone(area);
    }
}
