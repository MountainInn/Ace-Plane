using System;
using UnityEngine;

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
