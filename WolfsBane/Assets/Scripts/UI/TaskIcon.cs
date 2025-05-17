using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskIcon : MonoBehaviour
{
    [SerializeField] private MapArea ma;
    [SerializeField] private GameObject bg;
    [SerializeField] private GameObject[] icons;
    private void Start()
    {
        AllOf();
    }
    private void Update()
    {
        if(ma.taskIndex != 0 && ma.isVisible && !ma.buttonTask.activeSelf)
        {
            if(!icons[0].activeSelf && ma.taskIndex == 1)
            {
                AllOf();
                bg.SetActive(true);
                icons[0].SetActive(true);
            }
            else if (!icons[1].activeSelf && ma.taskIndex == 2)
            {
                AllOf();
                bg.SetActive(true);
                icons[1].SetActive(true);
            }
            else if (!icons[2].activeSelf && ma.taskIndex == 3)
            {
                AllOf();
                bg.SetActive(true);
                icons[2].SetActive(true);
            }
            else if (!icons[3].activeSelf && ma.taskIndex == 7)
            {
                AllOf();
                bg.SetActive(true);
                icons[3].SetActive(true);
            }
        }
        else
        {
            AllOf();
        }
    }

    private void AllOf()
    {
        bg.SetActive(false);
        foreach(GameObject go in icons)
        {
            go.SetActive(false);
        }
    }
}
