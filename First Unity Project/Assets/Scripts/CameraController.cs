using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    private Vector2 velocity;
    public float smoothTimeY;
    public float smoothTimeX;

    public PlayerController player;

    //public PlayerControl thePlayer;
    //private Vector3 lastPlayerPosition;
    //private float distanceToMove;

	// Use this for initialization
	void Start () {
        player = FindObjectOfType<PlayerController>();
        //thePlayer = FindObjectOfType<PlayerControl>();
        //lastPlayerPosition = thePlayer.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        float posX = Mathf.SmoothDamp(transform.position.x, player.transform.position.x, ref velocity.x, smoothTimeX);
        float posY = Mathf.SmoothDamp(transform.position.y, player.transform.position.y, ref velocity.y, smoothTimeY);
        transform.position = new Vector3(posX, posY, transform.position.z);
        //distanceToMove = thePlayer.transform.position.x - lastPlayerPosition.x ;
        //transform.position = new Vector3(transform.position.x + distanceToMove, transform.position.y, transform.position.x);
        //lastPlayerPosition = thePlayer.transform.position;
	}
}
