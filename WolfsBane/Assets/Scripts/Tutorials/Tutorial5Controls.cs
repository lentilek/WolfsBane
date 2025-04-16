using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial5Controls : MonoBehaviour
{
    private int currentPart;
    [SerializeField] private MapArea area1, area2, area3;
    [SerializeField] private GameObject part1UI, part2UI, part3UI, part4UI, part5UI, part6UI;
    private void Awake()
    {
        currentPart = 1;
        part1UI.SetActive(true);
        part2UI.SetActive(false);
        part3UI.SetActive(false);
        part4UI.SetActive(false);
        part5UI.SetActive(false);
        part6UI.SetActive(false);
        area1.buttonAction.SetActive(false);
        area2.buttonAction.SetActive(false);
        area3.buttonAction.SetActive(false);
    }
    private void Update()
    {
        if(currentPart != 5)
        {
            GameManager.Instance.nightButton.SetActive(false);
        }
        area2.buttonAction.SetActive(false);
        area3.buttonAction.SetActive(false);
        if (currentPart == 1)
        {
            PlayerInventory.Instance.woodAmount = 1;
            PlayerInventory.Instance.woodAmountTXT.text = $"{PlayerInventory.Instance.woodAmount}/{PlayerInventory.Instance.maxWoodAmount}";
            GameManager.Instance.currentActionPoints = 0;
            GameManager.Instance.actionPointsTXT.text = $"{GameManager.Instance.currentActionPoints}/{GameManager.Instance.maxActionPoints}";
            GameUI.Instance.NightAPImage();
        }
        if (!(currentPart == 3 || currentPart == 4))
        {
            area1.buttonAction.SetActive(false);
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
        area1.buttonAction.SetActive(true);
    }
    public void Part3Button()
    {
        part3UI.SetActive(false);
        part4UI.SetActive(true);
        currentPart = 4;
    }
    public void Part4Button()
    {
        currentPart = 5;
        part4UI.SetActive(false);
        part5UI.SetActive(true);
        GameManager.Instance.nightButton.SetActive(true);
    }
    public void Part5Button()
    {
        currentPart = 6;
        part5UI.SetActive(false);
        part6UI.SetActive(true);
    }
    public void GoToScene(int index)
    {
        SceneManager.LoadScene(index);
    }
}
