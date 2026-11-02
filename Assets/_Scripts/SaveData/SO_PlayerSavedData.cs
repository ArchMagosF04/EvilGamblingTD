using Alchemy.Inspector;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSavedData", menuName = "Scriptable Objects/Save Data")]
public class SO_PlayerSavedData : ScriptableObject
{
    public int gachaCoins;

    [Space(10)]

    public SO_TowerData Slot1Tower;
    public SO_TowerData Slot2Tower;
    public SO_TowerData Slot3Tower;
    public SO_TowerData Slot4Tower;
    public SO_TowerData Slot5Tower;

    [Space(10)]

    public SerializationDictionaryWrapper<TowerIndex, bool> towersUnlocked;

    public Dictionary<TowerIndex, bool> TowersUnlockedDict;

    public void SetDefaultValues()
    {
        gachaCoins = 500;

        TowersUnlockedDict = new Dictionary<TowerIndex, bool>();

        FillDictionary();

        towersUnlocked = new SerializationDictionaryWrapper<TowerIndex, bool>(TowersUnlockedDict);
    }

    public void SaveToWrapper()
    {
        towersUnlocked = new SerializationDictionaryWrapper<TowerIndex, bool>(TowersUnlockedDict);
    }

    public void LoadToDictionary()
    {
        TowersUnlockedDict = towersUnlocked.ConvertToDictionary();
    }

    private void FillDictionary()
    {
        TowersUnlockedDict.Add(TowerIndex.Demon, true);
        TowersUnlockedDict.Add(TowerIndex.DarkElf, false);
        TowersUnlockedDict.Add(TowerIndex.Siren, true);
        TowersUnlockedDict.Add(TowerIndex.Vampire, false);
        TowersUnlockedDict.Add(TowerIndex.Mummy, true);
        TowersUnlockedDict.Add(TowerIndex.Fairy, false);
        TowersUnlockedDict.Add(TowerIndex.Chimera, false);
    }

    public void ReceiveGachaCoins(int coins)
    {
        gachaCoins += coins;

        if (gachaCoins > 999999999) gachaCoins = 999999999;
    }

    public void SpendGachaCoins(int coins)
    {
        gachaCoins -= coins;

        if (gachaCoins < 0) gachaCoins = 0;
    }

    public bool HasEnoughCoins(int coins)
    {
        if (gachaCoins >= coins)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsTowerUnlocked(TowerIndex index)
    {
        if (TowersUnlockedDict.ContainsKey(index))
        {
            return TowersUnlockedDict[index];
        }

        Debug.LogWarning($"Index: {index} not found in Dictionary");

        return false;
    }

    [Button]
    public void UnlockTower(TowerIndex index)
    {
        if (TowersUnlockedDict.ContainsKey(index))
        {
            TowersUnlockedDict[index] = true;
        }
        else
        {
            Debug.LogWarning($"Index: {index} not found in Dictionary");
        }
    }
}

[Serializable]
public class SerializationDictionaryWrapper<TKey, TValue>
{
    public List<TKey> Keys = new List<TKey>();
    public List<TValue> Values = new List<TValue>();

    public SerializationDictionaryWrapper(Dictionary<TKey, TValue> dictionary)
    {
        foreach (KeyValuePair<TKey, TValue> kvp  in dictionary)
        {
            Keys.Add(kvp.Key);
            Values.Add(kvp.Value);
        }
    }

    public Dictionary<TKey, TValue> ConvertToDictionary()
    {
        Dictionary<TKey, TValue> dict = new Dictionary<TKey, TValue>();

        for (int i = 0; i < Keys.Count; i++)
        {
            dict[Keys[i]] = Values[i];
        }

        return dict;
    }
}
