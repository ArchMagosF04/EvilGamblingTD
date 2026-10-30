using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSavedData", menuName = "Scriptable Objects/Save Data")]
public class SO_PlayerSavedData : ScriptableObject
{
    public int gachaCoins;

    public SerializationDictionaryWrapper<TowerIndex, bool> towersUnlocked;



    public void SetDefaultValues()
    {
        gachaCoins = 500;

        Dictionary<TowerIndex, bool> defaultUnlocks = new Dictionary<TowerIndex, bool>();

        FillDictionary(ref defaultUnlocks);

        towersUnlocked = new SerializationDictionaryWrapper<TowerIndex, bool>(defaultUnlocks);
    }

    private void FillDictionary(ref Dictionary<TowerIndex, bool> dictionary)
    {
        dictionary.Add(TowerIndex.Demon, true);
        dictionary.Add(TowerIndex.DarkElf, true);
        dictionary.Add(TowerIndex.Siren, true);
        dictionary.Add(TowerIndex.Vampire, true);
        dictionary.Add(TowerIndex.Mummy, true);
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
