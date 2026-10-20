using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class DamageTextPool : MonoBehaviour
{
    public static DamageTextPool Instance;

    private ObjectPool<DamagePopUp> textPool;

    [SerializeField] private DamagePopUp popUpPrefab;
    [SerializeField] private int preWarmAmount = 30;

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

    private void Start()
    {
        textPool = new ObjectPool<DamagePopUp>
            (
                createFunc: () => Instantiate(popUpPrefab, transform),
                actionOnGet: (item) => item.gameObject.SetActive(true),
                actionOnRelease: (item) => item.gameObject.SetActive(false),
                actionOnDestroy: (item) => Destroy(item.gameObject),
                collectionCheck: true,
                defaultCapacity: 20,
                maxSize: 120
            );

        List<DamagePopUp> tempList = new(preWarmAmount);

        for (int j = 0; j < preWarmAmount; j++)
        {
            tempList.Add(GetPopUp(Vector3.down * 9999));
        }

        for (int j = 0; j < tempList.Count; j++)
        {
            ReturnToPool(tempList[j]);
        }
    }

    public DamagePopUp GetPopUp(Vector3 position, Transform parent = null)
    {
        DamagePopUp newEffect = textPool.Get();
        if (parent != null) newEffect.transform.SetParent(parent);
        newEffect.transform.SetPositionAndRotation(position, Quaternion.identity);
        return newEffect;
    }

    public void ReturnToPool(DamagePopUp instance)
    {
        if (instance.transform.parent != transform) instance.transform.SetParent(transform);
        textPool.Release(instance);
    }
}
