using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashSpawner : MonoBehaviour
{
    #region Fields and Properties

    [field: SerializeField] public List<TrashDataCollection> TrashWaves { get; private set; } 

    #endregion

    #region Events

    public static event Action OnSpawnNewTrash;

    #endregion

    #region Methods

    private void OnEnable()
    {
        OnSpawnNewTrash += SpawnNextTrashWave;
    }

    private void OnDisable()
    {
        OnSpawnNewTrash -= SpawnNextTrashWave;
    }

    private void SpawnNextTrashWave()
    {

    }





    public static void RaiseSpawnNewTrash()
    {
        OnSpawnNewTrash?.Invoke();
    }


    #endregion


}
