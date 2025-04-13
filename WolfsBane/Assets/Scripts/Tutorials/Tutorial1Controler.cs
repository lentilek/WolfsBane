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
        foreach(AreaRow ar in MapBoard.Instance.map)
        {
            foreach(MapArea ma in ar.moduleRow)
            {
                ma.buttonAction.SetActive(false);
            }
        }
    }
    private void Update()
    {
        GameManager.Instance.nightButton.SetActive(false);
        if(currentPart == 2 || currentPart == 3)
        {
            area2.buttonAction.SetActive(false);
            area1.state = 1;
        }
        else if(currentPart == 4)
        {
            area1.buttonAction.SetActive(false);
            area2.buttonAction.SetActive(false);
        }
    }
    public void Part1Button()
    {
        area1.buttonAction.SetActive(true);
        area2.buttonAction.SetActive(false);
        currentPart = 2;
    }
    public void Part2Button()
    {
        area2.buttonAction.SetActive(false);
        backpack.SetActive(true);
        currentPart = 3;
        part3UI.SetActive(true);
        part2UI.SetActive(false);
    }
    public void Part3Button()
    {
        area1.buttonAction.SetActive(false);
        area2.buttonAction.SetActive(false);
        currentPart = 4;
        part3UI.SetActive(false);
        part4UI.SetActive(true);
    }
    public void GoToScene(int index)
    {
        SceneManager.LoadScene(index);
    }
}
