using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject menu;
    public GameObject settings;
    public bool isPaused = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (settings.activeSelf)
            {
                settings.SetActive(false);
            }
            else
            {
                if (isPaused)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
            }
        }
    }

    private void Pause()
    {
        menu.SetActive(true);
        Time.timeScale = 0;
        isPaused = true;
    }
    public void Resume()
    {
        menu.SetActive(false);
        Time.timeScale = 1;
        isPaused = false;
    }
    public void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #elif UNITY_WEBPLAYER
        Application.OpenURL(webplayerQuitURL);
        #else
        Application.Quit();
        #endif
    }
    public void Save()
    {
        
    }
    public void Load()
    {

    }
    public void Settings()
    {
        if(settings.activeSelf)
        {
            settings.SetActive(false);
        }
        else
        {
            settings.SetActive(true);
        }
    }

}
