using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Placement Object", menuName = "Placement Object")]
public class PlacedObject : ScriptableObject
{
    public GameObject prefab;
    public Sprite icon;
    public string text;
}
