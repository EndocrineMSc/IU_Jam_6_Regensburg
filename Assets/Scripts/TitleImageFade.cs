using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleImageFade : MonoBehaviour
{
    private Image _image;
    // Start is called before the first frame update
    void Start()
    {
        _image = GetComponent<Image>();
        StartCoroutine(WaitTillFade());
    }

    private void Disable()
    {
        _image.raycastTarget = false;
    }

    private IEnumerator WaitTillFade()
    {
        yield return new WaitForSeconds(1);
        _image.DOFade(0f, 3f).SetEase(Ease.InCubic).OnComplete(Disable);
    }
}
