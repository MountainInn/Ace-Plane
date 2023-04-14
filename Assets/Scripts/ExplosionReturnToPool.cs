using UnityEngine;

internal class ExplosionReturnToPool : ReturnToPool<ParticleSystem>
{
    void Awake()
    {
        ParticleSystem.MainModule main = GetComponent<ParticleSystem>().main;
        main.stopAction = ParticleSystemStopAction.Callback;
    }

    void OnParticleSystemStopped()
    {
        pool.Release(gameObject);
    }
}
