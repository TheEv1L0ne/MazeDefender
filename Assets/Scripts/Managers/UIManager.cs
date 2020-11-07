using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    #region Buttons
    [Header ("BUTTONS")]
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _exitButton;
    [SerializeField] private Button _settingsButton;
    #endregion

    #region Screens
    [Header("SCREENS")]
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _gameplay;
    [SerializeField] private GameObject _settings;
    #endregion

    public delegate void OnStartPressed();
    public static OnStartPressed onStartPressedkDelegate;

    private void Awake()
    {
        _startButton.onClick.AddListener(() => StartGame());
        _exitButton.onClick.AddListener(() => ExitGame());
        _settingsButton.onClick.AddListener(() => OpenSettings());
    }

    private void OpenSettings()
    {
        _mainMenu.SetActive(false);
        _settings.SetActive(true);
    }

    private void ExitGame()
    {
        Application.Quit();
    }

    private void StartGame()
    {
        _mainMenu.SetActive(false);
        _gameplay.SetActive(true);
        onStartPressedkDelegate?.Invoke();
    }
}
