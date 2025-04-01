using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceRegeneration : MonoBehaviour
{
    public int howManyRoundsToRegenerate;
    [HideInInspector] public int roundsToRegenerateLeft;
    [SerializeField] private GameObject regularStateModels;
    [SerializeField] private GameObject regenerateStateModels;
    private void Awake()
    {
        roundsToRegenerateLeft = 0;
        regularStateModels.SetActive(true);
        regenerateStateModels.SetActive(false);
    }
    public void StartRegeneration()
    {
        roundsToRegenerateLeft = howManyRoundsToRegenerate;
        regenerateStateModels.SetActive(true);
        regularStateModels.SetActive(false);
    }
    public void Regenerate()
    {
        if(roundsToRegenerateLeft > 0)
        {
            roundsToRegenerateLeft--;
            if(roundsToRegenerateLeft == 0)
            {
                regularStateModels.SetActive(true);
                regenerateStateModels.SetActive(false);
            }
        }
    }
}
