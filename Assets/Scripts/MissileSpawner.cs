using UnityEngine;

public class MissileSpawner : MonoBehaviour
{
    Missile.MissilePool missilePool;
    Plane plane;

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
                spawnRadius = gameSettings.planeSpeed;
            };
#endif
    }

    private void Update()
    {
        if ((t += Time.deltaTime) >= interval)
        {
            t -= interval;

            var missile = missilePool.Rent();

            Vector2 circlePosition = Random.insideUnitCircle.normalized * spawnRadius;

            missile.transform.position =
                plane.transform.position + new Vector3(circlePosition.x,
                                                       0,
                                                       circlePosition.y);
        }
    }
}
