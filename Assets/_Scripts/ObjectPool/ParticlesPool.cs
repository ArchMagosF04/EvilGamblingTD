using Alchemy.Inspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ParticlesPool : MonoBehaviour
{
    public static ParticlesPool Instance;

    private readonly Dictionary<string, ObjectPool<PoolableEffect>> poolDictionary = new();

    [SerializeField] private EffectPoolPreWarm[] preWarmArray;

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

        PreWarmPools();
    }

    private void PreWarmPools()
    {
        for (int i = 0; i < preWarmArray.Length; i++)
        {
            if (poolDictionary.ContainsKey(preWarmArray[i].prefab.ID))
            {
                poolDictionary[preWarmArray[i].prefab.ID].Clear();
            }
            else
            {
                CreatePool(preWarmArray[i].prefab);
            }

            List<PoolableEffect> tempList = new(preWarmArray[i].amount);

            for (int j = 0; j < preWarmArray[i].amount; j++)
            {
                tempList.Add(GetEffect(preWarmArray[i].prefab, Vector3.down * 9999, Quaternion.identity));
            }

            for (int j = 0; j < tempList.Count; j++)
            {
                ReturnToPool(tempList[j].ID, tempList[j]);
            }
        }
    }

    public void PreWarmPool(PoolableEffect prefab, int amount)
    {
        if (poolDictionary.ContainsKey(prefab.ID))
        {
            return;
        }

        CreatePool(prefab);

        List<PoolableEffect> tempList = new(amount);

        for (int j = 0; j < amount; j++)
        {
            tempList.Add(GetEffect(prefab, Vector3.down * 9999, Quaternion.identity));
        }

        for (int j = 0; j < tempList.Count; j++)
        {
            ReturnToPool(tempList[j].ID, tempList[j]);
        }
    }

    public PoolableEffect GetEffect(PoolableEffect prefab, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(prefab.ID))
        {
            CreatePool(prefab);
        }

        PoolableEffect newEffect = poolDictionary[prefab.ID].Get();
        newEffect.transform.SetPositionAndRotation(position, rotation);
        return newEffect;
    }

    public void ReturnToPool(string key, PoolableEffect instance)
    {
        if (poolDictionary.TryGetValue(key, out var pool))
        {
            pool.Release(instance);
        }
        else
        {
            Debug.LogWarning($"No pool found for component prefabID: {key}. Destroying object.");
            Destroy(instance.gameObject);
        }
    }

    private void CreatePool(PoolableEffect prefab)
    {
        ObjectPool<PoolableEffect> newPool = new ObjectPool<PoolableEffect>
            (
                createFunc: () => Instantiate(prefab, transform),
                actionOnGet: (item) => item.gameObject.SetActive(true),
                actionOnRelease: (item) => item.gameObject.SetActive(false),
                actionOnDestroy: (item) => Destroy(item.gameObject),
                collectionCheck: true,
                defaultCapacity: 20,
                maxSize: 120
            );

        poolDictionary.Add(prefab.ID, newPool);
    }
}

[Serializable]
public struct EffectPoolPreWarm
{
    [AssetsOnly] public PoolableEffect prefab;
    [Min(0)] public int amount;
}
