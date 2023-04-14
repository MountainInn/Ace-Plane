using UnityEngine;
using Zenject;
using UniRx.Toolkit;
using System;

public class MissileTrail : MonoBehaviour
{
    Missile missile;
    TrailRenderer trail;

    [Inject]
    Pool pool;

    Action update;

    void Awake()
    {
        trail = GetComponent<TrailRenderer>();
        update = null;
    }

    public void SetMissile(Missile missile)
    {
        this.missile = missile;
        this.update = FollowMissile;
        FollowMissile();
    }

    void FollowMissile()
    {
        if (missile.gameObject.activeInHierarchy == false)
        {
            update = WaitForDissolve;
            return;
        }

        transform.position = missile.transform.position;
    }

    void WaitForDissolve()
    {
        if (trail.positionCount == 0)
        {
            update = null;
            missile = null;
            pool.Return(this);
        }
    }

    void LateUpdate()
    {
        update?.Invoke();
    }

    void OnEnable()
    {
        trail.emitting = true;
    }
    void OnDisable()
    {
        trail.Clear();
        trail.emitting = false;
    }

    public class Pool : ObjectPool<MissileTrail>
    {
        [Inject]
        Factory trailFactory;

        protected override MissileTrail CreateInstance()
        {
            var newTrail = trailFactory.Create();

            return newTrail;
        }
    }

    public class Factory : PlaceholderFactory<MissileTrail>
    {
    }

}
