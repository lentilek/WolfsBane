using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialog : MonoBehaviour
{
    public static Dialog Instance;
    [SerializeField] private GameObject dialogOptions;
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
            if(turist != null && Random.Range(1, 101) <= turist.GetComponent<Turist>().talkChanceAggresive)
            {
                if (area.state == 6 && !area.AreThereTuristsAround()) area.state = 2;
                else if (area.state == 6 && area.AreThereTuristsAround()) area.state = 4;
                else if (area.state == 5 && !area.AreThereTuristsAround()) area.state = 1;
                else if (area.state == 5 && area.AreThereTuristsAround()) area.state = 3;
                GameManager.Instance.gameIndicator += turist.GetComponent<Turist>().gameIndicatorWhenScared;
                if (GameManager.Instance.gameIndicator > 100) GameManager.Instance.gameIndicator = 100;
                GameManager.Instance.GetCurrentFillIndicator();
                Destroy(turist);
                GameManager.Instance.turistCamps.Remove(turist);
                foreach (MapArea n in area.neighbours)
                {
                    if (n.state == 4 && !n.AreThereTuristsAround()) n.state = 2;
                    else if (n.state == 3 && !n.AreThereTuristsAround()) n.state = 1;
                }
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
            GameObject turist = null;
            foreach (GameObject turistCamp in GameManager.Instance.turistCamps)
            {
                if (turistCamp.GetComponent<Turist>().mapModule.row == area.row && turistCamp.GetComponent<Turist>().mapModule.column == area.column)
                {
                    turist = turistCamp;
                    break;
                }
            }
            if (turist != null && Random.Range(1, 101) <= turist.GetComponent<Turist>().talkChanceFriendly)
            {
                if (area.state == 6 && !area.AreThereTuristsAround()) area.state = 2;
                else if (area.state == 6 && area.AreThereTuristsAround()) area.state = 4;
                else if (area.state == 5 && !area.AreThereTuristsAround()) area.state = 1;
                else if (area.state == 5 && area.AreThereTuristsAround()) area.state = 3;
                GameManager.Instance.turistCamps.Remove(turist);
                foreach (MapArea n in area.neighbours)
                {
                    if (n.state == 4 && !n.AreThereTuristsAround()) n.state = 2;
                    else if (n.state == 3 && !n.AreThereTuristsAround()) n.state = 1;
                }
                Destroy(turist);
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
