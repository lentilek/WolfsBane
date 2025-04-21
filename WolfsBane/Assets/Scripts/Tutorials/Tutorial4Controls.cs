using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial4Controls : MonoBehaviour
{
    private int currentPart;
    [SerializeField] private MapArea area1, area2, area3;
    [SerializeField] private GameObject part1UI, part2UI, part3UI, part4UI;
    private void Awake()
    {
        currentPart = 1;
        part1UI.SetActive(true);
        part2UI.SetActive(false);
        part3UI.SetActive(false);
        part4UI.SetActive(false);
        area1.buttonAction.SetActive(false);
        area2.buttonAction.SetActive(false);
    }
    private void Update()
    {
        GameManager.Instance.nightButton.SetActive(false);
        area1.buttonAction.SetActive(false);
        if (currentPart == 1)
        {
            PlayerInventory.Instance.woodAmount = 5;
            PlayerInventory.Instance.woodAmountTXT.text = $"{PlayerInventory.Instance.woodAmount}/{PlayerInventory.Instance.maxWoodAmount}";
            PlayerInventory.Instance.stoneAmount = 1;
            PlayerInventory.Instance.stoneAmountTXT.text = $"{PlayerInventory.Instance.stoneAmount}/{PlayerInventory.Instance.maxStoneAmount}";
        }
        if (currentPart != 3)
        {
            area2.buttonAction.SetActive(false);
            area3.buttonAction.SetActive(false);
        }
        else
        {
            if (area2.state == 6) area2.buttonAction.SetActive(false);
            else if (area2.state == 3) Part3Button();
            if (area3.state == 6) area3.buttonAction.SetActive(false);
            else if (area3.state == 3) Part3Button();
        }
    }
    public void Part1Button()
    {
        currentPart = 2;
        part1UI.SetActive(false);
        part2UI.SetActive(true);
    }
    public void Part2Button()
    {
        currentPart = 3;
        part2UI.SetActive(false);
        part3UI.SetActive(true);
        if(area2.state != 6) area2.buttonAction.SetActive(true);
        if(area3.state != 6) area3.buttonAction.SetActive(true);
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
