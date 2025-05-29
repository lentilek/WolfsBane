using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LorePickUp : MonoBehaviour
{
    [SerializeField] private GameObject pickUpButton;
    [HideInInspector] public MapArea module;

    private void Awake()
    {
        if (SceneManager.GetActiveScene().buildIndex != 1) this.enabled = false;
        pickUpButton.SetActive(false);
    }
    private void Update()
    {
        IsPlayerNear();
    }
    private bool IsPlayerNear()
    {
        if (PlayerControler.Instance.row == module.row && PlayerControler.Instance.column == module.column && !GameManager.Instance.isNight && !module.buttonsTraps.activeSelf)
        {
            pickUpButton.SetActive(true);
            return true;
        }
        foreach (MapArea ma in module.neighbours)
        {
            if (PlayerControler.Instance.row == ma.row && PlayerControler.Instance.column == ma.column && !GameManager.Instance.isNight)
            {
                pickUpButton.SetActive(true);
                return true;
            }
        }
        pickUpButton.SetActive(false);
        return false;
    }
    public void PickUp(int type)
    {
        AudioManager.Instance.PlaySound("uiSound");
        switch (type)
        {
            case 0:
                if (!Ledger.Instance.letter) Ledger.Instance.FindLetter();
                break;
            case 1:
                if (!Ledger.Instance.book) Ledger.Instance.FindBook();
                break;
            case 2:
                Ledger.Instance.FindFlowers();
                break;
            case 3:
                if (!Ledger.Instance.dyploma) Ledger.Instance.FindDyploma();
                break;
            case 4:
                if (!Ledger.Instance.poster) Ledger.Instance.FindPoster();
                break;
            case 5:
                Ledger.Instance.FindFernFlower();
                break;
            default: break;
        }
        this.gameObject.SetActive(false);
    }
}
