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
        if (col.TryGetComponent(out Plane plane))
        {
            coinVault.EarnCoin(1);
        }
    }

    public class Factory : PlaceholderFactory<Coin> {}
}
