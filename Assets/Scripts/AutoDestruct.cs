using UnityEngine;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AutoDestruct : MonoBehaviour
{
    [SerializeField] List<MeshRenderer> meshRenderers;

    public event Action onAutoDestruct;

    float lifetime = 10;
    float flashingThreshold = 10;

    Coroutine countdownCoroutine;

    Sequence sequence;
    private int tProperty;

    void Awake()
    {
        meshRenderers = GetComponentsInChildren<MeshRenderer>().ToList();

        tProperty = Shader.PropertyToID("_t");
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

                sequence = DOTween.Sequence();

                for (int i = 0; i < meshRenderers.Count; i++)
                {
                    MeshRenderer render  = meshRenderers[i];
                    sequence
                        .Append(render.material.DOFloat(.75f, tProperty, duration))
                        .Append(render.material.DOFloat(.0f, tProperty, duration));
                }

                sequence
                    .OnKill(() => sequence = null)
                    .Play();
            }
        }

        onAutoDestruct?.Invoke();
    }

    void OnDisable()
    {
        StopCoroutine(countdownCoroutine);
        foreach(var render in meshRenderers)
        {
            render.DOKill();
        }
        sequence = null;
    }
}
