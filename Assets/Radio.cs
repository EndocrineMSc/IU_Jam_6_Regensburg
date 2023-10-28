using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radio : MonoBehaviour
{
    [SerializeField] private EventReference _radioIntro;
    [SerializeField] private EventReference _levelAmbiente;

    private void OnEnable()
    {
        Player.OnBadEndingTrigger += DestroyThis;
        Player.OnGoodEndingTrigger += DestroyThis;
    }

    private void OnDisable()
    {
        Player.OnBadEndingTrigger -= DestroyThis;
        Player.OnGoodEndingTrigger -= DestroyThis;
    }

    private void Start()
    {
        AudioManager.Instance.PlayOneShot(_radioIntro, transform.position);
        AudioManager.Instance.PlayOneShot(_levelAmbiente, transform.position);
    }

    private void DestroyThis()
    {
        Destroy(gameObject);
    }
}
