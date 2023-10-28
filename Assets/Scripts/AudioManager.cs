using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    private EventInstance _menuMusicEventInstance;
    private EventInstance _levelMusicEventInstance;
    private EventInstance _levelAmbienceEventInstance;
    private EventInstance _radioIntro;

    [field: SerializeField] public EventReference MenuMusic { get; private set; }
    [field: SerializeField] public EventReference LevelAmbience { get; private set; }
    [field: SerializeField] public EventReference RadioIntro { get; private set; }

    public float MasterVolume { get; private set; } = 1f;
    public float MusicVolume { get; private set; } = 1f;
    public float SoundVolume { get; private set; } = 1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        RuntimeManager.GetBus("bus:/").setVolume(MasterVolume);
    }

    public void PlayOneShot(EventReference sound, Vector3 worldPosition)
    {
        RuntimeManager.PlayOneShot(sound, worldPosition);
    }

    public EventInstance CreateEventInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        return eventInstance;
    }

    public void PlayMenuMusic()
    {
        PLAYBACK_STATE playbackState;
        _menuMusicEventInstance.release();
        _menuMusicEventInstance = CreateEventInstance(MenuMusic);
        _menuMusicEventInstance.getPlaybackState(out playbackState);
        
        if (playbackState.Equals(PLAYBACK_STATE.STOPPED))
            _menuMusicEventInstance.start();
    }

    public void PlayLevelAmbience()
    {
        PLAYBACK_STATE playbackState;
        _levelAmbienceEventInstance.release();
        _levelAmbienceEventInstance = CreateEventInstance(LevelAmbience);
        _levelAmbienceEventInstance.getPlaybackState(out playbackState);

        if (playbackState.Equals(PLAYBACK_STATE.STOPPED))
            _levelAmbienceEventInstance.start();
    }

    public void PlayerIntroRadio()
    {
        Instance.PlayOneShot(RadioIntro, transform.position);
    }

    public void StopMenuMusic()
    {
        _menuMusicEventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    public void StopLevelMusic()
    {
        _levelMusicEventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    public void StopLevelAmbience()
    {
        _levelAmbienceEventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    public void SetMasterVolume(float volume)
    {
        if (volume >= 0 && volume <= 1)
            MasterVolume = volume;
    }

    public void SetMusicVolume(float volume)
    {
        if (volume >= 0 && volume <= 1)
            MusicVolume = volume;
    }

    public void SetSoundVolume(float volume)
    {
        if (volume >= 0 && volume <= 1)
            SoundVolume = volume;
    }
}
