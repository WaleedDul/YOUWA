using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControls : MonoBehaviour {

    Rigidbody2D myRigidBody;
    BoxCollider2D myCollider;

    public float rotationGoal;
    float totalDistance = 0f;
    bool didICollide;
    public float damageDone = 25f;
    public bool directionChanged = false;
    public Vector2 gravity;
    GameObject game;

    [SerializeField] float health = 100;

    //public Slider healthSlider;

    // Use this for initialization
    void Start() {
        myRigidBody = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<BoxCollider2D>();
        didICollide = myCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));

        game = GameObject.Find("GameManager");
        game.GetComponent<GravityManager>().AddEntity(this);
    }

    // Update is called once per frame
    void Update() {
        SpriteRotation();
        UpdateHealth();
    }

    void OnCollisionEnter2D(Collision2D collision) {

        Enemy enemy = collision.gameObject.GetComponent<Enemy>();

        if (enemy) {
            // TODO: Only take damage if you are moving towards the target
            health -= enemy.damageDone;
        }
    }

    private void UpdateHealth() {
        //if (healthSlider != null) {
            //healthSlider.value = health;
        //}
        if (health <= 0) {
            Debug.Log("YOU DIE");
            Destroy(gameObject);
        }
    }

    private void SpriteRotation() {
        directionChanged = false; // TODO: Update from GravityManager
        bool isCollidingWithGround = myCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));

        // Raycast in the direction of gravity rather than velocity to avoid
        //  the need to wait for velocity to settle in the correct direction
        RaycastHit2D closestObsticle = Physics2D.Raycast(transform.position,
            gravity.normalized,
            10000f,
            LayerMask.GetMask("Ground"));

        Debug.DrawRay(transform.position, gravity.normalized);

        float distanceToImpact = closestObsticle.distance;

        if (directionChanged) {
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
