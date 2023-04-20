using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class Vendible : MonoBehaviour
{
    [SerializeField] int cost;

    [Inject] Vault vault;



    public bool bought {get; protected set;}

    public event Action<bool> onUpdateAffordable;

    void Start()
    {
        vault.onCoinsChanged += UpdateInteractable;
    }

    public void Buy()
    {
        vault.SpendCoin(cost);
        bought = true;
    }

    void UpdateInteractable(int currentCoins)
    {
        bool canAfford = currentCoins >= cost;
        onUpdateAffordable?.Invoke(canAfford);
    }
}
