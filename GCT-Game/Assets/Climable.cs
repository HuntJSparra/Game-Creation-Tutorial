using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climable : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player" || other.tag == "Clone")
        {
            other.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player" || other.tag == "Clone")
        {
            other.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1.5F;
        }
    }
}
