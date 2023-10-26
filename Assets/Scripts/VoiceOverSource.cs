using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceOverSource : MonoBehaviour
{
    #region Fields and Properties

    [SerializeField] private Transform _muellerTransform;
    [SerializeField] private Transform _profTransform;

    [SerializeField] private List<EventReference> _voiceLines = new();

    #endregion

    #region Methods

    private void Start()
    {
        StageHandler.Instance.OnStageVoiceLine += PlayVoiceLine;
    }

    private void OnDisable()
    {
        StageHandler.Instance.OnStageVoiceLine -= PlayVoiceLine;
    }

    private void PlayVoiceLine(int index)
    {
        Debug.Log("Played voiceline with index: " + index);
        if (_voiceLines.Count >= index)
        {
            var voiceOrigin = index % 2 == 0 ? _profTransform.position : _muellerTransform.position;
            AudioManager.Instance.PlayOneShot(_voiceLines[index], voiceOrigin);

            var description = FMODUnity.RuntimeManager.GetEventDescription(_voiceLines[index]);
            description.getLength(out int lengthInMilliseconds);
            var waitTime = lengthInMilliseconds / 1000;

            if (index % 2 == 0 && _voiceLines.Count > index)
                StartCoroutine(TriggerNextVoiceLine(waitTime));
            else
                TrashHandler.RaiseSpawnNewTrash();
                
        }
    }

    private IEnumerator TriggerNextVoiceLine(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        StageHandler.Instance.RaiseStageVoiceLine();
    }



    #endregion
}
