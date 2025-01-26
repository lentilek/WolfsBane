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

    public GameObject trapPrefab;
    [HideInInspector]public List<GameObject> trapsList = new List<GameObject>();

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
    public void BuildTrap(MapArea ma)
    {
        woodAmount -= trapPrefab.GetComponent<Trap>().buildConst;
        woodAmountTXT.text = $"Wood: {woodAmount}";
        GameObject go = Instantiate(trapPrefab, ma.gameplayObject.transform, worldPositionStays: false);
        go.GetComponent<Trap>().module = ma;
        if(ma.state == 2)
        {
            ma.state = 1;
        }        
        else if(ma.state == 4)
        {
            ma.state = 3;
        }
        else if(ma.state == 6)
        {
            ma.state = 5;
        }
        trapsList.Add(go);
    }
    public bool CheckTrap(MapArea ma)
    {
        Trap trapComp = ma.gameplayObject.GetComponentInChildren<Trap>();
        if (trapComp != null)
        {
            GameObject trap = trapComp.gameObject;
            if (trapComp.module.state == 1 && GameManager.Instance.UseActionPointAI())
            {
                trapComp.module.state = 2;
                Destroy(trap);
                trapsList.Remove(trap);
            }
            else if (trapComp.module.state == 3 && GameManager.Instance.UseActionPointAI())
            {
                trapComp.module.state = 4;
                Destroy(trap);
                trapsList.Remove(trap);
            }
            else if (trapComp.module.state == 5 && GameManager.Instance.UseActionPointAI())
            {
                trapComp.module.state = 6;
                Destroy(trap);
                trapsList.Remove(trap);
            }
            return true;
        }
        return false;
    }
    public void DestroyAllTraps()
    {
        foreach (GameObject trap in trapsList)
        {
            Trap trapComponent = trap.GetComponent<Trap>();
            if(trapComponent.module.state == 1)
            {
                trapComponent.module.state = 2;
            }
            else if (trapComponent.module.state == 3)
            {
                trapComponent.module.state = 4;
            }
            else if(trapComponent.module.state == 5)
            {
                trapComponent.module.state = 6;
            }
            Destroy(trap);
        }
        trapsList.Clear();
    }
    public void BuildHouseTrap()
    {
        if (!doorTrap)
        {
            House.Instance.doorTrap.SetActive(true);
            doorTrap = true;
            woodAmount--;
            woodAmountTXT.text = $"Wood: {woodAmount}";
        }
        else if (!fenceTrap)
        {
            House.Instance.fenceTrap.SetActive(true);
            fenceTrap = true;
            woodAmount--;
            woodAmountTXT.text = $"Wood: {woodAmount}";
        }
    }
    public void CheckHouseForTrap()
    {
        StartCoroutine(Waiting(GameManager.Instance.actionWaitTimeAI));
    }

    IEnumerator Waiting(float time)
    {
        if (MapBoard.Instance.map[PlayerControler.Instance.row].moduleRow[PlayerControler.Instance.column].type == 4)
        {
            if (doorTrap)
            {
                yield return new WaitForSeconds(time);
                GameManager.Instance.UseActionPointAI();
                doorTrap = false;
                House.Instance.doorTrap.SetActive(false);
            }
            if (fenceTrap)
            {
                yield return new WaitForSeconds(time);
                GameManager.Instance.UseActionPointAI();
                fenceTrap = false;
                House.Instance.fenceTrap.SetActive(false);
            }
        }
        GameManager.Instance.StartNightWait();
    }
}
