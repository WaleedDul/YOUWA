using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{

    // Config
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] Vector2 deathKick = new Vector2(0f, 5f);

    // State
    bool isAlive = true;

    // Cached Component References
    Rigidbody2D myRigidBody;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;
    float gravityScaleAtStart;

    // Message then Methods
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        gravityScaleAtStart = myRigidBody.gravityScale;
    }

    void Update()
    {
        if (!isAlive) { return; }
        Run();
        ClimbLadder();
        Jump();
        FlipSprite();
        Die();
    }

    private void Run()
    {

        float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal"); // -1 to +1
        Vector2 playerVelocity = new Vector2(controlThrow * runSpeed, myRigidBody.velocity.y);
        myRigidBody.velocity = playerVelocity;

        myAnimator.SetBool("Running", true);

        bool isPlayerRunning = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("Running", isPlayerRunning);
    }

    private void ClimbLadder()
    {

        bool isPlayerOnLadder = myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing"));

        if (!isPlayerOnLadder)
        {
            myAnimator.SetBool("Climbing", false);
            myRigidBody.gravityScale = gravityScaleAtStart;
            return;
        }
        else
        {
            float controlThrow = CrossPlatformInputManager.GetAxis("Vertical"); // -1 to +1
            Vector2 climbVelocity = new Vector2(myRigidBody.velocity.x, controlThrow * climbSpeed);
            myRigidBody.velocity = climbVelocity;

            bool playerHasVerticalSpeed = Mathf.Abs(myRigidBody.velocity.y) > Mathf.Epsilon;
            myAnimator.SetBool("Climbing", playerHasVerticalSpeed);

            myRigidBody.gravityScale = 0f;
        }
    }

    private void Jump()
    {

        bool isPlayerOnTheGround = myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));

        if (!isPlayerOnTheGround)
        {
            return;
        }
        if (CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            Vector2 jumpVelocityToAdd = new Vector2(0f, jumpSpeed);
            myRigidBody.velocity += jumpVelocityToAdd;
        }
    }


    private void FlipSprite()
    {

        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidBody.velocity.x), 1f);
        }
    }

    private void Die()
    {
        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards")) || myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Hazards")))
        {
            myAnimator.SetTrigger("Die");
            myRigidBody.velocity = deathKick;
            isAlive = false;
        }
    }

}
