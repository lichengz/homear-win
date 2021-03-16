using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SavingManager : MonoBehaviour
{
    string savePath => $"{Application.persistentDataPath}/save";
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

    public void SaveGame()
    {
        var state = LoadFile();
        CaptureState(state);
        SaveFile(state);
    }
    public void LoadGame()
    {
        var state = LoadFile();
        RestoreState(state);
    }

    void SaveFile(object state)
    {
        using (var stream = File.Open(savePath, FileMode.Create))
        {
            var bf = new BinaryFormatter();
            bf.Serialize(stream, state);
        }
    }
    Dictionary<string, object> LoadFile()
    {
        if (!File.Exists(savePath))
        {
            return new Dictionary<string, object>();
        }
        using (var stream = File.Open(savePath, FileMode.Open))
        {
            var bf = new BinaryFormatter();
            return (Dictionary<string, object>)bf.Deserialize(stream);
        }
    }

    void CaptureState(Dictionary<string, object> state)
    {
        foreach (var saveable in FindObjectsOfType<SaveableEntity>())
        {
            state[saveable.Id] = saveable.CaptureState();
        }
    }
    void RestoreState(Dictionary<string, object> state)
    {
        foreach (var saveable in FindObjectsOfType<SaveableEntity>())
        {
            if (state.TryGetValue(saveable.Id, out object value))
            {
                saveable.RestoreState(value);
            }
        }
    }

    // public bool IsSaveFile()
    // {
    //     return Directory.Exists(Path.Combine(Application.persistentDataPath, "game_save"));
    // }

    // public void SaveGame()
    // {
    //     if (!IsSaveFile())
    //     {
    //         Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, "game_save"));
    //     }
    //     if (!Directory.Exists(Path.Combine(Application.persistentDataPath, "game_save/objects_data")))
    //     {
    //         Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, "game_save/objects_data"));
    //     }
    //     BinaryFormatter bf = new BinaryFormatter();
    //     FileStream file = File.Create(Path.Combine(Application.persistentDataPath, "game_save/objects_data/objects_save"));
    //     var json = JsonUtility.ToJson(placementObject);
    //     bf.Serialize(file, json);
    //     file.Close();
    // }

    // public void LoadGame()
    // {
    //     if (!IsSaveFile()) return;
    //     BinaryFormatter bf = new BinaryFormatter();
    //     if (File.Exists(Path.Combine(Application.persistentDataPath, "game_save/objects_data/objects_save")))
    //     {
    //         FileStream file = File.Open(Path.Combine(Application.persistentDataPath, "game_save/objects_data/objects_save"), FileMode.Open);
    //         JsonUtility.FromJsonOverwrite((string)bf.Deserialize(file), placementObject);
    //         file.Close();
    //     }
    // }
}
