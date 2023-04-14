using UnityEngine;
using UnityEngine.Pool;
using Zenject;

public class ExplosionSpawner : MonoBehaviour
{
    [Inject] ExplosionFactory explosionFactory;

    IObjectPool<GameObject> pool;

    void Awake()
    {
        pool = new ObjectPool<GameObject>(CreateExplosion,
                                          OnGet,
                                          OnRelease,
                                          OnExplosionDestroy,
                                          collectionCheck: true,
                                          defaultCapacity: 30,
                                          maxSize: 2000);
    }

    public void ClearPool()
    {
        pool.Clear();
    }

    public void Spawn(Vector3 position)
    {
        var newExplosion = pool.Get().GetComponent<ParticleSystem>();

        newExplosion.transform.position = position;

        newExplosion.Play();
    }

    GameObject CreateExplosion()
    {
        var newExplosion = explosionFactory.Create();

        newExplosion.gameObject.AddComponent<ExplosionReturnToPool>().pool = pool;

        return newExplosion.gameObject;
    }

    void OnGet(GameObject coin)
    {
        coin.SetActive(true);
    }

    void OnRelease(GameObject coin)
    {
        coin.SetActive(false);
    }

    void OnExplosionDestroy(GameObject coin)
    {
    }

    public class ExplosionFactory : PlaceholderFactory<ParticleSystem>
    {
    }
}
