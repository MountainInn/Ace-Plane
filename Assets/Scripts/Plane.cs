using System;
using UnityEngine;
using Zenject;

public class Plane : MonoBehaviour, Missile.ILockOnTarget
{
    private Rigidbody2D rb;
    private MeshRenderer meshRenderer;
    private Explosive explosive;
    private float speed;
    private bool isOperable;

    public event Action<Vector3> onChangePosition;

    [Inject]
    public void Construct(UserInput userInput, Explosive explosive)
    {
        this.explosive = explosive;
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
