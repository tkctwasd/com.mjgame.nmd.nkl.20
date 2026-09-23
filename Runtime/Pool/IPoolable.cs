using UnityEngine;

public interface IPoolable
{
    string PoolId { get; set; }
    void OnSpawnFromPool();
    void OnReturnToPool();
    void ReturnToPool();
}
