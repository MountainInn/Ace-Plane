using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UniRx;
using Zenject;

public class CoinCounter : MonoBehaviour
{
    TextMeshProUGUI countText;

    [Inject] Vault vault;

    void Start()
    {
        countText = GetComponentInChildren<TextMeshProUGUI>();

        vault.onCoinsChanged += SetCount;
    }

    public void SetCount(int count)
    {
        countText.text = count.ToString();
    }
}
