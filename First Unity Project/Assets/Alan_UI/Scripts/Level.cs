using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    public Button level2Button, level3Button;
    int levelPassed;

    void Start()
    {
        levelPassed = PlayerPrefs.GetInt("LevelPassed");
        level2Button.interactable = true;
        level3Button.interactable = false;

        switch (levelPassed)
        {
            case 1:
                level2Button.interactable = true;
                break;
            case 2:
                level2Button.interactable = true;
                level3Button.interactable = true;
                break;
        }
    }

    public void levelToLoad (int level)
    {
        SceneManager.LoadScene(level);
    }

    public void resetPlayerPrefs()
    {
        level2Button.interactable = false;
        level3Button.interactable = false;
        PlayerPrefs.DeleteAll();
    }
}
