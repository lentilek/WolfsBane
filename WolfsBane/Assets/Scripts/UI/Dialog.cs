using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static UnityEngine.ParticleSystem;
using UnityEngine.SceneManagement;

public class Dialog : MonoBehaviour
{
    public static Dialog Instance;
    [SerializeField] private GameObject dialogOptions, optionAggresive, optionFriendly, optionTalk;
    [SerializeField] private GameObject optionAggresiveCant, optionFriendlyCant, optionTalkCant;
    private MapArea area;
    // Dialog
    [SerializeField] private GameObject dialogueBox, continueButton;
    [SerializeField] private float textSpeed;
    [SerializeField] private TextMeshProUGUI dialogTXT, nameTXT;
    [SerializeField] private Image portrait;
    [SerializeField] private DialogueSO[] turistAggresive, turistFriendly, turistTalk; 
    [SerializeField] private DialogueSO[] thrillHunterAggresive, thrillHunterFriendly, thrillHunterTalk;
    [SerializeField] private DialogueSO[] policemanAggresive, policemanFriendly, policemanTalk;
    [SerializeField] private int firstFail;

    [HideInInspector] public int talkBonusChance;
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
    private void Update()
    {
        if (dialogueBox.activeSelf && Input.GetMouseButtonDown(0))
        {
            if(dialogTXT.text != dialogue.lines[index].text)
            {
                StopAllCoroutines();
                dialogTXT.text = dialogue.lines[index].text;
                continueButton.SetActive(true);
            }
        }
    }
    public void TuristInteract(MapArea areaNew)
    {        
        PlayerControler.Instance.ButtonsAroundOff();
        areaNew.buttonAction.SetActive(false);
        area = areaNew;
        dialogOptions.SetActive(true);
        dialogueBox.SetActive(false);
        AvaiableOptions();
    }
    private void AvaiableOptions()
    {
        Turist turistScript = null;
        GameObject turist = null;
        foreach (GameObject turistCamp in GameManager.Instance.turistCamps)
        {
            if (turistCamp.GetComponent<Turist>().mapModule.row == area.row && turistCamp.GetComponent<Turist>().mapModule.column == area.column)
            {
                turist = turistCamp;
                turistScript = turist.GetComponent<Turist>();
                break;
            }
        }
        if (turistScript != null)
        {
            if (turistScript.aggresiveTalks > 0)
            {
                optionAggresive.SetActive(true);
                optionAggresiveCant.SetActive(false);
            }
            else
            {
                optionAggresive.SetActive(false);
                optionAggresiveCant.SetActive(true);
            }
            if (turistScript.friendlyTalks > 0)
            {
                optionFriendly.SetActive(true);
                optionFriendlyCant.SetActive(false);
            }
            else
            {
                optionFriendly.SetActive(false);
                optionFriendlyCant.SetActive(true);
            }
            if (turistScript.regularTalks > 0)
            {
                optionTalk.SetActive(true);
                optionTalkCant.SetActive(false);
            }
            else
            {
                optionTalk.SetActive(false);
                optionTalkCant.SetActive(true);
            }
        }
    }
    public void TuristAction1Button()
    {
        //
        if (GameManager.Instance.UseActionPoint())
        {
            GameObject turist = null;
            int type = 0;
            foreach (GameObject turistCamp in GameManager.Instance.turistCamps)
            {
                if (turistCamp.GetComponent<Turist>().mapModule.row == area.row && turistCamp.GetComponent<Turist>().mapModule.column == area.column)
                {
                    turist = turistCamp;
                    type = turist.GetComponent<Turist>().type;
                    break;
                }
            }
            if (turist != null && Random.Range(1, 101) <= (turist.GetComponent<Turist>().talkChanceAggresive + talkBonusChance))
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
                DialogueStart(1, true, type);
            }
            else if (turist != null)
            {
                turist.GetComponent<Turist>().aggresiveTalks--;
                DialogueStart(1, false, type);
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
            int type = 0;
            foreach (GameObject turistCamp in GameManager.Instance.turistCamps)
            {
                if (turistCamp.GetComponent<Turist>().mapModule.row == area.row && turistCamp.GetComponent<Turist>().mapModule.column == area.column)
                {
                    turist = turistCamp;
                    type = turist.GetComponent<Turist>().type;
                    break;
                }
            }
            if (turist != null && Random.Range(1, 101) <= (turist.GetComponent<Turist>().talkChanceFriendly + talkBonusChance))
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
                DialogueStart(2, true, type);
            }
            else if (turist != null)
            {
                turist.GetComponent<Turist>().friendlyTalks--; 
                DialogueStart(2, false, type);
            }
        }
        dialogOptions.SetActive(false);
        area = null;
    }
    public void TuristLeaveButton()
    {
        //
        dialogOptions.SetActive(false);
        area = null;
        PlayerControler.Instance.ButtonsAround();
    }
    private DialogueSO dialogue;
    private int index;
    public void TuristAction3Button(int type)
    {
        DialogueStart(type, false, 0);
    }
    public void DialogueStart(int type, bool success, int turistType) // 1 - agrresive, 2 - friendly, 3 - talk;
    {
        dialogOptions.SetActive(false);
        dialogueBox.SetActive(true);
        continueButton.SetActive(false);
        dialogTXT.text = string.Empty;
        index = 0;
        if (type == 3)
        {
            foreach (GameObject turistCamp in GameManager.Instance.turistCamps)
            {
                if (turistCamp.GetComponent<Turist>().mapModule.row == area.row && turistCamp.GetComponent<Turist>().mapModule.column == area.column)
                {
                    turistType = turistCamp.GetComponent<Turist>().type;
                    turistCamp.GetComponent<Turist>().regularTalks--;
                    break;
                }
            }
        }
        switch (turistType)
        {
            case 0:
                if(type == 1)
                {
                    if (success) dialogue = turistAggresive[Random.Range(0, firstFail)];
                    else dialogue = turistAggresive[Random.Range(firstFail, turistAggresive.Length)];
                }
                else if (type == 2)
                {
                    if (success) dialogue = turistFriendly[Random.Range(0, firstFail)];
                    else dialogue = turistFriendly[Random.Range(firstFail, turistFriendly.Length)];
                }else if(type == 3)
                {
                    dialogue = turistTalk[Random.Range(0, turistTalk.Length)];
                }
                break;
            case 1:
                if (type == 1)
                {
                    if(success) dialogue = thrillHunterAggresive[Random.Range(0, firstFail)];
                    else dialogue = thrillHunterAggresive[Random.Range(firstFail, thrillHunterAggresive.Length)];
                }
                else if (type == 2)
                {
                    if (success) dialogue = thrillHunterFriendly[Random.Range(0, firstFail)];
                    else dialogue = thrillHunterFriendly[Random.Range(firstFail, thrillHunterFriendly.Length)];
                }
                else if (type == 3)
                {
                    dialogue = thrillHunterTalk[Random.Range(0, thrillHunterTalk.Length)];
                }
                break;
            case 2:
                if (type == 1)
                {
                    if (success) dialogue = policemanAggresive[Random.Range(0, firstFail)];
                    else dialogue = policemanAggresive[Random.Range(firstFail, policemanAggresive.Length)];
                }
                else if (type == 2)
                {
                    if (success) dialogue = policemanFriendly[Random.Range(0, firstFail)];
                    else dialogue = policemanFriendly[Random.Range(firstFail, policemanFriendly.Length)];
                }
                else if (type == 3)
                {
                    dialogue = policemanTalk[Random.Range(0, policemanTalk.Length)];
                }
                break;
            default: break;
        }
        StartCoroutine(TypeLine());
    }
    IEnumerator TypeLine()
    {
        nameTXT.text = dialogue.lines[index].name;
        portrait.sprite = dialogue.lines[index].portrait;
        foreach (char c in dialogue.lines[index].text.ToCharArray())
        {
            dialogTXT.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
        continueButton.SetActive(true);
    }
    public void NextLine()
    {
        index++;
        if (index < dialogue.lines.Count)
        {
            continueButton.SetActive(false);
            dialogTXT.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            dialogueBox.SetActive(false);
            PlayerControler.Instance.ButtonsAround();
        }
    }
}
