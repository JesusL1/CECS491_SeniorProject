using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevelMenu : MonoBehaviour
{
    public string mainMenuLevel;

    // in the future, should probably go to stage selection screen
    public void QuitToMain()
    {
        Application.LoadLevel(mainMenuLevel);
    }
}
