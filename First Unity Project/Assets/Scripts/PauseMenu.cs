﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public string mainMenuLevel;

    public void RestartGame()
    {
        
        Application.LoadLevel(Application.loadedLevel);
        Resume();
        //FindObjectOfType<GameManager>().Reset();
    }

    public void QuitToMain()
    {
        Resume();
        Application.LoadLevel(mainMenuLevel);
       //FindObjectOfType<GameManager>().Reset();
    }


    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        if (GameIsPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }
    
}
