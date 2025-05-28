using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turist : MonoBehaviour
{
    public int type; // 0 - regular, 1 - thrill hunter, 2 - police
    public float gameIndicatorWhenAgrresiveTalk;
    public float gameIndicatorWhenKilled;
    public float gameIndicatorWhenLived;
    public float gameIndicatorWhenScared;
    public int friendlyTalks, aggresiveTalks, regularTalks;
    public int talkChanceFriendly;
    public int talkChanceAggresive;
    public int killChance;
    [HideInInspector] public MapArea mapModule;
    public GameObject fireVFX;

    [SerializeField] private Sprite[] turistsSprites;
    [HideInInspector] public Sprite turistSprite;

    private void Awake()
    {
        if (fireVFX != null) fireVFX.SetActive(false);
        turistSprite = turistsSprites[Random.Range(0, turistsSprites.Length)];
    }
    private void Update()
    {
        if (GameManager.Instance.isNight)
        {
            if (fireVFX != null) fireVFX.SetActive(true);
        }
        else
        {
            if (fireVFX != null) fireVFX.SetActive(false);
        }
    }
    public void GetEaten()
    {
        GameManager.Instance.gameIndicator += gameIndicatorWhenKilled;
        this.gameObject.SetActive(false);
    }
}
