using System;
using UnityEngine;
using Zenject;
using UniRx.Toolkit;
using UniRx;

public class Missile : MonoBehaviour
{
    public interface ILockOnTarget
    {
        Transform transform {get;}
    }

    private Rigidbody2D rb;
    private float speed;

    [Inject] private ILockOnTarget target;
    [Inject] private CoinSpawner coinSpawner;
    [Inject] private ExplosionSpawner explosionSpawner;
    [Inject] private AutoDestruct autoDestruct;
    [Inject] private MissilePool missilePool;
    [Inject] private MissileTrail.Pool trailPool;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        var gameSettings = GameSettings.Get();
        speed = gameSettings.missileSpeed;

        autoDestruct.onAutoDestruct += ExplodeWithCoin;

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
        var direction = (target.transform.position - transform.position).normalized;
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
