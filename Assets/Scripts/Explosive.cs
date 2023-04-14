using System;
using UnityEngine;
using Zenject;

public class Explosive : MonoBehaviour
{
    [Inject] ExplosionSpawner explosionSpawner;

    public void Explode()
    {
        explosionSpawner.Spawn(transform.position);
    }
}
