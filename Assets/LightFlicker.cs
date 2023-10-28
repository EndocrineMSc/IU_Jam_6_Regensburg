using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using DG.Tweening;

public class LightFlicker : MonoBehaviour
{
    private Light _light;
    [SerializeField] private float _minIntensity;
    [SerializeField] private float _maxIntensity;

    private float _flickerDuration = 0;
    private bool _isMinIntensity;

    private void Awake()
    {
        _light = GetComponent<Light>();
    }

    private void OnEnable()
    {
        Player.OnEnergyTransferred += SetTimer;
    }

    private void OnDisable()
    {
        Player.OnEnergyTransferred -= SetTimer;
    }

    private void FlickerMax()
    {
        if (_flickerDuration > 0)
        {
            _light.DOIntensity(_maxIntensity, 0.1f).SetEase(Ease.InBounce);
            StartCoroutine(WaitTillFlickerChange());
        }
    }

    private void FlickerMin()
    {
        _light.DOIntensity(_minIntensity, 0.1f).SetEase(Ease.InBounce);
        StartCoroutine(WaitTillFlickerChange());
    }

    private void SetTimer(float duration)
    {
        _flickerDuration = duration / 100;
        FlickerMax();
    }

    private IEnumerator WaitTillFlickerChange()
    {
        var timer = Random.Range(0.1f, 1f);
        yield return new WaitForSeconds(timer);
        if (_isMinIntensity)
        {
            _isMinIntensity = false;
            FlickerMax();
        }
        else
        {
            _isMinIntensity = true;
            FlickerMin();
        }
    }

    private void Update()
    {
        if (_flickerDuration > 0)
            _flickerDuration -= Time.deltaTime;
    }
}
