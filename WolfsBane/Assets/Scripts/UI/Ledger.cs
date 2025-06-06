using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class Ledger : MonoBehaviour
{
    public static Ledger Instance;

    [HideInInspector] public bool letter, book, flowers, dyploma, poster, fernFlower;

    [SerializeField] private GameObject letterNote, bookNote, flowersNote, dyplomaNote, posterNote, fernFlowerNote;

    [SerializeField] private DialogueSO[] dialogues;

    [SerializeField] private Image ledgerIcon;
    [SerializeField] private Color baseLedgerColor, animLedgerColor;
    [SerializeField] private float animScale, animLength;

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
        letter = false;
        book = false;
        flowers = false;
        dyploma = false;
        poster = false;
        fernFlower = false;
        letterNote.SetActive(false);
        bookNote.SetActive(false);
        flowersNote.SetActive(false);
        dyplomaNote.SetActive(false);
        posterNote.SetActive(false);
        fernFlowerNote.SetActive(false);
        baseLedgerColor = ledgerIcon.color;
    }

    public void FindLetter()
    {
        letter = true;
        letterNote.SetActive(letter);
        LedgerAnimation();
        Dialog.Instance.SpecialDialogue(dialogues[0]);
    }
    public void FindBook()
    {
        book = true;
        bookNote.SetActive(book);
        LedgerAnimation();
        Dialog.Instance.SpecialDialogue(dialogues[1]);
    }
    public void FindFlowers()
    {
        flowers = true;
        flowersNote.SetActive(flowers);
        LedgerAnimation();
        Dialog.Instance.SpecialDialogue(dialogues[2]);
    }
    public void FindDyploma()
    {
        dyploma = true;
        dyplomaNote.SetActive(dyploma);
        LedgerAnimation();
        Dialog.Instance.SpecialDialogue(dialogues[3]);
    }
    public void FindPoster()
    {
        poster = true;
        posterNote.SetActive(poster);
        LedgerAnimation();
        Dialog.Instance.SpecialDialogue(dialogues[4]);
    }
    public void FindFernFlower()
    {
        fernFlower = true;
        fernFlowerNote.SetActive(fernFlower);
        LedgerAnimation();
        Dialog.Instance.SpecialDialogue(dialogues[5]);
    }
    public void LedgerAnimation()
    {
        ledgerIcon.gameObject.transform.localScale = Vector3.one;
        StartCoroutine(LedgerAnim());
    }
    private IEnumerator LedgerAnim()
    {
        ledgerIcon.color = animLedgerColor;
        ledgerIcon.gameObject.transform.DOScale(animScale, animLength);
        yield return new WaitForSeconds(animLength);
        ledgerIcon.gameObject.transform.DOScale(1f, animLength);
        yield return new WaitForSeconds(animLength);
        ledgerIcon.color = baseLedgerColor;
    }
    public bool SpecialEnding()
    {
        if (book && flowers && fernFlower) return true;
        return false;
    }
}
