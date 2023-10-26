using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Trash Data Collection")]
public class TrashDataCollection : ScriptableObject
{
    [field: SerializeField] public List<TrashData> TrashList { get; private set; } = new();
}
