using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//***************************************************************************
//Library required for voice recognition
//***************************************************************************
using VoiceRecognizer;

public class PlayerController : MonoBehaviour
{
    //Attributes for the animation
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

    //***************************************************************************
    //Attribute required for voice recognition
    //***************************************************************************
    AudioSource audioSource;

    // Use this for initialization
    void Start()
    {
        //This is for the animation
        myRigidbody = GetComponent<Rigidbody2D>();
        //myCollider = GetComponent<Collider2D>();
        myAnimator = GetComponent<Animator>();

        //***************************************************************************
        //Initialization required for voice recognition
        //***************************************************************************
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        //Check if it is connected at least one microphone
        if (Microphone.devices.Length > 0)
        {
            Debug.Log(string.Concat("Microphones connected:", Microphone.devices.Length.ToString()));

            //Settings for the microphone for endless capture
            audioSource.clip = Microphone.Start(null, true, 1, 16000);

            //Load the trained weights
            VoiceCommand.loadWeights();
        }
        else
            Debug.Log("There are not microphones connected..");
    }


    // Update is called once per frame
    void Update()
    {

    //grounded = Physics2D.IsTouchingLayers(myCollider, whatIsGround); //check if a collider is touching another collider
    //if the physics circle is overlapping with what's inside parameters then grounded is true
    grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        myRigidbody.velocity = new Vector2(moveSpeed, myRigidbody.velocity.y);

        if (grounded && Input.GetKeyDown(KeyCode.Space)) //use this for touch command
        {
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, JumpForce);
        }

       


        //******************************************************************************
        // Recognizing the command
        //******************************************************************************
        int command = VoiceCommand.getCommand(audioSource);

        if (command > 0)
        {
            //******************************************************************************
            //Using the command in the animation
            //******************************************************************************
            Debug.Log("Recognized command:");
            Debug.Log(command);

            if (grounded && command == 3)
            {
                //Vector2 force = Vector2.right * JumpForce;
                //myRigidbody.AddForce(force, ForceMode2D.Impulse);
                myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, JumpForce);
                Debug.Log("Up");
            }

            if (grounded && command == 2)
            {
                moveSpeed = moveSpeed + 2;
                Debug.Log("Go");
            }

            if (grounded && command == 1 && moveSpeed > 0)
            {
                moveSpeed = moveSpeed - 2;
                Debug.Log("Down");
            }
        }

        myAnimator.SetFloat("Speed", myRigidbody.velocity.x);
        myAnimator.SetBool("Grounded", grounded);
    }

    public void ToggleSpeedUp()
    {
        if(grounded)
        {
            moveSpeed = moveSpeed + 2;
        }
    }

    public void ToggleSpeedDown()
    {
        if (grounded && moveSpeed > 0)
        {
            moveSpeed = moveSpeed - 2;
        }
    }
    void OnCollisionEnter2D(Collision2D other) //two collision objects touch each other
    {

        //if player is touching another object tagged as killbox
        if (other.gameObject.tag == "killbox")
        {
            moveSpeed = 0;
            theGameManager.RestartGame();
        }

        else if (other.gameObject.tag == "endlevel")
        {
            theGameManager.EndGame();
        }

        //if player is touching the Moving Platform it will stick with the platform.
        if (other.transform.tag == "MovingPlatform")
        {
            transform.parent = other.transform;
        }
    }
    void OnCollisionExit2D(Collision2D other) //exit collision for the moving platform
    {
               
        if (other.transform.tag == "MovingPlatform")
        {
            transform.parent = null;// the start point would not follow the character

        }
    }

}
