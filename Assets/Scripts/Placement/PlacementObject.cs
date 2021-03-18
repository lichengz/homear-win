﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class PlacementObject : MonoBehaviour, ISaveable
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
    public Annotation annotation;
    [SerializeField]
    TextMeshPro OverLayTextMesh;
    [SerializeField]
    string OverLayText;

    [Serializable]
    struct SerializableVector3
    {
        float x;
        float y;
        float z;

        public SerializableVector3(Vector3 vec)
        {
            x = vec.x;
            y = vec.y;
            z = vec.z;
        }

        public Vector3 restore()
        {
            return new Vector3(x, y, z);
        }
    }

    // Save data
    [Serializable]
    private struct SaveData
    {
        public SerializableVector3 position;
        public SerializableVector3 rotation;
        public SerializableVector3 scale;
    }

    // Annotation
    [Serializable]
    public struct Annotation
    {
        public Boolean isReminderActive;
        public Boolean isScheduleActive;
        public string reminderText;
    }

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

    // ISaveable
    public object CaptureState()
    {
        return new SaveData
        {
            position = new SerializableVector3(transform.position),
            rotation = new SerializableVector3(transform.localEulerAngles),
            scale = new SerializableVector3(transform.localScale)
        };
    }

    public void RestoreState(object state)
    {
        var saveData = (SaveData)state;
        transform.position = saveData.position.restore();
        transform.localEulerAngles = saveData.rotation.restore();
        transform.localScale = saveData.scale.restore();
    }

}
