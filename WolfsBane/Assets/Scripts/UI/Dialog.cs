using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialog : MonoBehaviour
{
    public static Dialog Instance;
    [SerializeField] private GameObject dialogOptions;
    [SerializeField] private int chance = 5;
    private MapArea area;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(Instance.gameObject);
            Instance = this;
        }
        dialogOptions.SetActive(false);
    }
    public void TuristInteract(MapArea areaNew)
    {        
        PlayerControler.Instance.ButtonsAroundOff();
        areaNew.buttonAction.SetActive(false);
        area = areaNew;
        dialogOptions.SetActive(true);
    }
    public void TuristAction1Button()
    {
        //
        if (GameManager.Instance.UseActionPoint())
        {
            GameObject turist = null;
            foreach (GameObject turistCamp in GameManager.Instance.turistCamps)
            {
                if (turistCamp.GetComponent<Turist>().mapModule.row == area.row && turistCamp.GetComponent<Turist>().mapModule.column == area.column)
                {
                    turist = turistCamp;
                    break;
                }
            }
            if (turist != null)
            {
                if (area.state == 6) area.state = 2;
                else if (area.state == 5) area.state = 1;
                GameManager.Instance.gameIndicator += turist.GetComponent<Turist>().gameIndicatorWhenScared;
                if (GameManager.Instance.gameIndicator > 100) GameManager.Instance.gameIndicator = 100;
                GameManager.Instance.GetCurrentFillIndicator();
                Destroy(turist);
                GameManager.Instance.turistCamps.Remove(turist);
            }
        }
        dialogOptions.SetActive(false);
        area = null;
        PlayerControler.Instance.ButtonsAround();
    }
    public void TuristAction2Button()
    {
        //
        if (GameManager.Instance.UseActionPoint())
        {
            if(Random.Range(1,11) <= chance)
            {
                GameObject turist = null;
                foreach (GameObject turistCamp in GameManager.Instance.turistCamps)
                {
                    if (turistCamp.GetComponent<Turist>().mapModule.row == area.row && turistCamp.GetComponent<Turist>().mapModule.column == area.column)
                    {
                        turist = turistCamp;
                        break;
                    }
                }
                if (turist != null)
                {
                    if (area.state == 6) area.state = 2;
                    else if (area.state == 5) area.state = 1;
                    Destroy(turist);
                    GameManager.Instance.turistCamps.Remove(turist);
                }
            }
        }
        dialogOptions.SetActive(false);
        area = null;
        PlayerControler.Instance.ButtonsAround();
    }
    public void TuristLeaveButton()
    {
        //
        dialogOptions.SetActive(false);
        area = null;
        PlayerControler.Instance.ButtonsAround();
    }
}