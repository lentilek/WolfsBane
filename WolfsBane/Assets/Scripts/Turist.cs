using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turist : MonoBehaviour
{
    public float gameIndicatorWhenKilled;
    public float gameIndicatorWhenLived;

    public void GetEaten()
    {
        GameManager.Instance.gameIndicator += gameIndicatorWhenKilled;
        this.gameObject.SetActive(false);
    }
}
