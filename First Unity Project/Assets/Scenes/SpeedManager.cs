using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedManager : MonoBehaviour
{

    public PlayerController thePlayer;
    public Text speedText;

    public float speedCount;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        speedCount = thePlayer.moveSpeed;
        speedText.text = "Speed: " + speedCount;
        
    }
}
