using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turist : MonoBehaviour
{
    public float gameIndicatorWhenKilled;
    public float gameIndicatorWhenLived;
    public float gameIndicatorWhenScared;
    [HideInInspector] public MapArea mapModule;
    public GameObject fireVFX;

    private void Awake()
    {
        fireVFX.SetActive(false);
    }
    private void Update()
    {
        if (GameManager.Instance.isNight)
        {
            fireVFX.SetActive(true);
        }
        else
        {
            fireVFX.SetActive(false);
        }
    }
    public void GetEaten()
    {
        GameManager.Instance.gameIndicator += gameIndicatorWhenKilled;
        this.gameObject.SetActive(false);
    }
}
