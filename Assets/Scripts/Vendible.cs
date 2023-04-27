using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class Vendible : MonoBehaviour
{
    [SerializeField] public int cost;

    [Inject] Vault vault;

    public bool isAffordable {get; protected set;}
    public bool isBought {get; protected set;}

    public event Action<bool> onUpdateAffordable, onBought;

    void Start()
    {
        vault.onCoinsChanged += UpdateAffordable;
        UpdateAffordable(vault.coins);
    }

    public void Initialize(Action updateInteractable)
    {
        onUpdateAffordable += (_) => updateInteractable.Invoke();
        onBought += (_) => updateInteractable.Invoke();
        updateInteractable.Invoke();
    }

    public void Buy()
    {
        vault.SpendCoin(cost);
        isBought = true;

        onBought?.Invoke(isBought);
    }

    void UpdateAffordable(int currentCoins)
    {
        isAffordable = ( currentCoins >= cost );
        onUpdateAffordable?.Invoke(isAffordable);
    }
}
