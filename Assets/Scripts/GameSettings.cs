using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "ScriptableObjects/GameSettings", order = 1)]
public class GameSettings : ScriptableObject
{
    static public GameSettings Get() => inst ??= Resources.Load<GameSettings>("GameSettings");
    static private GameSettings inst;

    [SerializeField] public float planeSpeed;
    [Space]
    [SerializeField] public float missileSpeed;
    [SerializeField] public float missileSpawnRadius;
    [Space]
    [SerializeField] public float secondsToFever;
    [SerializeField] public float feverDuration;
}
