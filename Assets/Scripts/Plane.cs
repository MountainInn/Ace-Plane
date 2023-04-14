using System;
using UnityEngine;
using Zenject;
using System.Runtime.InteropServices;
using UniRx;

public class Plane : MonoBehaviour, Missile.ILockOnTarget
{
    [Inject] private ExplosionSpawner explosionSpawner;

    [DllImport("__Internal")]
    private static extern void Hello();

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private float speed;
    private Vector2 direction = Vector2.up;
    private Quaternion rot;
    private bool isOperable = true;

    [Inject]
    public void Construct(UserInput userInput)
    {
        userInput.onTouch += Steer;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

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

    public void PlaneInitCallback()
    {
        Debug.Log($"+++++++++++Plane Init Callabckckcc");
        Hello();
    }

    private void FixedUpdate()
    {
        if (!isOperable) return;

        rb.MoveRotation(
            Quaternion.RotateTowards(transform.rotation, rot, 360 * Time.fixedDeltaTime));

        rb.velocity = speed * transform.up;
    }

    public void Steer(Vector3 inputDirection)
    {
        if (!isOperable) return;

        this.direction = inputDirection;

        rot = Quaternion.LookRotation(Vector3.forward, this.direction);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.TryGetComponent(out Missile missile))
        {
            Explode();
        }
    }

    public void Explode()
    {
        explosionSpawner.Spawn(transform.position);

        spriteRenderer.enabled = false;
        isOperable = false;
        rb.simulated = false;

        MessageBroker.Default.Publish(new msgExploded{});
    }

    public void Repair()
    {
        spriteRenderer.enabled = true;
        isOperable = true;
        rb.simulated = true;
    }

    public struct msgExploded {}
}
