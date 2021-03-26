using EndlessWaveTD;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIButtonManager : MonoBehaviour
{

    private void Awake()
    {
        MainReferences.uIButtonManager = this;
    }

    public void RestartButtonPressed()
    {
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Times2GameSpeed()
    {
        Time.timeScale = 2;
    }

    public void Times3GameSpeed()
    {
        Time.timeScale = 3;
    }

    public void Times4GameSpeed()
    {
        Time.timeScale = 4;
    }

    public void NormalGameSpeed()
    {
        Time.timeScale = 1;
    }

    public void QuarterGameSpeed()
    {
        Time.timeScale = 0.25f;
    }

    //public void ToggleUpgradesPanel()
    //{
    //    EventManager.TriggerEvent(EventConstants.ToggleUpgradePanel);
    //}

    //public void ToggleOptionsPanel()
    //{
    //    EventManager.TriggerEvent(EventConstants.ToggleOptionsPanel);
    //}

    public void SetResolution900x600()
    {
        Screen.SetResolution(900, 600, false);
    }

    public void SetResolution1026x684()
    {
        Screen.SetResolution(1026, 684, false);
    }

    public void SetResolution1152x768()
    {
        Screen.SetResolution(1152, 768, false);
    }

    public void SetResolution1280x854()
    {
        Screen.SetResolution(1280, 854, false);
    }

    public void SetResolution1440x960()
    {
        Screen.SetResolution(1440, 960, false);
    }

    public void SetResolution1620x1080()
    {
        Screen.SetResolution(1620, 1080, false);
    }

    public void SetResolution1800x1200()
    {
        Screen.SetResolution(1800, 1200, false);
    }
}
