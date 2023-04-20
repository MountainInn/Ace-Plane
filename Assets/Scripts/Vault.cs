using UnityEngine;
using System;

public class Vault : MonoBehaviour, Coin.ICoinVault
{
    public int coins {get; protected set;}

    public event Action<int> onCoinsChanged;

    public void EarnCoin(int amount)
    {
        coins += amount;
        onCoinsChanged?.Invoke(coins);
    }

    public void SpendCoin(int amount)
    {
        if (amount >= coins) return;

        coins -= amount;
        onCoinsChanged?.Invoke(coins);
    }
}
