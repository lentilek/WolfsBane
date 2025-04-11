using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ButtonsStatesObject
{
    public int index;
    public GameObject possibleActionButton;
    public GameObject notPossibleActionButton;
    public GameObject possibleTooltip;
    public GameObject notPossibleTooltip;
}
public class TrapsButtonsManager : MonoBehaviour
{
    [SerializeField] private ButtonsStatesObject[] buttons;
    private void Awake()
    {
        foreach(var button in buttons)
        {
            AllOf(button);
        }
    }
    private void AllOf(ButtonsStatesObject button)
    {
        button.possibleActionButton.SetActive(false);
        button.notPossibleActionButton.SetActive(false);
        button.possibleTooltip.SetActive(false);
        button.notPossibleActionButton.SetActive(false);
    }
    private void ActionPossible(ButtonsStatesObject button)
    {
        AllOf(button);
        button.possibleActionButton.SetActive(true);
    }
    private void ActionNotPossible(ButtonsStatesObject button)
    {
        AllOf(button);
        button.notPossibleActionButton.SetActive(true);
    }
    public void AllButtonsStates()
    {
        foreach(var button in buttons)
        {
            if (PlayerInventory.Instance.trapPrefab[button.index].GetComponent<Trap>().CanUBuild())
            {
                ActionPossible(button);
            }
            else
            {
                ActionNotPossible(button);
            }
        }
    }
}
