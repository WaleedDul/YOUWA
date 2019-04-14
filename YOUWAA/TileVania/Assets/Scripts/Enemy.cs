﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public float damageDone = 10f;
    private float health = 50f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (health <= 0) {
            Debug.Log("Enemy Dead");
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        PlayerControls player = collision.gameObject.GetComponent<PlayerControls>();
        if (player) {
            health -= player.damageDone;
        }
    }
}
