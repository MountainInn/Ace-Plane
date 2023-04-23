using UnityEngine;
using DG.Tweening;
using System;
using System.Collections;

public class AutoDestruct : MonoBehaviour
{
    [SerializeField] MeshRenderer render;

    public event Action onAutoDestruct;

    float lifetime = 10;
    float flashingThreshold = 4;

    Coroutine countdownCoroutine;

    Color baseColor, flashColor;
    Sequence sequence;

    void Awake()
    {
        render = GetComponent<MeshRenderer>();

        baseColor = render.material.color;
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
                    .Append(render.material.DOColor(flashColor, duration))
                    .Append(render.material.DOColor(baseColor, duration))
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
