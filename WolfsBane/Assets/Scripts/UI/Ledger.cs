using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ledger : MonoBehaviour
{
    public static Ledger Instance;

    [HideInInspector] public bool letter, book, flowers, dyploma, poster, fernFlower;

    [SerializeField] private GameObject letterNote, bookNote, flowersNote, dyplomaNote, posterNote, fernFlowerNote;

    [SerializeField] private DialogueSO[] dialogues;

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
    }

    public void FindLetter()
    {
        letter = true;
        letterNote.SetActive(letter);
        Dialog.Instance.SpecialDialogue(dialogues[0]);
    }
    public void FindBook()
    {
        book = true;
        bookNote.SetActive(book);
        Dialog.Instance.SpecialDialogue(dialogues[1]);
    }
    public void FindFlowers()
    {
        flowers = true;
        flowersNote.SetActive(flowers);
        Dialog.Instance.SpecialDialogue(dialogues[2]);
    }
    public void FindDyploma()
    {
        dyploma = true;
        dyplomaNote.SetActive(dyploma);
        Dialog.Instance.SpecialDialogue(dialogues[3]);
    }
    public void FindPoster()
    {
        poster = true;
        posterNote.SetActive(poster);
        Dialog.Instance.SpecialDialogue(dialogues[4]);
    }
    public void FindFernFlower()
    {
        fernFlower = true;
        fernFlowerNote.SetActive(fernFlower);
        Dialog.Instance.SpecialDialogue(dialogues[5]);
    }
    public bool SpecialEnding()
    {
        if (book && flowers && fernFlower) return true;
        return false;
    }
}
