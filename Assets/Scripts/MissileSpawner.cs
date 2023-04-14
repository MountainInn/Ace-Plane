using UnityEngine;
using Zenject;

public class MissileSpawner : MonoBehaviour
{
    [Inject] Missile.MissilePool missilePool;
    [Inject] Plane plane;

    float spawnRadius;
    float t = 0, interval = 3;
    bool spawning;

    void Awake()
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

    void Update()
    {
        if (spawning)
        if ((t += Time.deltaTime) >= interval)
        {
            t -= interval;

            Spawn();
        }
    }

    void Spawn()
    {
        var missile = missilePool.Rent();

        Vector2 circlePosition = Random.insideUnitCircle.normalized;

        missile.transform.position =
            plane.transform.position
            + new Vector3(circlePosition.x, circlePosition.y, 0) * spawnRadius;
    }

    public void StartSpawn()
    {
        spawning = true;
    }
    public void StopSpawn()
    {
        spawning = false;
    }
    public void ClearPool()
    {
        missilePool.Clear();
    }
}
