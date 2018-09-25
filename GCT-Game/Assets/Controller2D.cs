using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller2D : MonoBehaviour {

    private Rigidbody2D rb;
    private float velMod;
    private float jumpForce;

	void Start () {
        rb = GetComponent<Rigidbody2D>();
        velMod = 5;
        jumpForce = 300;
	}

    private void Update()
    {
        Vector2 jump = new Vector2(0.0f, jumpForce);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(jump);
        }
        

    }

    // Update is called once per frame
    void FixedUpdate () {
        rb.velocity = new Vector2(Input.GetAxis("Horizontal")*velMod, rb.velocity.y);
    }
}
