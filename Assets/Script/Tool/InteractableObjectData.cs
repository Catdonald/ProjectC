using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InteractableObj", menuName = "Sriptable Object / Interactable data")]
public class InteractableObjectData : ScriptableObject
{
    public enum ObjectType { Cooker, Counter, Table, TrashCan }

    [Header("# Main Info")]
    public ObjectType objectType;
    public int objectID;
    public string objectName;

    [Header("# Level Data")]
    public float baseCount;
    public float[] capacity;
    public float[] prices;

    [Header("# Prefab Data")]
    public GameObject prefab;
}
