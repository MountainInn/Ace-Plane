using System;
using UnityEngine;
using Zenject;

public class Explosive : MonoBehaviour
{
    [Inject] private ParticleSystem explosionPS;

    public event Action onExplode;

    public void Explode()
    {
        explosionPS.transform.position = transform.position;
        explosionPS.Emit(4);

        onExplode?.Invoke();
    }
}
