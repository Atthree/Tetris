using DG.Tweening;
using System.Collections;
using UnityEngine;

public class ScreenFadeManager : MonoBehaviour
{
    [SerializeField] private float startingAlpha = 1f;
    [SerializeField] private float endAlpha = 0f;

    [SerializeField] private float waitTime = 0f;
    [SerializeField] private float fadeTime = 1f;

    private void Start()
    {
        GetComponent<CanvasGroup>().alpha = startingAlpha;
        StartCoroutine(FadeRoutine());
    }

    IEnumerator FadeRoutine()
    {
        yield return new WaitForSeconds(waitTime);
        GetComponent<CanvasGroup>().DOFade(endAlpha,fadeTime);
    }
}
