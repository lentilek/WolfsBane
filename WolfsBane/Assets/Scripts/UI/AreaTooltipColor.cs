using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AreaTooltipColor : MonoBehaviour
{
    private Image image;
    [SerializeField] private MapArea ma;
    [SerializeField] private Color colorHidden, colorEmpty, colorResource, colorBase, colorTurist, colorTask;
    private Color tempColor;
    private void Awake()
    {
        image = GetComponent<Image>();
        Hide();
    }
    private void Update()
    {
        ColorUpdate();
    }
    public void ColorUpdate()
    {
        if (gameObject.activeSelf)
        {
            if (!ma.isVisible) image.color = colorHidden;
            else if (ma.type == 2) image.color = colorResource;
            else if (ma.type == 4) image.color = colorBase;
            else if (ma.taskIndex != 0) image.color = colorTask;
            else if (ma.state == 5 || ma.state == 6) image.color = colorTurist;
            else image.color = colorEmpty;
        }
    }
    public void Hide()
    {
        tempColor = image.color;
        tempColor.a = 0f;
        image.color = tempColor;
    }
}
