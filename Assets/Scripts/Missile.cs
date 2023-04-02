using System;
using UnityEngine;
using Zenject;
using UniRx.Toolkit;


public class Missile : MonoBehaviour
{
    public interface ILockOnTarget
    {
    }

    private Transform target;
    private Explosive explosive;
    private Rigidbody2D rb;
    private CoinSpawner coinSpawner;
    private MissilePool missilePool;
    private MissileTrail.Pool trailPool;
    private float speed;

    [Inject]
    public void Construct(ILockOnTarget plane, Explosive explosive, CoinSpawner coinSpawner, MissilePool missilePool, MissileTrail.Pool trailPool)
    {
        this.explosive = explosive;
        this.missilePool = missilePool;
        this.trailPool = trailPool;

        target = ((MonoBehaviour) plane).transform;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        explosive = GetComponent<Explosive>();

        var gameSettings = GameSettings.Get();
        speed = gameSettings.missileSpeed;

#if UNITY_EDITOR
        gameObject.AddComponent<GameSettingsWatcher>()
            .onUpdate += () =>
            {
                speed = gameSettings.missileSpeed;
            };
#endif
    }

    private void Update()
    {
        if (target is null) return;

        Home();
    }

    private void OnEnable()
    {
        trailPool.Rent().SetMissile(this);
    }

    private void Home()
    {
        var direction = (target.position - transform.position).normalized;
        var force = speed * direction;
        rb.AddForce(force, ForceMode2D.Force);
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.TryGetComponent(out Missile missile))
        {
            Explode();
        }
        else if (col.TryGetComponent(out Plane plane))
        {
            Explode();
        }

    }

    private void Explode()
    {
        explosive.Explode();

        missilePool.Return(this);
    }



    public class MissilePool : ObjectPool<Missile>
    {
        [Inject]
        Missile.Factory missileFactory;

        protected override Missile CreateInstance()
        {
            var newMissile = missileFactory.Create();

            return newMissile;
        }
    }

    public class Factory : PlaceholderFactory<Missile>
    {
    }
}
