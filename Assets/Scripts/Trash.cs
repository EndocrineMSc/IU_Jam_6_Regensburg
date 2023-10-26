using System.Collections;
using System.Collections.Generic;
using System.Runtime.Versioning;
using UnityEngine;

/// <summary>
/// Trash needs to:
/// - have a energy equivalent
/// - have a size
/// - have a consumption speed
/// - have a consumption progress
/// </summary>
public class Trash : MonoBehaviour
{
    #region Fields and Properties

    public TrashData Data { get; private set; }

    public const float CONSUMPTION_SPEED = 5f;

    public float ConsumptionProgress { get; private set; } = 0;
    private float _energy = 0;
    private bool _isSetUp;

    #endregion

    #region Methods

    private void SetUp(TrashData data)
    {
        Data = data;
        _energy = Data.Energy;
        _isSetUp = true;
    }

    public float ConsumeTrash()
    {
        var consumedEnergy = Time.deltaTime * CONSUMPTION_SPEED;

        _energy -= consumedEnergy;
        return consumedEnergy;
    }

    private void Update()
    {
        if (_isSetUp && _energy <= 0)
            Destroy(gameObject);
    }

    #endregion
}
