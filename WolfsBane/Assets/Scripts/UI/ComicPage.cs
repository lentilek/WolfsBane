using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ComicPage : MonoBehaviour
{
    [SerializeField] private Image[] pages;
    [SerializeField] private float duration;
    [SerializeField] private bool onTop;

    private void Awake()
    {
        SetUp();
    }
    private void Start()
    {
        if (onTop) StartCoroutine(FadeInComic());
    }
    private void SetUp()
    {
        foreach(var page in pages)
        {
            Transparent(page, 0);
        }
        if (!onTop) this.gameObject.SetActive(false);
    }
    private void Transparent(Image image, float alpha)
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
    }
    public void StartFade()
    {
        StartCoroutine(FadeInComic());
    }
    IEnumerator FadeInComic()
    {
        foreach(var page in pages)
        {
            page.DOFade(1f, duration).SetUpdate(true);
            yield return new WaitForSecondsRealtime(duration);
        }
    }
}
