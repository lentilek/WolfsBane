using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    public static House Instance;

    public GameObject doorTrap;
    public GameObject fenceTrap;
    [SerializeField] private Material matDay, matNight;
    [SerializeField] private MeshRenderer mr;
    [SerializeField] private GameObject nightLight;

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
        nightLight.SetActive(false);
        mr.material = matDay;
    }
    public void HouseDay()
    {
        mr.material = matDay;
        nightLight.SetActive(false);
    }
    public void HouseNight()
    {
        mr.material = matNight;
        nightLight.SetActive(true);
    }
}
