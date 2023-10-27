using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class StageHandler : MonoBehaviour
{
    #region Fields and Properties

    public static StageHandler Instance { get; private set; }

    public event Action<int> OnStageVoiceLine;

    public int CurrentVoiceIndex { get; private set; } = 0;

    [SerializeField] private Image _goodEndingImage;
    [SerializeField] private Image _badEndingImage;

    #endregion

    #region Methods

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void OnEnable()
    {
        Player.OnBadEndingTrigger += BadEnding;
        Player.OnGoodEndingTrigger += GoodEnding;
    }

    private void OnDisable()
    {
        Player.OnBadEndingTrigger -= BadEnding;
        Player.OnGoodEndingTrigger -= GoodEnding;
    }

    public void RaiseStageVoiceLine()
    {
        var index = CurrentVoiceIndex;
        OnStageVoiceLine?.Invoke(index);
        CurrentVoiceIndex++;
    }

    private void BadEnding()
    {
        _badEndingImage.DOFade(1, 2f);
    }

    private void GoodEnding()
    {
        _goodEndingImage.DOFade(1, 2f);
    }
    #endregion
}
