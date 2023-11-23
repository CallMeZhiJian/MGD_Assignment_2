using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject SettingScreen;
    public GameObject BGM_Checkmark;
    public GameObject SFX_Checkmark;

    private void Update()
    {
        if (AudioManager.instance._BGMSource.mute)
        {
            BGM_Checkmark.SetActive(false);
        }
        else
        {
            BGM_Checkmark.SetActive(true);
        }

        if (AudioManager.instance._SFXSource.mute)
        {
            SFX_Checkmark.SetActive(false);
        }
        else
        {
           SFX_Checkmark.SetActive(true);
        }
    }

    public void StartButton()
    {
        AudioManager.instance.PlaySFX("JiSFX");
        SceneManager.LoadScene("GameScene");
    }

    public void Quit()
    {
        AudioManager.instance.PlaySFX("JiSFX");
        Application.Quit();
    }

    public void OnSetting()
    {
        AudioManager.instance.PlaySFX("JiSFX");
        SettingScreen.SetActive(true);
    }

    public void Back()
    {
        AudioManager.instance.PlaySFX("JiSFX");
        SettingScreen.SetActive(false);
    }

    public void MuteUnmuteBGM()
    {
        if (AudioManager.instance._BGMSource.mute)
        {
            AudioManager.instance._BGMSource.mute = false;
        }
        else
        {
           AudioManager.instance._BGMSource.mute = true;
        }
    }

    public void MuteUnmuteSFX()
    {
        if (AudioManager.instance._SFXSource.mute)
        {
            AudioManager.instance._SFXSource.mute = false;
        }
        else
        {
            AudioManager.instance._SFXSource.mute = true;
        }
    }

    public void MainMenu()
    {
        AudioManager.instance.PlaySFX("JiSFX");
        SceneManager.LoadScene("MainMenu");
    }
}
