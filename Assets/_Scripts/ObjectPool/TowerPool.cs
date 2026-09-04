using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class TowerPool : MonoBehaviour
{
    public static TowerPool Instance;

    private readonly Dictionary<int, ObjectPool<TowerController>> poolDictionary = new();

    [SerializeField] private SO_SelectedTowersArray selectedTowersArray;
    [SerializeField, Min(0)] private int preWarmTowersAmount = 5;

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

        PreWarmSelectedTowerPool(selectedTowersArray.Slot1Tower);
        PreWarmSelectedTowerPool(selectedTowersArray.Slot2Tower);
        PreWarmSelectedTowerPool(selectedTowersArray.Slot3Tower);
        PreWarmSelectedTowerPool(selectedTowersArray.Slot4Tower);
        PreWarmSelectedTowerPool(selectedTowersArray.Slot5Tower);
    }

    private void PreWarmSelectedTowerPool(SO_TowerData data)
    {
        if (data != null)
        {
            if (poolDictionary.ContainsKey(data.ID))
            {
                poolDictionary[data.ID].Clear();
            }
            else
            {
                CreatePool(data.TowerPrefab);
            }

            List<TowerController> tempList = new(preWarmTowersAmount);

            for (int j = 0; j < preWarmTowersAmount; j++)
            {
                tempList.Add(GetTower(data.TowerPrefab, Vector3.down * 9999, Quaternion.identity));
            }

            for (int j = 0; j < tempList.Count; j++)
            {
                ReturnToPool(tempList[j].TowerData.ID, tempList[j]);
            }
        }
        else
        {
            Debug.LogWarning("Empty Tower Slot");
        }
    }

    public TowerController GetTower(TowerController prefab, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        if (!poolDictionary.ContainsKey(prefab.TowerData.ID))
        {
            CreatePool(prefab);
        }

        TowerController newEffect = poolDictionary[prefab.TowerData.ID].Get();
        if (parent != null) newEffect.transform.SetParent(parent);
        newEffect.transform.SetPositionAndRotation(position, rotation);
        return newEffect;
    }

    public void ReturnToPool(int key, TowerController instance)
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

    private void CreatePool(TowerController prefab)
    {
        ObjectPool<TowerController> newPool = new ObjectPool<TowerController>
            (
                createFunc: () => Instantiate(prefab, transform),
                actionOnGet: (item) => item.gameObject.SetActive(true),
                actionOnRelease: (item) => item.gameObject.SetActive(false),
                actionOnDestroy: (item) => Destroy(item.gameObject),
                collectionCheck: true,
                defaultCapacity: 50,
                maxSize: 150
            );

        poolDictionary.Add(prefab.TowerData.ID, newPool);
    }
}
