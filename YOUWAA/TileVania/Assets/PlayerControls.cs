using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerControls : MonoBehaviour {

    Rigidbody2D myRigidBody;
    float rotationGoal;
    float defaultGravity;
    float totalDistance;
    [SerializeField] float gravityScale = 1f;

	// Use this for initialization
	void Start () {
        myRigidBody = GetComponent<Rigidbody2D>();
        defaultGravity = -1 * Physics2D.gravity.y;
	}
	
	// Update is called once per frame
	void Update () {
        GravityControl();

    }



    private void GravityControl()
    {

        bool direcitonChanged = false;
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            myRigidBody.gravityScale = gravityScale;
            Physics2D.gravity = new Vector2(defaultGravity, 0f);
            rotationGoal = 90;
            direcitonChanged = true;
        }

        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            myRigidBody.gravityScale = gravityScale;
            Physics2D.gravity = new Vector2(-defaultGravity, 0f);
            rotationGoal = 270;
            direcitonChanged = true;
        }

        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            myRigidBody.gravityScale = gravityScale;
            Physics2D.gravity = new Vector2(0f, defaultGravity);
            rotationGoal = 180;
            direcitonChanged = true;
        }

        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            myRigidBody.gravityScale = gravityScale;
            Physics2D.gravity = new Vector2(0f, -defaultGravity);
            rotationGoal = 0;
            direcitonChanged = true;
        }

        RaycastHit2D closestObsticle = Physics2D.Raycast(transform.position, myRigidBody.velocity.normalized, 10000f, LayerMask.GetMask("Ground"));
        float distanceToImpact = closestObsticle.distance;
        
        if (direcitonChanged)
        {
            totalDistance = distanceToImpact;
            direcitonChanged = false;
        }

        Debug.Log(Time.deltaTime);


        transform.rotation = Quaternion.SlerpUnclamped(transform.rotation, Quaternion.Euler(0, 0, rotationGoal), totalDistance/myRigidBody.velocity.magnitude);
    }

}
