using System;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// - The player will increase in size
/// - The player can only absorb stuff that is somewhat smaller than it
/// - The player needs to eat a certain amount before discharging energy
/// </summary>
public class Player : MonoBehaviour
{
    #region Fields and Properties

    private float _currentSize = 1000;
    private readonly float _minimumEnergyCapacity = 100;
    public float CurrentEnergy { get; private set; }

    private bool _isTouchingTrash;
    private bool _isTouchingPad;
    private Trash _trashInContact;

    #endregion

    #region Events

    public static event Action<float> OnTrashConsumed;

    #endregion

    #region Methods

    private void OnEnable()
    {
        OnTrashConsumed += ScaleSizeOnTrashConsumed;
    }

    private void OnDisable()
    {
        OnTrashConsumed -= ScaleSizeOnTrashConsumed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Trash trash))
        {
            _trashInContact = trash;
            _isTouchingTrash = true;
        }

        if (collision.gameObject.TryGetComponent(out EnergyPad _))
            _isTouchingPad = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Trash _))
        {
            _trashInContact = null;
            _isTouchingTrash = false;
        }

        if (collision.gameObject.TryGetComponent(out EnergyPad _))
            _isTouchingPad = false;
    }

    private void Update()
    {
        if (_isTouchingTrash && _trashInContact != null && _currentSize >= _trashInContact.Data.Size)
            CurrentEnergy += _trashInContact.ConsumeTrash();

        if (CurrentEnergy >= _minimumEnergyCapacity)
            ShowEnoughEnergy();

        if (_isTouchingPad && CurrentEnergy >= _minimumEnergyCapacity)
            LoseStoredEnergy();
    }

    private void ShowEnoughEnergy()
    {
        //do some player feedback
    }

    private void LoseStoredEnergy()
    {

        var trashInScene = FindObjectsOfType<Trash>();

        if (trashInScene.Length <= 0)
        {
            CurrentEnergy = 0;
            StageHandler.Instance.RaiseStageVoiceLine();
        }
    }

    public static void RaiseTrashConsumed(float size)
    {
        OnTrashConsumed?.Invoke(size);
    }

    private void ScaleSizeOnTrashConsumed(float size)
    {
        var newScale = transform.localScale *= (1 + (size / 100));
        transform.DOScale(newScale, 1f);
        _currentSize += size;
    }

    #endregion
}
