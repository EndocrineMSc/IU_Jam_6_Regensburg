using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class TextBlinker : MonoBehaviour
{
    private TextMeshProUGUI _text;
    private bool _isBlinking;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
        _text.enabled = false;
        FadeOut();
    }

    private void Update()
    {
        if (_text.enabled && !_isBlinking)
        {
            _isBlinking = true;
            FadeIn();
        }
    }

    private void FadeIn()
    {
        _text.DOFade(1, 1f).OnComplete(FadeOut);
    }

    private void FadeOut()
    {
        _text.DOFade(0, 1f).OnComplete(CheckIfStillEnabled);
    }
    
    private void CheckIfStillEnabled()
    {
        if (!_text.enabled)
        {
            _text.DOKill();
            _isBlinking = false;
        }
        else
        {
            FadeIn();
        }
    }
}
