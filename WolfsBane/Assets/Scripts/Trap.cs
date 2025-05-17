using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [HideInInspector] public MapArea module;
    public int buildCostWood;
    public int buildCostStone;
    public int buildCostRope;
    public int buildCostMeat;
    public int trapType; // 0 - regular, 1 - meat, 2 - big meat
    public float chance;

    public void BuildCost()
    {
        PlayerInventory.Instance.woodAmount -= buildCostWood;
        PlayerInventory.Instance.woodAmountTXT.text = $"{PlayerInventory.Instance.woodAmount}/{PlayerInventory.Instance.maxWoodAmount}";
        if (buildCostWood != 0) GameUI.Instance.InventoryAnimation(1, $"-{buildCostWood}");

        PlayerInventory.Instance.stoneAmount -= buildCostStone;
        PlayerInventory.Instance.stoneAmountTXT.text = $"{PlayerInventory.Instance.stoneAmount}/{PlayerInventory.Instance.maxStoneAmount}";
        if (buildCostStone != 0) GameUI.Instance.InventoryAnimation(2, $"-{buildCostStone}");

        PlayerInventory.Instance.ropeAmount -= buildCostRope;
        PlayerInventory.Instance.ropeAmountTXT.text = $"{PlayerInventory.Instance.ropeAmount}/{PlayerInventory.Instance.maxRopeAmount}";
        if (buildCostRope != 0) GameUI.Instance.InventoryAnimation(3, $"-{buildCostRope}");

        PlayerInventory.Instance.meatAmount -= buildCostMeat;
        PlayerInventory.Instance.meatAmountTXT.text = $"{PlayerInventory.Instance.meatAmount}/{PlayerInventory.Instance.maxMeatAmount}";
        if (buildCostMeat != 0) GameUI.Instance.InventoryAnimation(4, $"-{buildCostMeat}");
    }
    public bool CanUBuild()
    {
        if(PlayerInventory.Instance.woodAmount >= buildCostWood && PlayerInventory.Instance.stoneAmount >= buildCostStone &&
            PlayerInventory.Instance.ropeAmount >= buildCostRope && PlayerInventory.Instance.meatAmount >= buildCostMeat)
        {
            return true;
        }
        return false;
    }
}
