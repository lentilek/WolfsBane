using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial3Controler : MonoBehaviour
{
    private int currentPart;
    [SerializeField] private MapArea area1, area2;
    [SerializeField] private GameObject pm, part1UI, part2UI, part3UI, part4UI, part5UI, part6UI;
    [SerializeField] private GameObject options, talk, dialogueBox;
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
        if(currentPart != 3)
        {
            area2.buttonAction.SetActive(false);
        }
        if (currentPart == 4 && !talk.activeSelf && !dialogueBox.activeSelf)
        {
            Part4Button();
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
        area2.buttonAction.SetActive(true);
    }
    public void Part3Button()
    {
        pm.SetActive(true);
        part3UI.SetActive(false);
        part4UI.SetActive(true);
        area2.buttonAction.SetActive(false);
        currentPart = 4;
        options.SetActive(false);
        talk.SetActive(true);
    }
    public void Part4Button()
    {
        currentPart = 5;
        part4UI.SetActive(false);
        part5UI.SetActive(true);
        talk.SetActive(false);
    }
    public void Part5Button()
    {
        currentPart = 6;
        part5UI.SetActive(false);
        part6UI.SetActive(true);
        PlayerPrefs.SetInt("Tutorials", 1);
    }
    public void GoToScene(int index)
    {
        SceneManager.LoadScene(index);
    }
}
