using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject credits;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (credits.activeSelf)
            {
                credits.SetActive(false);
            }

        }

    }
    public void PlayGame()
    {
       SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
    public void Credits()
    {
        if(credits.activeSelf)
        {
            credits.SetActive(false);
        }
        else
        {
            credits.SetActive(true);
        }
    }
}
