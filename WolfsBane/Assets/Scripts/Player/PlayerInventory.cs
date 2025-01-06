using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance;

    public int woodAmount;
    [SerializeField] private int woodCollect;
    [SerializeField] private TextMeshProUGUI woodAmountTXT;

    public bool doorTrap;
    public bool fenceTrap;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(Instance.gameObject);
            Instance = this;
        }
        doorTrap = false;
        fenceTrap = false;
        woodAmount = 0;
        woodAmountTXT.text = $"Wood: {woodAmount}";
    }

    public void CollectWood()
    {
        woodAmount += woodCollect;
        woodAmountTXT.text = $"Wood: {woodAmount}";
    }

    public void BuildHouseTrap()
    {
        woodAmount--;
        woodAmountTXT.text = $"Wood: {woodAmount}";
    }
    public void CheckHouseForTrap()
    {
        if (MapBoard.Instance.map[PlayerControler.Instance.row].moduleRow[PlayerControler.Instance.column].type == 4)
        {
            Waiting(1f);
            if(doorTrap)
            {
                GameManager.Instance.UseActionPointAI();
                doorTrap = false;
            }
            if(fenceTrap)
            {
                GameManager.Instance.UseActionPointAI();
                fenceTrap = false;
            }
            Waiting(1f);
        }
    }

    IEnumerator Waiting(float time)
    {
        yield return new WaitForSeconds(time);
    }
}
