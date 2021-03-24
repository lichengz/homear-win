using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class User
{
    public int index;
    public string name;

    public User(int index, string name)
    {
        this.index = index;
        this.name = name;
    }
}
