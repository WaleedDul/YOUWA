using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerControls : MonoBehaviour {

    Rigidbody2D myRigidBody;
    BoxCollider2D myCollider;

    float rotationGoal;
    float defaultGravity;
    float totalDistance = 0f;
    bool didICollide;
    public float damageDone = 25f;

    [SerializeField] float health = 100;
    [SerializeField] float gravityScale = 1f;

    public Slider healthSlider;

    // Use this for initialization
    void Start() {
        myRigidBody = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<BoxCollider2D>();
        defaultGravity = -1 * Physics2D.gravity.y;
        didICollide = myCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
    }

    // Update is called once per frame
    void Update() {
        GravityControl();
        healthSlider.value = health;
        if (health <= 0) {
            Debug.Log("YOU DIE");
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {

        Enemy enemy = collision.gameObject.GetComponent<Enemy>();

        if (enemy) {
            // TODO: Only take damage if you are moving towards the target
            health -= enemy.damageDone;
        }
    }

    private void GravityControl() {
        bool isCollidingWithGround = myCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
        bool direcitonChanged = false;
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            myRigidBody.gravityScale = gravityScale;
            Physics2D.gravity = new Vector2(defaultGravity, 0f);
            rotationGoal = 90;
            direcitonChanged = true;
        } else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            myRigidBody.gravityScale = gravityScale;
            Physics2D.gravity = new Vector2(-defaultGravity, 0f);
            rotationGoal = 270;
            direcitonChanged = true;
        } else if (Input.GetKeyDown(KeyCode.UpArrow)) {
            myRigidBody.gravityScale = gravityScale;
            Physics2D.gravity = new Vector2(0f, defaultGravity);
            rotationGoal = 180;
            direcitonChanged = true;
        } else if (Input.GetKeyDown(KeyCode.DownArrow)) {
            myRigidBody.gravityScale = gravityScale;
            Physics2D.gravity = new Vector2(0f, -defaultGravity);
            rotationGoal = 0;
            direcitonChanged = true;
        }

        // Raycast in the direction of gravity rather than velocity to avoid
        //  the need to wait for velocity to settle in the correct direction
        RaycastHit2D closestObsticle = Physics2D.Raycast(transform.position,
            Physics2D.gravity.normalized,
            10000f,
            LayerMask.GetMask("Ground"));

        Debug.DrawRay(transform.position, Physics2D.gravity.normalized);

        float distanceToImpact = closestObsticle.distance;

        if (direcitonChanged) {
            totalDistance = distanceToImpact;
        }

        // TODO: Rotating when you are in a corner
        // Somehow ensure that an object completely rotates before stopping
        float rotationFactor = isCollidingWithGround != didICollide && isCollidingWithGround ? 1 : 1 - (distanceToImpact / totalDistance);


        transform.rotation = Quaternion.Slerp(transform.rotation,
            Quaternion.Euler(0f, 0f, rotationGoal),
            rotationFactor);


        //if (isCollidingWithGround != didICollide && isCollidingWithGround && transform.rotation.z != rotationGoal)
        //{
        //    transform.position += transform.up.normalized * 0.1f;
        //}

        didICollide = isCollidingWithGround;


    }

}
