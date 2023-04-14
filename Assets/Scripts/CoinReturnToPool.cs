using UnityEngine;

public class CoinReturnToPool : ReturnToPool<Coin>
{
    public CoinReturnToPool()
    {
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.TryGetComponent(out Plane plane))
        {
            pool.Release(gameObject);
        }
    }
}
