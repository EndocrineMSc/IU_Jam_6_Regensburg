using FMODUnity;
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

    public const float CONSUMPTION_SPEED = 50f;

    public float ConsumptionProgress { get; private set; } = 0;
    private float _energy = 0;
    private bool _isSetUp;

    private MeshRenderer _meshRenderer;

    #endregion

    #region Methods

    public void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    public void SetUp(TrashData data)
    {
        Data = data;
        _energy = Data.Energy;
        _isSetUp = true;
        transform.localScale = Data.Scale;
        SetLooks();
        AddCollider();
    }

    private void SetLooks()
    {
        GetComponent<MeshFilter>().mesh = Data.TrashMesh;
        GetComponent<MeshRenderer>().material = Data.TrashMaterial;
    }

    private void AddCollider()
    {
        switch (Data.Shape)
        {
            case ColliderShape.Capsule:
                gameObject.AddComponent<CapsuleCollider>(); 
                break;
            case ColliderShape.Box:
                gameObject.AddComponent<BoxCollider>();
                break;
            case ColliderShape.Sphere:
                gameObject.AddComponent<SphereCollider>();
                break;
        }
    }

    public float ConsumeTrash()
    {
        var consumedEnergy = Time.deltaTime * CONSUMPTION_SPEED;

        _energy -= consumedEnergy;
        SetTransparencyValue(_energy);

        return consumedEnergy;
    }

    // Call this method with an external input or value
    private void SetTransparencyValue(float energy)
    {
        if (energy > 0)
        {
            // Map the clamped value to the desired transparency range
            float lerpedAlpha = Mathf.Lerp(0, 1, MapTo01(energy, 0, Data.Energy));

            // Set the transparency directly
            Color currentColor = _meshRenderer.material.color;
            _meshRenderer.material.color = new Color(currentColor.r, currentColor.g, currentColor.b, lerpedAlpha); 
        }
    }

    private void Update()
    {
        if (_isSetUp && _energy <= 0)
        {
            Player.RaiseTrashConsumed(Data.Size);
            Destroy(gameObject);
        }
    }

    private float MapTo01(float value, float min1, float max1)
    {
        value = Mathf.Clamp(value, min1, max1);
        return (value - min1) / (max1 - min1);
    }

    #endregion
}
