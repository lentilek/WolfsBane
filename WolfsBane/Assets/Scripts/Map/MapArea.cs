using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapArea : MonoBehaviour
{
    public int type; // 0 - out of map, 1 - regular, 2 - resource, 3 - blocked
    public bool isAvailable;
    public bool isVisible;
    public GameObject cloud;

    private void Start()
    {
        if(type == 3 || type == 0)
        {
            isAvailable = false;
        }
        else
        {
            isAvailable = true;
        }
        if(!isVisible)
        {
            cloud.SetActive(true);
        }
        else
        {
            cloud.SetActive(false);
        }
    }
}
