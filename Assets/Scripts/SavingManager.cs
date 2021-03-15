using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SavingManager : MonoBehaviour
{
    public static SavingManager instance;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    [SerializeField]
    Object placementObject;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;

        }
        else if (instance != this)
        {
            Destroy(this);
            DontDestroyOnLoad(this);
        }
    }

    public bool IsSaveFile()
    {
        return Directory.Exists(Path.Combine(Application.persistentDataPath, "game_save"));
    }

    public void SaveGame()
    {
        if (!IsSaveFile())
        {
            Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, "game_save"));
        }
        if (!Directory.Exists(Path.Combine(Application.persistentDataPath, "game_save/objects_data")))
        {
            Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, "game_save/objects_data"));
        }
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Path.Combine(Application.persistentDataPath, "game_save/objects_data/objects_save"));
        var json = JsonUtility.ToJson(placementObject);
        bf.Serialize(file, json);
        file.Close();
    }

    public void LoadGame()
    {
        if (!IsSaveFile()) return;
        BinaryFormatter bf = new BinaryFormatter();
        if (File.Exists(Path.Combine(Application.persistentDataPath, "game_save/objects_data/objects_save")))
        {
            FileStream file = File.Open(Path.Combine(Application.persistentDataPath, "game_save/objects_data/objects_save"), FileMode.Open);
            JsonUtility.FromJsonOverwrite((string)bf.Deserialize(file), placementObject);
            file.Close();
        }
    }
}
