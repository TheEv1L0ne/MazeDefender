using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _playerHp;
    [SerializeField] private Image _playerHPBackground; 


    [SerializeField] private TMP_Text _baseHp;
    [SerializeField] private Image _baseHPBackground;

    [SerializeField] private GameObject _gameEnd;
    [SerializeField] private GameObject _vicotry;
    [SerializeField] private GameObject _defeat;

    [SerializeField] private TMP_Text _enemiesLeftText;

    public void SetPlayerHp(float currentHp, float maxHp)
    {
        _playerHPBackground.fillAmount = currentHp / maxHp;
        _playerHp.text = $"Player HP {currentHp}/{maxHp}";
    }

    public void SetBaseHp(float currentHp, float maxHp)
    {
        _baseHPBackground.fillAmount = currentHp / maxHp;
        _baseHp.text = $"Bese HP {currentHp}/{maxHp}";
    }

    public void SetGameEndState(bool state, bool won = false)
    {
        _gameEnd.SetActive(state);
        _vicotry.SetActive(won);
        _defeat.SetActive(!won);
    }

    public void SetEnemiesLeft(int enemiesLeftText)
    {
        _enemiesLeftText.text = $"Enemies left: {enemiesLeftText}";
    }

}
