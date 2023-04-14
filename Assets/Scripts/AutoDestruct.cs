using UnityEngine;
using DG.Tweening;
using System;
using System.Collections;

public class AutoDestruct : MonoBehaviour
{
    [SerializeField] SpriteRenderer render;

    public event Action onAutoDestruct;

    float lifetime = 10;
    float flashingThreshold = 4;

    Coroutine countdownCoroutine;

    Color baseColor, flashColor;
    Sequence sequence;

    void Awake()
    {
        render = GetComponent<SpriteRenderer>();

        baseColor = render.color;
        flashColor = Color.Lerp(baseColor, Color.white, .75f);
    }

    void OnEnable()
    {
        countdownCoroutine = StartCoroutine(Countdown());
    }

    IEnumerator Countdown()
    {
        for (float l = lifetime;
             l > 0;
             l -= Time.deltaTime)
        {
            yield return null;

            if (l <= flashingThreshold && sequence == null)
            {
                float duration = l / flashingThreshold / 2;
                sequence =
                    DOTween.Sequence()
                    .Append(render.DOColor(flashColor, duration))
                    .Append(render.DOColor(baseColor, duration))
                    .OnKill(() => sequence = null)
                    .Play();
            }
        }

        onAutoDestruct?.Invoke();
    }

    void OnDisable()
    {
        StopCoroutine(countdownCoroutine);
        render.DOKill();
        sequence = null;
    }
}
