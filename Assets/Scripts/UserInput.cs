using System;
using UnityEngine;
using Zenject;

public class UserInput : MonoBehaviour
{
    public event Action<Vector2> onTouch;

    void Update()
    {
        onTouch?.Invoke(Input.GetTouch(0).deltaPosition);
    }
}

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

public class Plane : MonoBehaviour, Missile.ILockOnTarget
{
    private Rigidbody2D rb;
    private MeshRenderer meshRenderer;
    private Explosive explosive;
    private float speed;
    private bool isOperable;

    public event Action<Vector3> onChangePosition;

    // [Inject]
    public void Construct(UserInput userInput, Explosive explosive)
    {
        userInput.onTouch += Steer;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        meshRenderer = GetComponent<MeshRenderer>();
        explosive = GetComponent<Explosive>();

        var gameSettings = GameSettings.Get();
        speed = gameSettings.planeSpeed;

#if UNITY_EDITOR
        gameObject.AddComponent<GameSettingsWatcher>()
            .onUpdate += () =>
            {
                speed = gameSettings.planeSpeed;
            };
#endif
    }

    private void Steer(Vector2 touchDelta)
    {
        if (!isOperable) return;

        var direction = touchDelta.normalized;
        transform.LookAt(direction);
        rb.velocity = speed * direction;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.otherCollider.TryGetComponent(out Missile missile))
        {
            Explode();
        }
    }

    private void Explode()
    {
        explosive.Explode();

        meshRenderer.enabled = false;
        isOperable = false;
        rb.velocity = Vector2.zero;
    }

    private void Repair()
    {
        meshRenderer.enabled = true;
        isOperable = true;
    }
}

public class GameSettingsWatcher : MonoBehaviour
{
    private GameSettings gameSettings;

    public event Action onUpdate;

    private void Awake()
    {
        gameSettings = GameSettings.Get();
    }

    private void Update()
    {
        onUpdate?.Invoke();
    }
}

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

    /// TODO: Спавнить ракеты через фабрику Zenject, использующую пул UniRx
    /// Добавить Coin и CoinSpawner
    /// ДОбавить Plane.Repair()
    /// Скорее всего понадобится проверять touchDeltaPosition.magnitude в UserInput, чтобы понизить чувствительность джойстика
    /// Написать контекст зенжекта

    // [Inject]
    public void Construct(ILockOnTarget plane, Explosive explosive, CoinSpawner coinSpawner)
    {
        plane.onChangePosition += Home;
        explosive.onExplode += OnExplode;
        onHitMissile += coinSpawner.Spawn();
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
            explosive.Explode();
            onHitMissile?.Invoke();
        }
        else if (collision.otherCollider.TryGetComponent(out Plane plane))
        {
            explosive.Explode();
        }

    }

    private void OnExplode()
    {
        Destroy(gameObject);
    }
}

// [CreateAssetMenu(fileName = "GameSettings", menuName = "ScriptableObjects/GameSettings", order = 1)]
// public class GameSettings : ScriptableObject
// {
//     static public GameSettings Get() => inst ??= Resources.Load("GameSettings");
//     static private GameSettings inst;

//     [SerializeField] public float planeSpeed;
//     [SpaceAttribute]
//     [SerializeField] public float missileSpeed;
//     }
