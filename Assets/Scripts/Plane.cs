using System;
using UnityEngine;
using Zenject;

public class Plane : MonoBehaviour, Missile.ILockOnTarget
{
    private Rigidbody2D rb;
    private MeshRenderer meshRenderer;
    private Explosive explosive;
    private float speed;
    private Vector2 direction = Vector2.up;
    private Quaternion rot;
    private bool isOperable;

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

    private void FixedUpdate()
    {
        rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, rot, 360 * Time.fixedDeltaTime));
    }

    private void Steer(Vector3 touchDelta)
    {
        if (isOperable) return;

        direction = touchDelta.normalized;

        rb.velocity = speed * direction;

        rot = Quaternion.LookRotation(Vector3.forward, direction);
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
        rb.velocity = direction * speed;
    }
}
