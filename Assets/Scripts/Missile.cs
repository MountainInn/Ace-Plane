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
    private float speed;

    public event Action onHitMissile;

    /// TODO: Добавить Coin и CoinSpawner
    /// TODO: ДОбавить Plane.Repair()
    /// TODO: Скорее всего понадобится проверять touchDeltaPosition.magnitude в UserInput, чтобы понизить чувствительность джойстика

    [Inject]
    public void Construct(ILockOnTarget plane, Explosive explosive, CoinSpawner coinSpawner, MissilePool missilePool)
    {
        this.explosive = explosive;
        explosive.onExplode += () => missilePool.Return(this);

        onHitMissile += coinSpawner.Spawn;

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

    private void Home()
    {
        var direction = (target.position - transform.position).normalized;
        var force = speed * direction;
        rb.AddForce(force, ForceMode2D.Force);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.otherCollider.TryGetComponent(out Missile missile))
        {
            onHitMissile?.Invoke();
            Explode();
        }
        else if (collision.otherCollider.TryGetComponent(out Plane plane))
        {
            Explode();
        }

    }

    private void Explode()
    {
        explosive.Explode();
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
