using System;
using UnityEngine;

public class FadeController : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    [SerializeField] private float _timer;
    public AnimationCurve fadeCurve;
    [SerializeField] private bool _isFading;
    [SerializeField] private bool _isFadingOut;
    public bool startFadeOn;
    public float _fadeSpeed;
    public bool firstFadeOut = true;

    //this is just for the one time sleeping cutscene to play once the fade in has completed.
    public GameObject cutscene;
    public GameObject cutscene2;

    //DONT USE UNLESS ITS FOR THE BED, I HAVE MADE THIS A BED ONLY FADING FOR NOW BECAUSE OF THE NATURE OF ITS USE.
    public void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (startFadeOn)
        {
            _timer = 1f * _fadeSpeed;
            _isFadingOut = true;
        }
        else
        {
            _timer = 0f;
        }
    }

    public void StartFadeIn()
    {
        _timer = 0f;
        _isFading = true;
        _isFadingOut = false;
    }

    public void StartFadeOut()
    {
        _timer = 1f * _fadeSpeed;
        _isFadingOut = true;
        _isFading = false;
    }
    public void Update()
    {
        if (firstFadeOut)
        {
            cutscene2.SetActive(true);
            firstFadeOut = false;
        }
        if (_isFadingOut)
        {
            _timer -= Time.deltaTime;
        }

        if (_isFading)
        {
            _timer += Time.deltaTime;
        }
        canvasGroup.alpha = fadeCurve.Evaluate(_timer/_fadeSpeed);
        if (_timer > 1 * _fadeSpeed)
        {
            _isFading = false;
            cutscene2.SetActive(false);
            cutscene.SetActive(true);
        }
        if (_timer < 0)
        {
            _isFadingOut = false;
            firstFadeOut = false;
        }
    }
}
