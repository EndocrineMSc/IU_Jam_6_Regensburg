using DG.Tweening;
using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashHandler : MonoBehaviour
{
    #region Fields and Properties

    public static TrashHandler Instance { get; private set; }

    [field: SerializeField] public List<TrashDataCollection> TrashWaves { get; private set; } = new();
    [SerializeField] private Trash _trashPrefab;
    [SerializeField] private Transform _trashSpawn;
    [SerializeField] private float _speed = 250;

    [SerializeField] private Transform _hatch;
    Vector3 _openHatchRotation = new(-40, -90, 0);
    Vector3 _closedHatchRotation = new(-90, -90, 0);

    public List<Trash> AvailableTrash = new();

    [SerializeField] private EventReference _hatchSound;

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
        StartCoroutine(TiltHatchThenSpawnWave());
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

        trash.GetComponent<Rigidbody>().AddForce(Vector3.right * _speed); 

        AvailableTrash.Add(trash);
    }

    public static void RaiseSpawnNewTrash()
    {
        OnSpawnNewTrash?.Invoke();
    }

    private IEnumerator TiltHatchThenSpawnWave()
    {
        if (TrashWaves.Count > 0)
        {
            var trashWave = TrashWaves[0];
            TrashWaves.RemoveAt(0);
            _hatch.DORotate(_openHatchRotation, 2f);
            AudioManager.Instance.PlayOneShot(_hatchSound, _hatch.position);
            yield return new WaitForSeconds(2);
            InstantiateTrashWave(trashWave);
            yield return new WaitForSeconds(0.2f);
            _hatch.DORotate(_closedHatchRotation, 2f);
            AudioManager.Instance.PlayOneShot(_hatchSound, _hatch.position);
        }
    }

    #endregion
}
