using System;
using UnityEngine;

public class Explosive : MonoBehaviour
{
    [SerializeField] private ParticleSystem explosionPS;

    public event Action onExplode;

    public void Explode()
    {
        explosionPS.transform.position = transform.position;
        explosionPS.Emit(4);

        onExplode?.Invoke();
    }
}
