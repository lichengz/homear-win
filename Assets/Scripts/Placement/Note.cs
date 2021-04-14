using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HomeAR.Managers;

[System.Serializable]
public class Note
{
    // public Table dict;
    public SerializableDictionary<User, List<string>> dict;

    // public SerializableDictionary<User, string> test = new SerializableDictionary<User, string>();
    public void InsertNote(User user, string text)
    {
        if (!dict.ContainsKey(user))
        {
            dict.Add(user, new List<string>());
        }
        dict[user].Add(text);
        Debug.Log(string.Format("{0} is added to {1}'s notes", text, user.name));
    }
}

[System.Serializable]
public class StringListWrapper
{
    public List<string> notes;
}