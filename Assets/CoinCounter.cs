using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UniRx;

public class CoinCounter : MonoBehaviour
{
    TextMeshProUGUI countText;

    void Start()
    {
        countText = GetComponentInChildren<TextMeshProUGUI>();

        Vault.onCoinsChanged += SetCount;
    }

    public void SetCount(int count)
    {
        countText.text = count.ToString();
    }
}
