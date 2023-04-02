using UnityEngine;
using Zenject;
using UniRx.Toolkit;
using System;

public class MissileTrail : MonoBehaviour
{
    private Missile missile;
    private TrailRenderer trail;

    [Inject]
    private Pool pool;

    Action update;

    private void Awake()
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

    private void FollowMissile()
    {
        if (missile.gameObject.activeInHierarchy == false)
        {
            update = WaitForDissolve;
            return;
        }

        transform.position = missile.transform.position;
    }

    private void WaitForDissolve()
    {
        if (trail.positionCount == 0)
        {
            update = null;
            missile = null;
            pool.Return(this);
        }
    }

    private void LateUpdate()
    {
        update?.Invoke();
    }

    private void OnEnable()
    {
        trail.emitting = true;
    }
    private void OnDisable()
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
