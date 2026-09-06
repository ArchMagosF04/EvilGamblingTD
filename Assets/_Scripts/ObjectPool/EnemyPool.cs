using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyPool : MonoBehaviour
{
    public static EnemyPool Instance;

    private readonly Dictionary<int, ObjectPool<EnemyController>> poolDictionary = new();

    [SerializeField] private EnemyPoolPreWarm[] preWarmArray;

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
            if (poolDictionary.ContainsKey(preWarmArray[i].prefab.EnemyData.ID))
            {
                poolDictionary[preWarmArray[i].prefab.EnemyData.ID].Clear();
            }
            else
            {
                CreatePool(preWarmArray[i].prefab);
            }

            List<EnemyController> tempList = new(preWarmArray[i].amount);

            for (int j = 0; j < preWarmArray[i].amount; j++)
            {
                tempList.Add(GetEnemy(preWarmArray[i].prefab, Vector3.down * 9999, Quaternion.identity));
            }

            for (int j = 0; j < tempList.Count; j++)
            {
                ReturnToPool(tempList[j].EnemyData.ID, tempList[j]);
            }
        }
    }

    public EnemyController GetEnemy(EnemyController prefab, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        if (!poolDictionary.ContainsKey(prefab.EnemyData.ID))
        {
            CreatePool(prefab);
        }

        EnemyController newEffect = poolDictionary[prefab.EnemyData.ID].Get();
        if (parent != null) newEffect.transform.SetParent(parent);
        newEffect.transform.SetPositionAndRotation(position, rotation);
        return newEffect;
    }

    public void ReturnToPool(int key, EnemyController instance)
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

    private void CreatePool(EnemyController prefab)
    {
        ObjectPool<EnemyController> newPool = new ObjectPool<EnemyController>
            (
                createFunc: () => Instantiate(prefab, transform),
                actionOnGet: (item) => item.gameObject.SetActive(false),
                actionOnRelease: (item) => item.gameObject.SetActive(false),
                actionOnDestroy: (item) => Destroy(item.gameObject),
                collectionCheck: true,
                defaultCapacity: 50,
                maxSize: 150
            );

        poolDictionary.Add(prefab.EnemyData.ID, newPool);
    }
}

[Serializable]
public struct EnemyPoolPreWarm
{
    public EnemyController prefab;
    [Min(0)] public int amount;
}
