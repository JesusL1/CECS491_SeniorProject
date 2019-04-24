using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Transform platformGenerator;
    private Vector3 platformStartPoint;

    public PlayerController thePlayer;
    private Vector3 playerStartPoint;

    private PlatformDestroyer[] platformList;

    public DeathMenu theDeathScreen;
    public EndLevelMenu endlevel;

    // Start is called before the first frame update
    void Start()
    {
        //platformStartPoint = platformGenerator.position;
        playerStartPoint = thePlayer.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RestartGame()
    {
        thePlayer.gameObject.SetActive(false);

        theDeathScreen.gameObject.SetActive(true);
        /*Couroutine runs independently from script and is useful to 
        add time delays so that the player doesn't go back to the beginning point right away */
       // StartCoroutine("RestartGameCo");
    }

    public void EndGame()
    {
        thePlayer.gameObject.SetActive(false);
        endlevel.gameObject.SetActive(true);
    }

    public void Reset()
    {
        thePlayer.moveSpeed = 0;
        theDeathScreen.gameObject.SetActive(false);
        //platformList = FindObjectsOfType<PlatformDestroyer>();
        //for (int i = 0; i < platformList.Length; i++)
        //{
        //    platformList[i].gameObject.SetActive(false);
        //}

        thePlayer.transform.position = playerStartPoint;
        //platformGenerator.position = platformStartPoint;
        thePlayer.gameObject.SetActive(true); //player is set back to start
    }

    //public IEnumerator RestartGameCo()
    //{
    //    thePlayer.gameObject.SetActive(false); //playerObject becomes inactive and no longer visible
    //    yield return new WaitForSeconds(0.5f); // half a second delay upon death 
    //    platformList = FindObjectsOfType<PlatformDestroyer>();
    //    for (int i = 0; i < platformList.Length; i++)
    //    {
    //        platformList[i].gameObject.SetActive(false);
    //    }

    //    thePlayer.transform.position = playerStartPoint;
    //    platformGenerator.position = platformStartPoint;
    //    thePlayer.gameObject.SetActive(true); //player is set back to start
    //}
}
