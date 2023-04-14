using System.Runtime.InteropServices;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using System.Linq;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    Image
        continueWheel;
   
    [SerializeField]
    Button
        startGameButton,
        repairButton,
        endGameButton,
        mainMenuButton;

    [SerializeField]
    GameObject
        screen,
        mainMenuContents,
        gameOverContents,
        scoreContents;

    CoinSpawner coinSpawner;
    MissileSpawner missileSpawner;
    Plane plane;
    UserInput userInput;

    float autoContinueTimer = 3f;
    Coroutine autoContinueCoroutine;

    [Inject]
    public void Construct(CoinSpawner coinSpawner, MissileSpawner missileSpawner, Plane plane, UserInput userInput)
    {
        this.coinSpawner = coinSpawner;
        this.missileSpawner = missileSpawner;
        this.plane = plane;
        this.userInput = userInput;
    }

    void Start()
    {
        MessageBroker.Default.Receive<Plane.msgExploded>()
            .Subscribe(msg => ShowGameOver())
            .AddTo(this);

        startGameButton.onClick.AddListener(()=>StartGame());
        repairButton.onClick.AddListener(()=>Repair());
        endGameButton.onClick.AddListener(()=>EndGame());
        mainMenuButton.onClick.AddListener(()=>ShowMainMenu());

        HideAllContents();
        ShowMainMenu();
    }


    void StartGame()
    {
        HideAllContents();

        missileSpawner.StartSpawn();

        userInput.gameObject.SetActive(true);
    }

    void ShowGameOver()
    {
        HideAllContents();

        screen.SetActive(true);
        gameOverContents.gameObject.SetActive(true);

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

    void Repair()
    {
        HideAllContents();

        plane.Repair();

        userInput.gameObject.SetActive(true);

        Time.timeScale = 1f;

        StopCoroutine(autoContinueCoroutine);
    }

    void EndGame()
    {
        FindObjectsOfType<Missile>()
            .ToList()
            .ForEach(m => m.Explode());

        plane.Explode();

        missileSpawner.StopSpawn();
        coinSpawner.ClearPool();

        Invoke(nameof(ShowScore), 1.5f);

        HideAllContents();

        userInput.gameObject.SetActive(false);

        Time.timeScale = 1f;
    }

    void ShowScore()
    {
        HideAllContents();

        screen.SetActive(true);
        scoreContents.gameObject.SetActive(true);
        userInput.gameObject.SetActive(false);
    }

    void ShowMainMenu()
    {
        HideAllContents();

        plane.Steer(Vector3.up);
        plane.Repair();

        screen.SetActive(true);
        mainMenuContents.SetActive(true);
        userInput.gameObject.SetActive(false);
    }

    void HideAllContents()
    {
        screen.SetActive(false);
        gameOverContents.SetActive(false);
        mainMenuContents.SetActive(false);
        scoreContents.SetActive(false);
    }
}
