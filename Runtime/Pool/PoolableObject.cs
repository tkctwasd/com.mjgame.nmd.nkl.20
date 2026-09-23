using UnityEngine;

public class PoolableObject : MonoBehaviour, IPoolable
{

    [SerializeField] private bool autoReturnToPool = false;
    [SerializeField] private float autoReturnDelay = 5f;

    private float spawnTime;

    public string PoolId { get; set; }

    private void Update()
    {
        if (autoReturnToPool && Time.time - spawnTime >= autoReturnDelay)
        {
            ReturnToPool();
        }
    }

    public virtual void OnSpawnFromPool()
    {
        spawnTime = Time.time;
        OnSpawn();
    }

    public virtual void OnReturnToPool()
    {
        OnDespawn();
    }

    public void ReturnToPool()
    {
        if (!string.IsNullOrEmpty(PoolId))
        {
            PoolManager.Instance.ReturnToPool(PoolId, gameObject);
        }
        else
        {
            Debug.LogWarning($"PoolableObject {gameObject.name} has no poolId set!");
        }
    }

    // Override these methods in derived classes
    protected virtual void OnSpawn() { }
    protected virtual void OnDespawn() { }
}
