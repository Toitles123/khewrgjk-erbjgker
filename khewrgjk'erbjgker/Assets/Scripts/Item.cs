using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public class Item : ScriptableObject
{
    public string id;
    public GameObject gameObject;
    public int maxStack;
    public Vector3 UIPosition;
    public Vector3 UIRotation;
    public Vector3 UIScale = Vector3.one;
}
