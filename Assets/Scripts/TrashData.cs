using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Trash Data")]
public class TrashData : ScriptableObject
{
    [field: SerializeField] public float Size { get; private set; }
    [field: SerializeField] public float Energy { get; private set; }
}
