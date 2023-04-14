using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;

public class CoinSpawner : MonoBehaviour
{
    [Inject] Coin.Factory coinFactory;

    IObjectPool<GameObject> pool;

    void Awake()
    {
        pool = new ObjectPool<GameObject>(CreateCoin,
                                          OnGet,
                                          OnRelease,
                                          OnCoinDestroy,
                                          collectionCheck: true,
                                          defaultCapacity: 30,
                                          maxSize: 2000);
    }

    public void ClearPool()
    {
        FindObjectsOfType<Coin>()
            .ToList()
            .ForEach(c => Destroy(c.gameObject));

        pool.Clear();
    }

    public void Spawn(Vector3 position)
    {
        var newCoin = pool.Get();

        newCoin.transform.position = position;
    }

    GameObject CreateCoin()
    {
        var newCoin = coinFactory.Create();

        newCoin.gameObject.AddComponent<CoinReturnToPool>().pool = pool;

        return newCoin.gameObject;
    }

    void OnGet(GameObject coin)
    {
        coin.SetActive(true);
    }

    void OnRelease(GameObject coin)
    {
        coin.SetActive(false);
    }

    void OnCoinDestroy(GameObject coin)
    {
        Destroy(coin.gameObject);
    }

}
