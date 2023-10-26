using System;
using UnityEngine;

public class StageHandler : MonoBehaviour
{
    #region Fields and Properties

    public static StageHandler Instance { get; private set; }

    public event Action<int> OnStageVoiceLine;

    public int CurrentVoiceIndex { get; private set; } = 0;

    #endregion

    #region Methods

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void RaiseStageVoiceLine()
    {
        var index = CurrentVoiceIndex;
        OnStageVoiceLine?.Invoke(index);
        CurrentVoiceIndex++;
    }

    #endregion
}
