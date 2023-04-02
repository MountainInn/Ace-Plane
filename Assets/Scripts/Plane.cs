using System;
using UnityEngine;
using Zenject;

public class Plane : MonoBehaviour, Missile.ILockOnTarget
{
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Explosive explosive;
    private float speed;
    private Vector2 direction = Vector2.up;
    private Quaternion rot;
    private bool isOperable = true;

    [Inject]
    public void Construct(UserInput userInput, Explosive explosive)
    {
        this.explosive = explosive;
        userInput.onTouch += Steer;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
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
        if (!isOperable) return;

        rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, rot, 360 * Time.fixedDeltaTime));

        rb.velocity = speed * transform.up;
    }

    private void Steer(Vector3 touchDelta)
    {
        if (!isOperable) return;

        direction = touchDelta.normalized;

        rot = Quaternion.LookRotation(Vector3.forward, direction);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.TryGetComponent(out Missile missile))
        {
            Explode();
        }
    }

    private void Explode()
    {
        explosive.Explode();

        // spriteRenderer.enabled = false;
        // isOperable = false;
        // rb.velocity = Vector2.zero;

        // Time.timeScale = 0f;
    }

    private void Repair()
    {
        // spriteRenderer.enabled = true;
        // isOperable = true;
        // rb.velocity = direction * speed;

        // Time.timeScale = 1f;
    }
}
