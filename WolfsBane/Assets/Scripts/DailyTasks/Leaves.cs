using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaves : MonoBehaviour
{
    [HideInInspector] public MapArea module;
    // animation maybe ??
    // mini game
    public void LeavesMiniGame(MapArea area)
    {
        AudioManager.Instance.PlaySound("taskLeaves");
        FinishGame(area);
    }
    private void FinishGame(MapArea area)
    {
        TaskManager.Instance.allLeaves.Remove(this);
        TaskManager.Instance.LeafCleaningDone(area);
        Destroy(gameObject);
    }
}
