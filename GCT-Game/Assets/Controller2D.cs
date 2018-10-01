using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller2D : MonoBehaviour {

    private Rigidbody2D rb;
    private float velMod;
    private float jumpForce;
    //private RaycastHit2D hit;

	void Start () {
        rb = GetComponent<Rigidbody2D>();
        velMod = 5;
        jumpForce = 300;
	}

    private void Update()
    {
        //float dist = .51F;
        Vector2 jump = new Vector2(0.0f, jumpForce);
        //hit = Physics2D.Raycast(rb.transform.position, Vector2.down, dist);
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
