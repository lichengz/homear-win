using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Placement Object", menuName = "Placement Object")]
public class Object : ScriptableObject
{
    public GameObject prefab;
    public Vector3 position;
    public Quaternion rotation;
}
