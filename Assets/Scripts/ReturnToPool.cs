using UnityEngine;
using UnityEngine.Pool;

abstract public class ReturnToPool<T> : MonoBehaviour
    where T : Component
{
    public IObjectPool<GameObject> pool;
}
