using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static VoiceRecognizer;

public class PlayerControl : MonoBehaviour
{

    public float moveSpeed;
    public float JumpForce;
    private Rigidbody2D myRigidbody;
    public bool grounded;
    public LayerMask whatIsGround;
    public Transform groundCheck;
    public float groundCheckRadius;
    //private Collider2D myCollider;
    private Animator myAnimator;
    public GameManager theGameManager;

    AudioSource audioSource;



    // Use this for initialization
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();

        //myCollider = GetComponent<Collider2D>();

        myAnimator = GetComponent<Animator>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

    }

    // Update is called once per frame
    void Update()
    {

        //grounded = Physics2D.IsTouchingLayers(myCollider, whatIsGround); //check if a collider is touching another collider

        //if the physics circle is overlapping with what's inside parameters then grounded is true
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        myRigidbody.velocity = new Vector2(moveSpeed, myRigidbody.velocity.y);

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) //use this for touch command
        {
            audioSource.clip = Microphone.Start(null, false, 1, 16000);

            if (grounded)
            {
                myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, JumpForce);
            }
        }

        //if (Input.GetKeyUp(KeyCode.Space))
        //{
        //    //******************************************************************************
        //    // Required to recognize the command

        //    Microphone.End(null);
        //    int vcommand = VoiceCommand.getCommand(audioSource);



        //    //******************************************************************************
        //    //Using the command in the animation
        //    //******************************************************************************
        //    Debug.Log("Recognized command:");
        //    Debug.Log(vcommand);
        //    if (grounded && vcommand == 3)
        //        myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, JumpForce);

        //    if (grounded && vcommand == 2)
        //        moveSpeed = 4;

        //    if (grounded && vcommand == 1)
        //        moveSpeed = 0;
        //}

        myAnimator.SetFloat("Speed", myRigidbody.velocity.x);
        myAnimator.SetBool("Grounded", grounded);
    }

    void OnCollisionEnter2D(Collision2D other) //two collision objects touch each other
    {
        //if player is touching another object tagged as killbox
        if (other.gameObject.tag == "killbox") 
        {
            theGameManager.RestartGame();
        }
    }
}
