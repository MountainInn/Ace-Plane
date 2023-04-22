using TMPro;
using UniRx;
using UnityEngine;

public class Score : MonoBehaviour
{
    public struct msgScoreChange { public int amount; }

    TextMeshProUGUI scoreLabel;

    int score;

    void Awake()
    {
        scoreLabel = GetComponent<TextMeshProUGUI>();

        MessageBroker.Default.Receive<msgScoreChange>()
            .Subscribe(OnScoreChange)
            .AddTo(this);
    }

    void OnScoreChange(msgScoreChange msg)
    {
        score += msg.amount;

        scoreLabel.text = $"Your score: {score}";
    }

    public void Reset()
    {
        score = 0;
    }
}
