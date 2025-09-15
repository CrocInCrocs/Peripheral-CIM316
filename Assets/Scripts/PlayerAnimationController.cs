using System;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private bool _gameHasStarted;
    private float _playerStartTimer;
    public float _timeUntilFadeIn;
    private bool _hasFadeStarted;
    public FadeController _fadeController;
    public void Start()
    {
        _gameHasStarted = true;
    }

    public void StartPlayerAnimation()
    {
        _fadeController.StartFadeOut();
    }
    public void Update()
    {
        if (_gameHasStarted)
        {
            _playerStartTimer += Time.deltaTime;
            if (_playerStartTimer >= _timeUntilFadeIn && _hasFadeStarted == false)
            {
                StartPlayerAnimation();
                _hasFadeStarted = true;
            }
        }
    }
}
