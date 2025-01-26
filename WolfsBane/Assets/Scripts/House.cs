using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    public static House Instance;

    public GameObject doorTrap;
    public GameObject fenceTrap;

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
        doorTrap.SetActive(false);
        fenceTrap.SetActive(false);
    }
}
