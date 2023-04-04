using System;
using UnityEngine;
using UnityEngine.UI;

public class Fever : MonoBehaviour
{
    CircleCollider2D trigger;
    Image indicator;

    int missilesInRange;
    float
        secondsToFever,
        feverDuration,
        t;

    Action<Collider2D> onTriggerStay;
    Action update;

    public event Action
        onFeverStarted,
        onFeverEnded;

    private void Awake()
    {
        var gameSettings = GameSettings.Get();
        secondsToFever = gameSettings.secondsToFever;
        feverDuration = gameSettings.feverDuration;

#if UNITY_EDITOR

        gameObject.AddComponent<GameSettingsWatcher>()
            .onUpdate += () =>
            {
                secondsToFever = gameSettings.secondsToFever;
                feverDuration = gameSettings.feverDuration;
            };
#endif
    }

    private void Start()
    {
        SwitchToNormal();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.TryGetComponent(out Missile missile))
        {
            missilesInRange++;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.TryGetComponent(out Missile missile))
        {
            missilesInRange--;
        }
    }

    private void SwitchToNormal()
    {
        t = 0;
        update = NormalUpdate;

        onFeverEnded?.Invoke();
    }

    private void NormalUpdate()
    {
        if (missilesInRange == 0)
        {
            t -= Time.deltaTime * .5f;
        }
        else if (missilesInRange > 0)
        {
            t += Time.deltaTime;
        }

        indicator.fillAmount = t / secondsToFever;

        if (t >= secondsToFever)
        {
            SwitchToFever();
        }
    }

    private void SwitchToFever()
    {
        t = feverDuration;
        update = FeverUpdate;

        onFeverStarted?.Invoke();
    }

    private void FeverUpdate()
    {
        t -= Time.deltaTime;

        if (t <= 0)
        {
            SwitchToNormal();
        }
    }

    private void Update()
    {
        update.Invoke();
    }
}
