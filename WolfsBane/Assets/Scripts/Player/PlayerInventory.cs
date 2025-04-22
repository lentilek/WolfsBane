using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance;

    public int woodAmount;
    public int maxWoodAmount;
    [SerializeField] private int woodCollect;
    public TextMeshProUGUI woodAmountTXT;

    public int stoneAmount;
    public int maxStoneAmount;
    [SerializeField] private int stoneCollect;
    public TextMeshProUGUI stoneAmountTXT;

    public int ropeAmount;
    public int maxRopeAmount;
    [SerializeField] private int ropeCollect;
    public TextMeshProUGUI ropeAmountTXT;

    public int meatAmount;
    public int maxMeatAmount;
    public int chickensAmount;
    [SerializeField] private int meatCollect;
    public TextMeshProUGUI meatAmountTXT;

    public bool doorTrap;
    public bool fenceTrap;

    public GameObject[] trapPrefab;
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
        woodAmountTXT.text = $"{woodAmount}/{maxWoodAmount}";
        stoneAmount = 0;
        stoneAmountTXT.text = $"{stoneAmount}/{maxStoneAmount}";
        ropeAmount = 0;
        ropeAmountTXT.text = $"{ropeAmount}/{maxRopeAmount}";
        meatAmount = 0;
        meatAmountTXT.text = $"{meatAmount}/{maxMeatAmount}";
    }

    public void CollectWood()
    {
        woodAmount += woodCollect;
        if (woodAmount > maxWoodAmount) woodAmount = maxWoodAmount;
        woodAmountTXT.text = $"{woodAmount}/{maxWoodAmount}";
    }
    public void CollectStone()
    {
        stoneAmount += stoneCollect;
        if (stoneAmount > maxStoneAmount) stoneAmount = maxStoneAmount;
        stoneAmountTXT.text = $"{stoneAmount}/{maxStoneAmount}";
    }
    public void CollectRope()
    {
        ropeAmount += ropeCollect;
        if (ropeAmount > maxRopeAmount) ropeAmount = maxRopeAmount;
        ropeAmountTXT.text = $"{ropeAmount}/{maxRopeAmount}";
    }
    public void CollectMeat()
    {
        chickensAmount--;
        meatAmount += meatCollect;
        if (meatAmount > maxMeatAmount) meatAmount = maxMeatAmount;
        meatAmountTXT.text = $"{meatAmount}/{maxMeatAmount}";
    }
    public bool IsThereInventorySpace(int resourceIndex)
    {
        switch(resourceIndex)
        {
            case 1:
                if(woodAmount < maxWoodAmount)
                {
                    return true;
                }
                break;
            case 2:
                if(stoneAmount < maxStoneAmount)
                {
                    return true;
                }
                break;
            case 3:
                if(ropeAmount < maxRopeAmount)
                {
                    return true;
                }
                break;
            case 4:
                if(meatAmount < maxMeatAmount)
                {
                    return true;
                }
                break;
            default:
                break;
        }
        return false;
    }
    public void BuildTrap(int trapType, MapArea ma)
    {
        AudioManager.Instance.PlaySound("trap");
        trapPrefab[trapType].GetComponent<Trap>().BuildCost();
        GameObject go = Instantiate(trapPrefab[trapType], ma.gameplayObject.transform, worldPositionStays: false);
        go.GetComponent<Trap>().module = ma;
        trapsList.Add(go);
        BuildTrapState(trapType, ma);
    }
    private void BuildTrapState(int trapType, MapArea ma)
    {
        if(trapType == 1 || trapType == 2)
        {
            ma.state = 7;
            if(trapType == 2)
            {
                foreach (MapArea n in ma.neighbours)
                {
                    if (n.state == 2) n.state = 4;
                    else if (n.state == 1) n.state = 3;
                }
            }
        }
        else if(trapType == 5)
        {
            return;
        }
        else if(ma.state == 2)
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
    }
    public bool CheckTrap(MapArea ma)
    {
        Trap trapComp = ma.gameplayObject.GetComponentInChildren<Trap>();
        if (trapComp != null && GameManager.Instance.UseActionPointAI())
        {
            if (trapComp.trapType == 3)
            {
                if (Random.Range(0, 100) < trapComp.chance) GameManager.Instance.UseActionPointAI();
            }
            else if (trapComp.trapType == 4) GameManager.Instance.UseActionPointAI();
            else if(trapComp.trapType == 5)
            {
                GameManager.Instance.UseActionPointAI();
                GameManager.Instance.UseActionPointAI();
            }
            GameObject trap = trapComp.gameObject;
            if (trapComp.module.state == 1)
            {
                trapComp.module.state = 2;
                trapsList.Remove(trap);
                Destroy(trap);
            }
            else if (trapComp.module.state == 3)
            {
                trapComp.module.state = 4;
                trapsList.Remove(trap);
                Destroy(trap);
            }
            else if (trapComp.module.state == 5)
            {
                trapComp.module.state = 6;
                trapsList.Remove(trap);
                Destroy(trap);
            }
            else if(trapComp.module.state == 7)
            {
                trapComp.module.state = 2;
                if (trapComp.trapType == 2)
                {
                    foreach (MapArea n in trapComp.module.neighbours)
                    {
                        if (n.state == 4 && !n.AreThereTuristsAround()) n.state = 2;
                        else if (n.state == 3 && !n.AreThereTuristsAround()) n.state = 1;
                    }
                }
                trapsList.Remove(trap);
                Destroy(trap);
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
            else if(trapComponent.module.state == 7)
            {
                trapComponent.module.state = 2;
                if(trapComponent.trapType == 2)
                {
                    foreach (MapArea n in trapComponent.module.neighbours)
                    {
                        if (n.state == 4 && !n.AreThereTuristsAround()) n.state = 2;
                        else if (n.state == 3 && !n.AreThereTuristsAround()) n.state = 1;
                    }
                }
            }
            Destroy(trap);
        }
        trapsList.Clear();
    }
    public void BuildHouseTrap()
    {
        if (!doorTrap)
        {
            AudioManager.Instance.PlaySound("trap");
            House.Instance.doorTrap.SetActive(true);
            doorTrap = true;
            woodAmount--;
            woodAmountTXT.text = $"{woodAmount}/{maxWoodAmount}";
        }
        else if (!fenceTrap)
        {
            AudioManager.Instance.PlaySound("trap");
            House.Instance.fenceTrap.SetActive(true);
            fenceTrap = true;
            woodAmount--;
            woodAmountTXT.text = $"{woodAmount}/{maxWoodAmount}";
        }
    }
    public void CheckHouseForTrap()
    {
        StartCoroutine(Waiting(GameManager.Instance.actionWaitTimeAI));
    }

    IEnumerator Waiting(float time)
    {
        if (MapBoard.Instance.map[PlayerControler.Instance.row].moduleRow[PlayerControler.Instance.column].type == 4 && 
            MapBoard.Instance.map[PlayerControler.Instance.row].moduleRow[PlayerControler.Instance.column].state == 1)
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
