using System;
using System.Collections.Generic;
using UnityEngine;

public class TrashHandler : MonoBehaviour
{
    #region Fields and Properties

    public static TrashHandler Instance { get; private set; }

    [field: SerializeField] public List<TrashDataCollection> TrashWaves { get; private set; } = new();
    [SerializeField] private Trash _trashPrefab;
    [SerializeField] private Transform _trashSpawn;
    [SerializeField] private float _maxSpeed = 100f;

    public List<Trash> AvailableTrash = new();

    #endregion

    #region Events

    public static event Action OnSpawnNewTrash;

    #endregion

    #region Methods

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        SpawnNextTrashWave();
    }

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
        if (TrashWaves.Count > 0)
        {
            var trashWave = TrashWaves[0];
            TrashWaves.RemoveAt(0);
            InstantiateTrashWave(trashWave);
        }
    }

    private void InstantiateTrashWave(TrashDataCollection trashCollection)
    {
        foreach (var trash in trashCollection.TrashList)
            InstantiateTrash(trash);
    }

    private void InstantiateTrash(TrashData trashData)
    {
        var trash = Instantiate(_trashPrefab, _trashSpawn.position, Quaternion.identity);
        trash.SetUp(trashData);

        float speed = UnityEngine.Random.Range(0, _maxSpeed);
        trash.GetComponent<Rigidbody>().AddForce(Vector3.right * speed);

        AvailableTrash.Add(trash);
    }

    public static void RaiseSpawnNewTrash()
    {
        OnSpawnNewTrash?.Invoke();
    }

    #endregion
}
