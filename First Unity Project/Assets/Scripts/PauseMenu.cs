using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public string mainMenuLevel;

    public void RestartGame()
    {
        FindObjectOfType<GameManager>().Reset();
    }

    public void QuitToMain()
    {
        Application.LoadLevel(mainMenuLevel);
    }
}
