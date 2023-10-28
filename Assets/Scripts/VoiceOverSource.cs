using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceOverSource : MonoBehaviour
{
    #region Fields and Properties

    public static VoiceOverSource Instance { get; private set; }

    [SerializeField] private Transform _muellerTransform;
    [SerializeField] private Transform _profTransform;
    [SerializeField] private EventReference _goodEndingVoice;
    [SerializeField] private EventReference _badEndingVoice;

    [field: SerializeField] public List<EventReference> VoiceLines { get; private set; } = new();

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
        Player.OnGoodEndingTrigger += PlayGoodEndingLine;
        Player.OnBadEndingTrigger += PlayBadEndingLine;
    }

    private void Start()
    {
        StageHandler.Instance.OnStageVoiceLine += PlayVoiceLine;
    }

    private void OnDisable()
    {
        StageHandler.Instance.OnStageVoiceLine -= PlayVoiceLine;
        Player.OnBadEndingTrigger -= PlayBadEndingLine;
        Player.OnGoodEndingTrigger  -= PlayGoodEndingLine;
    }

    private void PlayVoiceLine(int index)
    {
        if (VoiceLines.Count >= index)
        {
            var voiceOrigin = index % 2 == 0 ? _profTransform.position : _muellerTransform.position;
            AudioManager.Instance.PlayOneShot(VoiceLines[index], voiceOrigin);

            if (index % 2 == 0 && VoiceLines.Count > index)
                StartCoroutine(TriggerNextVoiceLine(6f));
            else
                TrashHandler.RaiseSpawnNewTrash();               
        }
    }

    private IEnumerator TriggerNextVoiceLine(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        StageHandler.Instance.RaiseStageVoiceLine();
    }

    private void PlayGoodEndingLine()
    {
        AudioManager.Instance.PlayOneShot(_goodEndingVoice, _muellerTransform.position);
    }

    private void PlayBadEndingLine()
    {
        AudioManager.Instance.PlayOneShot(_badEndingVoice, Camera.main.transform.position);
    }

    #endregion
}
