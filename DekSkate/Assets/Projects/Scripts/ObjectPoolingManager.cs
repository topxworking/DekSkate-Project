using UnityEngine;
using UnityEngine.Pool;
using System.Collections.Generic;

public class ObjectPoolingManager : MonoBehaviour
{
    [System.Serializable]
    public class PoolConfig
    {
        public string id;
        public GameObject prefab;
    }

    public List<PoolConfig> configs;
    private Dictionary<string, IObjectPool<GameObject>> _pools = new Dictionary<string, IObjectPool<GameObject>>();

    void Awake()
    {
        foreach (var config in configs)
        {
            GameObject prefabToSpawn = config.prefab;
            var pool = new ObjectPool<GameObject>(
                createFunc: () => Instantiate(prefabToSpawn),
                actionOnGet: obj => obj.SetActive(true),
                actionOnRelease: obj => obj.SetActive(false)
            );
            _pools.Add(config.id, pool);
        }
    }

    public GameObject GetFromPool(string id)
    {
        if (_pools.ContainsKey(id)) return _pools[id].Get();
        Debug.LogError($"ไม่พบ Pool ที่มี ID: {id}");
        return null;
    }
}
