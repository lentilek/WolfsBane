using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial1Controler : MonoBehaviour
{
    private int currentPart; 
    [SerializeField] private MapArea area1, area2;
    [SerializeField] private GameObject backpack, part2UI, part3UI, part4UI;
    private void Start()
    {
        currentPart = 1;
        backpack.SetActive(false);
        part2UI.SetActive(false);
        part3UI.SetActive(false);
        part4UI.SetActive(false);
    }
    private void Update()
    {
        GameManager.Instance.nightButton.SetActive(false);
        area2.buttonAction.SetActive(false);
        if(currentPart == 1 || currentPart == 4)
        {
            area1.state = 1;
            area1.buttonAction.SetActive(false);
        }
        else
        {
            //area1.buttonAction.SetActive(true);
        }
    }
    public void Part1Button()
    {
        area1.buttonAction.SetActive(true);
        part2UI.SetActive(true);
        currentPart = 2;
    }
    public void Part2Button()
    {
        backpack.SetActive(true);
        currentPart = 3;
        part3UI.SetActive(true);
        part2UI.SetActive(false);
    }
    public void Part3Button()
    {
        area1.buttonAction.SetActive(false);
        currentPart = 4;
        part3UI.SetActive(false);
        part4UI.SetActive(true);
        PlayerPrefs.SetInt("Tutorials", 1);
    }
    public void GoToScene(int index)
    {
        SceneManager.LoadScene(index);
    }
}
