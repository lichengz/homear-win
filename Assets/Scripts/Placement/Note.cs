using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HomeAR.Managers;

[System.Serializable]
public class Note
{
    // public Table dict;
    public SerializableDictionary<User, StringListWrapper> dict;

    // public SerializableDictionary<User, string> test = new SerializableDictionary<User, string>();
    public void InsertNote(User user, string text)
    {
        if (!dict.ContainsKey(user))
        {
            dict.Add(user, new StringListWrapper());
        }
        dict[user].notes.Add(text);
    }
}

[System.Serializable]
public class StringListWrapper
{
    public List<string> notes;
}