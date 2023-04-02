using UnityEngine;
using Zenject;

public class MissileSpawner : MonoBehaviour
{
    [Inject] Missile.MissilePool missilePool;
    [Inject] Plane plane;

    float spawnRadius;
    float t = 0, interval = 3;

    private void Awake()
    {
        var gameSettings = GameSettings.Get();
        spawnRadius = gameSettings.missileSpawnRadius;

#if UNITY_EDITOR
        gameObject.AddComponent<GameSettingsWatcher>()
            .onUpdate += () =>
            {
                spawnRadius = gameSettings.missileSpawnRadius;
            };
#endif
    }

    private void Update()
    {
        if ((t += Time.deltaTime) >= interval)
        {
            t -= interval;

            Spawn();
        }
    }

    private void Spawn()
    {
        var missile = missilePool.Rent();

        Vector2 circlePosition = Random.insideUnitCircle.normalized;

        missile.transform.position =
            plane.transform.position
            + new Vector3(circlePosition.x, circlePosition.y, 0) * spawnRadius;
    }
}
