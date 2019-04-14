using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityManager : MonoBehaviour {

    float defaultGravity;
    Vector2 gravity, prevGravity;
    List<PlayerControls> entities;

    private void Awake() {
        entities = new List<PlayerControls>();
    }

    // Start is called before the first frame update
    void Start() {
        defaultGravity = -1 * Physics2D.gravity.y;
        gravity = new Vector2(0f, -defaultGravity);
        Physics2D.gravity = Vector2.zero;

        prevGravity = Vector2.zero;
    }

    // Update is called once per frame
    void Update() {
        GravityUpdate();
    }

    void GravityUpdate() {
        // TODO: Fix Gravity Speed
        // TODO: Fix left over forces from previous change
        // TODO: Color matching between gravity changes
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            gravity = new Vector2(defaultGravity, 0f);
            UpdateForces(90);
        } else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            gravity = new Vector2(-defaultGravity, 0f);
            UpdateForces(270);
        } else if (Input.GetKeyDown(KeyCode.UpArrow)) {
            gravity = new Vector2(0f, defaultGravity);
            UpdateForces(180);
        } else if (Input.GetKeyDown(KeyCode.DownArrow)) {
            gravity = new Vector2(0f, -defaultGravity);
            UpdateForces(0);
        }
    }

    void UpdateForces(float rotationGoal) {
        foreach(PlayerControls entity in entities) {
            Rigidbody2D rb = entity.GetComponent<Rigidbody2D>();
            rb.AddForce(-prevGravity);
            rb.AddForce(gravity);
            entity.directionChanged = true;
            entity.rotationGoal = rotationGoal;
            entity.gravity = gravity;
        }

        prevGravity = gravity;
    }

    public void AddEntity(PlayerControls entity) {
        entities.Add(entity);
    }
}
