using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [SerializeField] private Image musicOnOff;
    [SerializeField] private Image soundOnOff;
    [SerializeField] private Image hardOnOff;
    [SerializeField] private TMP_Text diffText;

    [SerializeField] private Button musicButtn;
    [SerializeField] private Button soundButtn;
    [SerializeField] private Button diffButtn;

    private void Awake()
    {
        musicButtn.onClick.AddListener(() => Music());
        soundButtn.onClick.AddListener(() => Sound());
        diffButtn.onClick.AddListener(() => Diff());
    }

    void OnEnable()
    {
        int sound = PlayerPrefs.GetInt(StaticPrefStrings.SOUND, 1);
        if (sound == 1)
            soundOnOff.sprite = SpriteManager.Instance.on;
        else
            soundOnOff.sprite = SpriteManager.Instance.off;

        int music = PlayerPrefs.GetInt(StaticPrefStrings.MUSIC, 1);
        if (music == 1)
            musicOnOff.sprite = SpriteManager.Instance.on;
        else
            musicOnOff.sprite = SpriteManager.Instance.off;

        int diff = PlayerPrefs.GetInt(StaticPrefStrings.DIFFICULTY, 0);
        if (diff == 1)
        {
            diffText.text = "Difficulty: Hard";
            hardOnOff.sprite = SpriteManager.Instance.on;
        }
        else
        {
            diffText.text = "Difficulty: Easy";
            hardOnOff.sprite = SpriteManager.Instance.off;
        }
    }

    private void Diff()
    {
        if (PlayerPrefs.GetInt(StaticPrefStrings.SOUND, 1) == 1)
            AudioManager.Instance.PlaySound(1);

        int diff = PlayerPrefs.GetInt(StaticPrefStrings.DIFFICULTY, 0);
        if (diff == 1)
        {
            diffText.text = "Difficulty: Easy";
            hardOnOff.sprite = SpriteManager.Instance.off;
            PlayerPrefs.SetInt(StaticPrefStrings.DIFFICULTY, 0);

        }
        else
        {
            diffText.text = "Difficulty: Hard";
            hardOnOff.sprite = SpriteManager.Instance.on;
            PlayerPrefs.SetInt(StaticPrefStrings.DIFFICULTY, 1);
        }
    }

    private void Sound()
    {
        int sound = PlayerPrefs.GetInt(StaticPrefStrings.SOUND, 1);
        if (sound == 0)
        {
            PlayerPrefs.SetInt(StaticPrefStrings.SOUND, 1);
            soundOnOff.sprite = SpriteManager.Instance.on;

            if(PlayerPrefs.GetInt(StaticPrefStrings.SOUND, 1) == 1)
                AudioManager.Instance.PlaySound(1);

        }
        else
        {
            PlayerPrefs.SetInt(StaticPrefStrings.SOUND, 0);
            soundOnOff.sprite = SpriteManager.Instance.off;
        }
    }

    private void Music()
    {
        if (PlayerPrefs.GetInt(StaticPrefStrings.SOUND, 1) == 1)
            AudioManager.Instance.PlaySound(1);

        int music = PlayerPrefs.GetInt(StaticPrefStrings.MUSIC, 1);
        if (music == 0)
        {
            PlayerPrefs.SetInt(StaticPrefStrings.MUSIC, 1);
            musicOnOff.sprite = SpriteManager.Instance.on;
            AudioManager.Instance.PlaySound(0);
        }
        else
        {
            PlayerPrefs.SetInt(StaticPrefStrings.MUSIC, 0);
            musicOnOff.sprite = SpriteManager.Instance.off;
            AudioManager.Instance.StopSound(0);
        }
    }

    // Start is called before the first frame update

}
