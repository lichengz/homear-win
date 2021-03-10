using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Home Object", menuName = "Home Object")]
public class QRCode : ScriptableObject
{
    public new string name;
    public string description;
    public Sprite QR;
}
