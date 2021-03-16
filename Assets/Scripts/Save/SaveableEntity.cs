using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveableEntity : MonoBehaviour
{
    [SerializeField]
    string id = string.Empty;
    public string Id => id;
    [ContextMenu("Generate Id")]
    void GenerateId() => id = System.Guid.NewGuid().ToString();
    public object CaptureState()
    {
        var state = new Dictionary<string, object>();
        foreach (var saveable in GetComponents<ISaveable>())
        {
            state[saveable.GetType().ToString()] = saveable.CaptureState();
        }
        return state;
    }
    public void RestoreState(object state)
    {
        var stateDict = (Dictionary<string, object>)state;
        foreach (var saveable in GetComponents<ISaveable>())
        {
            string typeName = saveable.GetType().ToString();
            if (stateDict.TryGetValue(typeName, out object value))
            {
                saveable.RestoreState(value);
            }
        }
    }
}
