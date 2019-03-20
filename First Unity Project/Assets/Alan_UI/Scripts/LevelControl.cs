using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class levelScript : MonoBehaviour
{
    public static levelScript instance = null;
    GameObject levelSign, gameOverText, youWinText;
    int sceneIndex, levelPassed;

    void Start()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        levelSign = GameObject.Find("LevelNumber");
        gameOverText = GameObject.Find("GameOverText");
        youWinText = GameObject.Find("YouWinText");
        gameOverText.gameObject.SetActive(false);
        youWinText.gameObject.SetActive(false);

        sceneIndex = SceneManager.GetActiveScene().buildIndex;
        levelPassed = PlayerPrefs.GetInt("LevelPassed");
    }

    public void youWin()
    {
        if (sceneIndex == 3)
            Invoke("loadCredits", 1f);
        else
        {
            if (levelPassed < sceneIndex)
                PlayerPrefs.SetInt("LevelPassed", sceneIndex);
            levelSign.gameObject.SetActive(false);
            youWinText.gameObject.SetActive(true);
            Invoke("loadNextLevel", 1f);
        }
    }

    public void youLose()
    {
        levelSign.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(true);
        Invoke("loadMainMenu", 1f);
    }

    void loadNextLevel()
    {
        SceneManager.LoadScene(sceneIndex + 1);
    }

    void loadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    void loadCredits()
    {
        SceneManager.LoadScene("Credits");
    }
}
