using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static UnityEngine.ParticleSystem;
using UnityEngine.SceneManagement;
using DG.Tweening;

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
    [SerializeField] private DialogueSO[] turistAggresiveS, turistAggresiveF, turistFriendlyS, turistFriendlyF, turistTalk; 
    [SerializeField] private DialogueSO[] thrillHunterAggresiveS, thrillHunterAggresiveF, thrillHunterFriendlyS, thrillHunterFriendlyF, thrillHunterTalk;
    [SerializeField] private DialogueSO[] policemanAggresiveS, policemanAggresiveF, policemanFriendlyS, policemanFriendlyF, policemanTalk;

    [HideInInspector] public int talkBonusChance;

    // charisma check
    [SerializeField] private GameObject diceRoll, checkFail, checkSuccess, dialogueButton;
    [SerializeField] private TextMeshProUGUI difficultyTXT, checkTXT;
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
        diceRoll.SetActive(false);
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
        checkFail.SetActive(false);
        checkSuccess.SetActive(false);
        checkTXT.text = "00";
        difficultyTXT.text = "00";
        dialogueButton.SetActive(false);
        diceRoll.SetActive(true);
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
    public void SetDifficultyTXT(int number)
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
        switch (number)
        {
            case 0:
                difficultyTXT.text = "00";
                break;
            case 1:
                difficultyTXT.text = (turist.GetComponent<Turist>().talkChanceAggresive + talkBonusChance).ToString();
                break;
            case 2:
                difficultyTXT.text = (turist.GetComponent<Turist>().talkChanceFriendly + talkBonusChance).ToString();
                break;
            default: break;
        }
    }
    public void TuristAction1Button()
    {
        StartCoroutine(TuristAction1());
    }
    private IEnumerator TuristAction1()
    {
        dialogOptions.SetActive(false);

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
                    turistType = type;
                    break;
                }
            }
            if (turist != null)//&& Random.Range(1, 100) <= (turist.GetComponent<Turist>().talkChanceAggresive + talkBonusChance))
            {
                difficultyTXT.text = (turist.GetComponent<Turist>().talkChanceAggresive + talkBonusChance).ToString();
                for (int i = 0; i < 20; i++)
                {
                    checkTXT.text = Random.Range(0, 100).ToString();
                    yield return new WaitForSeconds(.05f);
                }
                int check = Random.Range(1, 100);
                checkTXT.text = check.ToString();
                if (check <= (turist.GetComponent<Turist>().talkChanceAggresive + talkBonusChance))
                {
                    checkSuccess.GetComponent<TextMeshProUGUI>().alpha = 0f;
                    checkSuccess.SetActive(true);
                    checkSuccess.GetComponent<TextMeshProUGUI>().DOFade(1f, .6f);
                    yield return new WaitForSeconds(.6f);

                    if (area.state == 6 && !area.AreThereTuristsAround()) area.state = 2;
                    else if (area.state == 6 && area.AreThereTuristsAround()) area.state = 4;
                    else if (area.state == 5 && !area.AreThereTuristsAround()) area.state = 1;
                    else if (area.state == 5 && area.AreThereTuristsAround()) area.state = 3;
                    GameManager.Instance.gameIndicator += turist.GetComponent<Turist>().gameIndicatorWhenScared;
                    if (GameManager.Instance.gameIndicator > 100) GameManager.Instance.gameIndicator = 100;
                    GameManager.Instance.GetCurrentFillIndicator();
                    GameManager.Instance.turistCamps.Remove(turist);
                    foreach (MapArea n in area.neighbours)
                    {
                        if (n.state == 4 && !n.AreThereTuristsAround()) n.state = 2;
                        else if (n.state == 3 && !n.AreThereTuristsAround()) n.state = 1;
                    }
                    Destroy(turist);
                    dialogueType = 1;
                    dialogueButton.SetActive(true);
                }
                else
                {
                    checkFail.GetComponent<TextMeshProUGUI>().alpha = 0f;
                    checkFail.SetActive(true);
                    checkFail.GetComponent<TextMeshProUGUI>().DOFade(1f, .6f);
                    yield return new WaitForSeconds(.6f);

                    turist.GetComponent<Turist>().aggresiveTalks--;
                    dialogueType = 1;
                    dialogueButton.SetActive(true);
                }
            }
        }
        //dialogOptions.SetActive(false);
        area = null;
        PlayerControler.Instance.ButtonsAround();
    }
    private int turistType, dialogueType;
    public void DialogueTrigger()
    {
        if (checkSuccess.activeSelf)
        {
            DialogueStart(dialogueType, true, turistType);
        }
        else
        {
            DialogueStart(dialogueType, false, turistType);
        }
    }
    public void TuristAction2Button()
    {
        StartCoroutine(TuristAction2());
    }
    IEnumerator TuristAction2()
    {
        dialogOptions.SetActive(false);

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
            if (turist != null)
            {
                difficultyTXT.text = (turist.GetComponent<Turist>().talkChanceFriendly + talkBonusChance).ToString();
                for (int i = 0; i < 20; i++)
                {
                    checkTXT.text = Random.Range(0, 100).ToString();
                    yield return new WaitForSeconds(.05f);
                }
                int check = Random.Range(1, 100);
                checkTXT.text = check.ToString();
                if (check <= (turist.GetComponent<Turist>().talkChanceFriendly + talkBonusChance))
                {
                    checkSuccess.GetComponent<TextMeshProUGUI>().alpha = 0f;
                    checkSuccess.SetActive(true);
                    checkSuccess.GetComponent<TextMeshProUGUI>().DOFade(1f, .6f);
                    yield return new WaitForSeconds(.6f);

                    if (area.state == 6 && !area.AreThereTuristsAround()) area.state = 2;
                    else if (area.state == 6 && area.AreThereTuristsAround()) area.state = 4;
                    else if (area.state == 5 && !area.AreThereTuristsAround()) area.state = 1;
                    else if (area.state == 5 && area.AreThereTuristsAround()) area.state = 3;
                    GameManager.Instance.gameIndicator += turist.GetComponent<Turist>().gameIndicatorWhenScared;
                    GameManager.Instance.turistCamps.Remove(turist);
                    foreach (MapArea n in area.neighbours)
                    {
                        if (n.state == 4 && !n.AreThereTuristsAround()) n.state = 2;
                        else if (n.state == 3 && !n.AreThereTuristsAround()) n.state = 1;
                    }
                    Destroy(turist);
                    dialogueType = 2;
                    dialogueButton.SetActive(true);
                }
                else
                {
                    checkFail.GetComponent<TextMeshProUGUI>().alpha = 0f;
                    checkFail.SetActive(true);
                    checkFail.GetComponent<TextMeshProUGUI>().DOFade(1f, .6f);
                    yield return new WaitForSeconds(.6f);

                    turist.GetComponent<Turist>().friendlyTalks--;
                    dialogueType = 2;
                    dialogueButton.SetActive(true);
                }
            }
        }
        //dialogOptions.SetActive(false);
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
                    if (success) dialogue = turistAggresiveS[Random.Range(0, turistAggresiveS.Length)];
                    else dialogue = turistAggresiveF[Random.Range(0, turistAggresiveF.Length)];
                }
                else if (type == 2)
                {
                    if (success) dialogue = turistFriendlyS[Random.Range(0, turistFriendlyS.Length)];
                    else dialogue = turistFriendlyF[Random.Range(0, turistFriendlyF.Length)];
                }else if(type == 3)
                {
                    dialogue = turistTalk[Random.Range(0, turistTalk.Length)];
                }
                break;
            case 1:
                if (type == 1)
                {
                    if(success) dialogue = thrillHunterAggresiveS[Random.Range(0, thrillHunterAggresiveS.Length)];
                    else dialogue = thrillHunterAggresiveF[Random.Range(0, thrillHunterAggresiveF.Length)];
                }
                else if (type == 2)
                {
                    if (success) dialogue = thrillHunterFriendlyS[Random.Range(0, thrillHunterFriendlyS.Length)];
                    else dialogue = thrillHunterFriendlyF[Random.Range(0, thrillHunterFriendlyF.Length)];
                }
                else if (type == 3)
                {
                    dialogue = thrillHunterTalk[Random.Range(0, thrillHunterTalk.Length)];
                }
                break;
            case 2:
                if (type == 1)
                {
                    if (success) dialogue = policemanAggresiveS[Random.Range(0, policemanAggresiveS.Length)];
                    else dialogue = policemanAggresiveF[Random.Range(0, policemanAggresiveF.Length)];
                }
                else if (type == 2)
                {
                    if (success) dialogue = policemanFriendlyS[Random.Range(0, policemanFriendlyS.Length)];
                    else dialogue = policemanFriendlyF[Random.Range(0, policemanFriendlyF.Length)];
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
