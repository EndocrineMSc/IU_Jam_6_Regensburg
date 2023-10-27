using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Loads the next Scene after a delay, referencing the LoadHelper class
/// </summary>
public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _loadingText;

    const string _loading1 = "Laden.";
    const string _loading2 = "Laden..";
    const string _loading3 = "Laden...";

    private readonly float _textTimerMax = 1f;
    private float _textTimer;
    
    private void Start()
    {
        StartCoroutine(WaitForLoadingScreen());
        _textTimer = _textTimerMax;
        _loadingText.text = _loading1;
    }

    private IEnumerator WaitForLoadingScreen()
    {
        yield return new WaitForSeconds(LoadHelper.LoadDuration);
        SceneManager.LoadScene(LoadHelper.SceneToBeLoaded.ToString());
    }

    private void Update()
    {
        _textTimer -= Time.deltaTime;

        if (_textTimer <= 0)
        {
            switch (_loadingText.text)
            {
                case _loading1:
                    _loadingText.text = _loading2;
                    break;
                case _loading2:
                    _loadingText.text = _loading3;
                    break;
                case _loading3:
                    _loadingText.text = _loading1;
                    break;
            }

            _textTimer = _textTimerMax;
        }
    }
}
