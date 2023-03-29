using System;
using UnityEngine;
using Zenject;
using UniRx.Toolkit;


public class Missile : MonoBehaviour
{
    public interface ILockOnTarget
    {
        event Action<Vector3> onChangePosition;
    }

    private Explosive explosive;
    private Rigidbody2D rb;
    private float speed;

    public event Action onHitMissile;

    /// TODO: Добавить Coin и CoinSpawner
    /// TODO: ДОбавить Plane.Repair()
    /// TODO: Скорее всего понадобится проверять touchDeltaPosition.magnitude в UserInput, чтобы понизить чувствительность джойстика

    [Inject]
    public void Construct(ILockOnTarget plane, Explosive explosive, CoinSpawner coinSpawner)
    {
        this.explosive = explosive;
        plane.onChangePosition += Home;
        onHitMissile += coinSpawner.Spawn;
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

    private void Home(Vector3 newPosition)
    {
        var direction = (newPosition - transform.position).normalized;
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

            newMissile.explosive.onExplode += () => Return(newMissile);

            return newMissile;
        }
    }

    public class Factory : PlaceholderFactory<Missile>
    {
        [Inject]
        Missile prefab;

        public override Missile Create()
        {
            var missile = Instantiate(prefab);

            return missile;
        }
    }
}
