using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tooltip
{
    public int index;
    public GameObject tip;
}
public class MultiTooltipManager : MonoBehaviour
{
    [SerializeField] private Tooltip[] tooltips;
    private void Awake()
    {
        AllOf();
    }
    private void AllOf()
    {
        foreach (var tooltip in tooltips)
        {
            tooltip.tip.SetActive(false);
        }
    }
    public void TipOn(int index)
    {
        AllOf();
        foreach(var tooltip in tooltips)
        {
            if(tooltip.index == index)
            {
                tooltip.tip.SetActive(true);
            }
        }
    }
}
