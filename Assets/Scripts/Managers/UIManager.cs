using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    #region Buttons
    [Header ("BUTTONS")]
    [Header("main menu")]
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _exitButton;
    [SerializeField] private Button _settingsButton;

    [Header("Settings")]
    [SerializeField] private Button _backSettings;

    [Header("gameplay")]
    [SerializeField] private Button _quitButton;
    #endregion

    #region Screens
    [Header("SCREENS")]
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _gameplay;
    [SerializeField] private GameObject _settings;
    #endregion

    [Header("Script references")]
    public GamePlayUI playUI;

    public delegate void OnStartPressed();
    public static OnStartPressed onStartPressedDelegate;

    public delegate void OnQuitPressed();
    public static OnQuitPressed onQuitPressedDelegate;

    private void Awake()
    {
        _startButton.onClick.AddListener(() => StartGame());
        _exitButton.onClick.AddListener(() => ExitGame());
        _settingsButton.onClick.AddListener(() => OpenSettings());

        _quitButton.onClick.AddListener(() => QuitGame());

        _backSettings.onClick.AddListener(() => OpenMainMenu());
    }

    private void OnEnable()
    {
        _mainMenu.SetActive(true);
        _settings.SetActive(false);
        _gameplay.SetActive(false);
    }

    private void OpenMainMenu()
    {
        _mainMenu.SetActive(true);
        _settings.SetActive(false);
    }

    private void QuitGame()
    {
        _mainMenu.SetActive(true);
        _gameplay.SetActive(false);

        playUI.SetGameEndState(false);

        onQuitPressedDelegate?.Invoke();
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
        playUI.SetGameEndState(false);

        onStartPressedDelegate?.Invoke();
    }
}
