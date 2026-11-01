using System.IO;
using UnityEngine;

public class DataSaveManager : MonoBehaviour
{
    public static DataSaveManager Instance;

    [SerializeField] private SO_PlayerSavedData savedData;

    //Name of the settings file
    private const string SETTINGS_FILE_NAME = "playerSaveData.json";
    //Full path to the settings file in persistent data path.
    private string SettingsFilePath => Path.Combine(Application.persistentDataPath, SETTINGS_FILE_NAME);

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

        if (transform.parent != null) transform.SetParent(null);
        DontDestroyOnLoad(gameObject);

        if (savedData == null)
        {
            Debug.LogError("PlayerSavedData: Data asset not asigned! Please assign it in the Inspector");
        }

        LoadData();
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }

    public void LoadData()
    {
        if (File.Exists(SettingsFilePath))
        {
            // Read the text back from the file
            string json = File.ReadAllText(SettingsFilePath);

            // Convert the JSON string back into a C# object
            JsonUtility.FromJsonOverwrite(json, savedData);

            savedData.LoadToDictionary();

            Debug.Log($"Loaded Data.");
        }
        else
        {
            Debug.Log("No save file found, creating default");

            savedData.SetDefaultValues();
        }
    }

    public void SaveData()
    {
        if (savedData == null)
        {
            Debug.LogError("PlayerSavedData: Data asset not asigned! Please assign it in the Inspector");
            return;
        }

        savedData.SaveToWrapper();

        string json = JsonUtility.ToJson(savedData, true); //Serialize to JSON
        File.WriteAllText(SettingsFilePath, json/*, Encoding.UTF8*/);
        Debug.Log($"Settings saved to: {SettingsFilePath}");
    }

    public SO_PlayerSavedData GetSaveData()
    {
        return savedData;
    }
}
