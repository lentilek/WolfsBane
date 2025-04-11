using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionButtonManager : MonoBehaviour
{
    [SerializeField] private GameObject possibleActionButton, notPossibleActionButton, possibleTooltip, notPossibleTooltip;

    private void Awake()
    {
        AllOf();
    }
    private void AllOf()
    {
        possibleActionButton.SetActive(false);
        notPossibleActionButton.SetActive(false);
        possibleTooltip.SetActive(false);
        notPossibleActionButton.SetActive(false);
    }
    public void ActionPossible()
    {
        AllOf();
        possibleActionButton.SetActive(true);
    }
    public void ActionNotPossible()
    {
        AllOf();
        notPossibleActionButton.SetActive(true);
    }
}
