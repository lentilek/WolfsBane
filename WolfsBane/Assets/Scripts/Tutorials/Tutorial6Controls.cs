using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial6Controls : MonoBehaviour
{
    //private int currentPart;
    [SerializeField] private MapArea trapArea;
    [SerializeField] private GameObject part1UI, part2UI, part3UI, part4UI, part5UI, part6UI, part7UI;
    private void Awake()
    {
        //currentPart = 1;
        part1UI.SetActive(false);
        part2UI.SetActive(false);
        part3UI.SetActive(false);
        part4UI.SetActive(false);
        part5UI.SetActive(false);
        part6UI.SetActive(false);
        part7UI.SetActive(false);
    }
    private void Start()
    {
        PlayerInventory.Instance.woodAmount = 3;
        PlayerInventory.Instance.stoneAmount = 1;
        StartCoroutine(Night());
    }
    private void Update()
    {
        if (part3UI.activeSelf) part2UI.SetActive(false);
    }

    IEnumerator Night()
    {
        yield return new WaitForSeconds(0.1f);
        int i = 0;
        PlayerInventory.Instance.BuildTrap(i, trapArea);
        PlayerInventory.Instance.BuildHouseTrap();
        MapBoard.Instance.map[PlayerControler.Instance.row].moduleRow[PlayerControler.Instance.column].noAPTip.SetActive(false);
        PlayerControler.Instance.ButtonsAroundOff();
        GameManager.Instance.nightButton.SetActive(false);
        GameUI.Instance.Night();
        GameManager.Instance.Light();
        yield return new WaitForSeconds(0.1f);
        part1UI.SetActive(true);
    }
    public void Part1Button()
    {
        //currentPart = 2;
        part1UI.SetActive(false);
        part2UI.SetActive(true);
        GameManager.Instance.FinishDayStartNight();
    }
    public void Part2Button()
    {
        //currentPart = 3;
        part2UI.SetActive(false);
        part3UI.SetActive(true);
    }
    public void Part3Button()
    {
        //currentPart = 4;
        part2UI.SetActive(false);
        part3UI.SetActive(false);
        part4UI.SetActive(true);
    }
    public void Part4Button()
    {
        //currentPart = 5;
        part4UI.SetActive(false);
        part5UI.SetActive(true);
    }
    public void Part5Button()
    {
        //currentPart = 6;
        part5UI.SetActive(false);
        part6UI.SetActive(true);
    }
    public void Part6Button()
    {
        //currentPart = 7;
        part6UI.SetActive(false);
        part7UI.SetActive(true);
        PlayerPrefs.SetInt("Tutorials", 1);
    }
    public void GoToScene(int index)
    {
        SceneManager.LoadScene(index);
    }
}
