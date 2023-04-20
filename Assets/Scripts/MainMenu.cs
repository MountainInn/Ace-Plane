using System.Runtime.InteropServices;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using System.Linq;
using System.Collections;
using System;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    Image
        continueWheel;

    [SerializeField]
    RectTransform
        mainMenuContents,
        gameOverContents,
        scoreContents,
        storeContents;

    [Inject] CoinSpawner coinSpawner;
    [Inject] MissileSpawner missileSpawner;
    [Inject] Plane plane;
    [Inject] UserInput userInput;
    [Inject] MenuRadioGroup menuRadio;

    float autoContinueTimer = 3f;
    Coroutine autoContinueCoroutine;

    void Start()
    {
        MessageBroker.Default.Receive<Plane.msgExploded>()
            .Subscribe(msg => ShowGameOver())
            .AddTo(this);

        ShowMainMenu();
    }

    public void ShowStore()
    {
        menuRadio.RadioToggle(storeContents);
    }

    public void StartGame()
    {
        missileSpawner.StartSpawn();

        userInput.gameObject.SetActive(true);

        menuRadio.AllToggleOff();
    }

    void ShowGameOver()
    {
        Debug.Log($"showGameObver");
        
        menuRadio.RadioToggle(gameOverContents);

        userInput.gameObject.SetActive(false);

        Time.timeScale = 0.1f;

        autoContinueCoroutine = StartCoroutine(AutoContinue());
    }

    IEnumerator AutoContinue()
    {
        for (float t = 0f;
             t < autoContinueTimer;
             t += Time.unscaledDeltaTime)
        {
            continueWheel.fillAmount = t / autoContinueTimer;
            yield return null;
        }

        Repair();
    }

    [DllImport("__Internal")]
    private static extern void ShowAd();

    public void Repair()
    {
        menuRadio.AllToggleOff();

        plane.Repair();

        userInput.gameObject.SetActive(true);

        Time.timeScale = 1f;

        StopCoroutine(autoContinueCoroutine);
    }

    public void EndGame()
    {
        FindObjectsOfType<Missile>()
            .ToList()
            .ForEach(m => m.Explode());

        missileSpawner.StopSpawn();
        coinSpawner.ClearPool();

        menuRadio.AllToggleOff();

        StopCoroutine(autoContinueCoroutine);
        Invoke(nameof(ShowScore), 1.5f);

        userInput.gameObject.SetActive(false);

        Time.timeScale = 1f;
    }

    void ShowScore()
    {
        menuRadio.RadioToggle(scoreContents);
        userInput.gameObject.SetActive(false);
    }

    public void ShowMainMenu()
    {
        menuRadio.RadioToggle(mainMenuContents);
       
        plane.Steer(Vector3.up);
        plane.Repair();

        userInput.gameObject.SetActive(false);
    }
}
