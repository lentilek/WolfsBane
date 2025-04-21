using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial2Controler : MonoBehaviour
{
    private int currentPart;
    [SerializeField] private MapArea area;
    [SerializeField] private GameObject dailyTasks, part1UI, part2UI, part3UI, part4UI;
    private RockResearch rr;
    private void Awake()
    {
        currentPart = 1;
        part1UI.SetActive(true);
        part2UI.SetActive(false);
        part3UI.SetActive(false);
        part4UI.SetActive(false);
    }
    private void Update()
    {
        GameManager.Instance.nightButton.SetActive(false);
        area.buttonAction.SetActive(false);
        if (currentPart == 1)
        {
            rr = FindObjectOfType<RockResearch>();
            PlayerInventory.Instance.woodAmount = 3;
            PlayerInventory.Instance.woodAmountTXT.text = $"{PlayerInventory.Instance.woodAmount}/{PlayerInventory.Instance.maxWoodAmount}";
        }
        rr.module.buttonAction.SetActive(false);
        if (currentPart != 3)
        {
            rr.rockSample.SetActive(false);
        }
        else if(currentPart == 3 && PlayerInventory.Instance.stoneAmount > 0)
        {
            Part3Button();
        }
    }
    public void Part1Button()
    {
        currentPart = 2;
        part1UI.SetActive(false);
        part2UI.SetActive(true);
        dailyTasks.SetActive(true);
    }
    public void Part2Button()
    {
        part2UI.SetActive(false);
        part3UI.SetActive(true);
        currentPart = 3;
        rr.rockSample.SetActive(true);
    }
    public void Part3Button()
    {
        part3UI.SetActive(false);
        part4UI.SetActive(true);
        currentPart = 4;
        PlayerPrefs.SetInt("Tutorials", 1);
    }
    public void GoToScene(int index)
    {
        SceneManager.LoadScene(index);
    }
}
