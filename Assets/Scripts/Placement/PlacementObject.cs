﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlacementObject : MonoBehaviour
{
    [SerializeField]
    bool Selected;
    public Vector3 oriScale { get; set; }
    public int curScale { get; set; }
    public bool IsSelected
    {
        get
        {
            return this.Selected;
        }
        set
        {
            this.Selected = value;
            OverLayTextMesh.gameObject.SetActive(this.Selected);
        }
    }
    [SerializeField]
    TextMeshPro OverLayTextMesh;
    [SerializeField]
    string OverLayText;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        OverLayTextMesh = GetComponentInChildren<TextMeshPro>();
        if (OverLayTextMesh != null)
        {
            OverLayTextMesh.text = "Just a " + gameObject.name;
            OverLayTextMesh.gameObject.SetActive(false);
        }

    }

}
