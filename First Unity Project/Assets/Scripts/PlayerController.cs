using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
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

    // Use this for initialization
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();

        //myCollider = GetComponent<Collider2D>();

        myAnimator = GetComponent<Animator>();

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
            if (grounded)
            {
                myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, JumpForce);
            }
        }

        myAnimator.SetFloat("Speed", myRigidbody.velocity.x);
        myAnimator.SetBool("Grounded", grounded);
    }

    void OnCollisionEnter2D(Collision2D other) //two collision objects touch each other
    {
        //if player is touching another object tagged as killbox
        if (other.gameObject.tag == "killbox") 
        {
            myAnimator.SetBool("IsDead", true);
            theGameManager.RestartGame();
        }

        if (other.gameObject.tag == "endlevel")
        {
            theGameManager.EndGame();
        }
    }
}
