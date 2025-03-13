using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearPath : MonoBehaviour
{
    [HideInInspector] public MapArea module;
    public void ClearPathMiniGame(MapArea area)
    {
        PlayerInventory.Instance.woodAmount++;
        PlayerInventory.Instance.woodAmountTXT.text = $"{PlayerInventory.Instance.woodAmount}/{PlayerInventory.Instance.maxWoodAmount}";
        ClearPathFinish(area);
    }
    private void ClearPathFinish(MapArea area)
    {
        TaskManager.Instance.allClearPath.Remove(this);
        TaskManager.Instance.ClearingThePathDone(area);
        Destroy(gameObject);
    }
}
