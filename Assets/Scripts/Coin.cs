using System;
using UnityEngine;
using Zenject;
using UniRx;

public class Coin : MonoBehaviour
{
    public interface ICoinVault
    {
        static public event Action<int> onCoinsChanged;
        public void EarnCoin(int amount);
        public void SpendCoin(int amount);
    }

    [Inject] ICoinVault coinVault;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponentInParent<Plane>() is not null)
        {
            coinVault.EarnCoin(1);
            MessageBroker.Default.Publish(new Score.msgScoreChange{ amount = 10 });
        }
    }

    public class Factory : PlaceholderFactory<Coin> {}
}
