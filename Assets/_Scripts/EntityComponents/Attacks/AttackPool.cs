using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class AttackPool : MonoBehaviour
{
    public static AttackPool Instance;

    private readonly Dictionary<string, ObjectPool<Projectile>> poolDictionary = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    public void PreWarmPool(Projectile prefab, int amount)
    {
        if (poolDictionary.ContainsKey(prefab.AttackData.ID))
        {
            return;
        }

        CreatePool(prefab);

        List<Projectile> tempList = new(amount);

        for (int j = 0; j < amount; j++)
        {
            tempList.Add(GetAttack(prefab, Vector3.down * 9999, Quaternion.identity));
        }

        for (int j = 0; j < tempList.Count; j++)
        {
            ReturnToPool(tempList[j].AttackData.ID, tempList[j]);
        }
    }

    public Projectile GetAttack(Projectile prefab, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        if (!poolDictionary.ContainsKey(prefab.AttackData.ID))
        {
            CreatePool(prefab);
        }

        Projectile newEffect = poolDictionary[prefab.AttackData.ID].Get();
        if (parent != null) newEffect.transform.SetParent(parent);
        newEffect.transform.SetPositionAndRotation(position, rotation);
        return newEffect;
    }

    public void ReturnToPool(string key, Projectile instance)
    {
        if (poolDictionary.TryGetValue(key, out var pool))
        {
            if (instance.transform.parent != transform) instance.transform.SetParent(transform);
            pool.Release(instance);
        }
        else
        {
            Debug.LogWarning($"No pool found for component prefabID: {key}. Destroying object.");
            Destroy(instance.gameObject);
        }
    }

    private void CreatePool(Projectile prefab)
    {
        ObjectPool<Projectile> newPool = new ObjectPool<Projectile>
            (
                createFunc: () => Instantiate(prefab, transform),
                actionOnGet: (item) => item.gameObject.SetActive(false),
                actionOnRelease: (item) => item.gameObject.SetActive(false),
                actionOnDestroy: (item) => Destroy(item.gameObject),
                collectionCheck: true,
                defaultCapacity: 50,
                maxSize: 150
            );

        poolDictionary.Add(prefab.AttackData.ID, newPool);
    }
}
