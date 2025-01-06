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
        woodAmount = 0;
        woodAmountTXT.text = $"Wood: {woodAmount}";
    }

    public void CollectWood()
    {
        woodAmount += woodCollect;
        woodAmountTXT.text = $"Wood: {woodAmount}";
    }
}
