using System;
using UnityEngine;
using DG.Tweening;
using TMPro;
using FMODUnity;
using FMOD.Studio;

/// <summary>
/// - The player will increase in size
/// - The player can only absorb stuff that is somewhat smaller than it
/// - The player needs to eat a certain amount before discharging energy
/// </summary>
public class Player : MonoBehaviour
{
    #region Fields and Properties

    private float _currentSize = 10;
    private readonly float _minimumEnergyCapacity = 100;
    public float CurrentEnergy { get; private set; }

    private bool _isTouchingTrash;
    private bool _isTouchingPad;
    private Trash _trashInContact;

    [SerializeField] private TextMeshProUGUI _choiceText;
    private float _decisionTimer = 15f;
    private bool _isDecisionTime;

    public static bool BadEndingTriggered;
    public static bool GoodEndingTriggered;

    [SerializeField] private ParticleSystem _particleSystem;

    private EventInstance _metabolizeInstance;
    [SerializeField] private EventReference _playerMetabolize;
    [SerializeField] private EventReference _paperCrumble;

    #endregion

    #region Events

    public static event Action<float> OnTrashConsumed;
    public static event Action OnPlayerChoiceEnding;
    public static event Action OnGoodEndingTrigger;
    public static event Action OnBadEndingTrigger;
    public static event Action<float> OnEnergyTransferred;

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
            AudioManager.Instance.PlayOneShot(_paperCrumble, transform.position);
            //_metabolizeInstance.start();
        }

        if (collision.gameObject.TryGetComponent(out EnergyPad _))
            _isTouchingPad = true;
    }

    private void StopMetabolizeSound()
    {
        _metabolizeInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Trash _))
        {
            _trashInContact = null;
            _isTouchingTrash = false;
            //StopMetabolizeSound();
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

        if (_isDecisionTime)
            _decisionTimer -= Time.deltaTime;

        if (_decisionTimer <= 0 && !BadEndingTriggered && !GoodEndingTriggered)
            RaiseGoodEnding();

        if (transform.position.y < -10 && !BadEndingTriggered)
            RaiseBadEnding();

        var emission = _particleSystem.emission;
        emission.rateOverTime = CurrentEnergy;
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
            RaiseEnergyTransferred(CurrentEnergy);
            CurrentEnergy = 0;

            if (StageHandler.Instance.CurrentVoiceIndex < VoiceOverSource.Instance.VoiceLines.Count)
            {
                StageHandler.Instance.RaiseStageVoiceLine();
            }
            else
            {
                DisplayChoice();
                OnPlayerChoiceEnding?.Invoke();
                _isDecisionTime = true;
            }
        }
    }

    private void DisplayChoice()
    {
        _choiceText.enabled = true;
    }

    private void ScaleSizeOnTrashConsumed(float size)
    {
        var newScale = transform.localScale *= (1 + (size / 200));
        transform.DOScale(newScale, 1f);
        _currentSize += size;
    }

    public static void RaiseTrashConsumed(float size)
    {
        OnTrashConsumed?.Invoke(size);
    }

    public static void RaiseGoodEnding()
    {
        OnGoodEndingTrigger?.Invoke();
        GoodEndingTriggered = true;
    }

    public static void RaiseBadEnding()
    {
        OnBadEndingTrigger?.Invoke();
        BadEndingTriggered = true;
    }

    public static void RaiseEnergyTransferred(float amount)
    {
        OnEnergyTransferred?.Invoke(amount);
    }

    #endregion
}
