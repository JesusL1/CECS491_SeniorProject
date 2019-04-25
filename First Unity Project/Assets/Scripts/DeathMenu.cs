using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathMenu : MonoBehaviour
{
    public string mainMenuLevel;

    public void RestartGame()
    {
        Application.LoadLevel(Application.loadedLevel);
        FindObjectOfType<GameManager>().Reset();
    }

    public void QuitToMain()
    {
        Application.LoadLevel(mainMenuLevel);
    }
}
