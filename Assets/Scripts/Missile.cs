using System;
using UnityEngine;
using Zenject;
using UniRx.Toolkit;
using UniRx;

public class Missile : MonoBehaviour
{
    public interface ILockOnTarget
    {
    }

    private Rigidbody2D rb;
    private float speed;

    private Transform target;
    private CoinSpawner coinSpawner;
    private ExplosionSpawner explosionSpawner;
    private AutoDestruct autoDestruct;
    private MissilePool missilePool;
    private MissileTrail.Pool trailPool;

    [Inject]
    public void Construct(Plane plane,
                          CoinSpawner coinSpawner,
                          MissilePool missilePool,
                          ExplosionSpawner explosionSpawner,
                          MissileTrail.Pool trailPool,
                          AutoDestruct autoDestruct)
    {
        this.missilePool = missilePool;
        this.trailPool = trailPool;
        this.coinSpawner = coinSpawner;
        this.explosionSpawner = explosionSpawner;
        this.autoDestruct = autoDestruct;

        autoDestruct.onAutoDestruct += ExplodeWithCoin;

        target = ((MonoBehaviour) plane).transform;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

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

        var rot = Quaternion.LookRotation(Vector3.forward, rb.velocity.normalized);
        rb.MoveRotation(rot);
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.TryGetComponent(out Missile missile))
        {
            ExplodeWithCoin();
        }
        else if (col.TryGetComponent(out Plane plane))
        {
            Explode();
        }

    }

    void ExplodeWithCoin()
    {
        Explode();
        coinSpawner.Spawn(transform.position);

        MessageBroker.Default.Publish(new Score.msgScoreChange{ amount = 5 });
    }

    public void Explode()
    {
        explosionSpawner.Spawn(transform.position);

        missilePool.Return(this);
    }

    public struct msgExploded {  }


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
