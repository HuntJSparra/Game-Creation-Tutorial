using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climable : MonoBehaviour {
    bool entered;
    int numClimbers;
	// Use this for initialization
	void Start () {
        entered = false;
        numClimbers = 0;
	}
	
	// Update is called once per frame
	void Update () {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player" || other.tag == "Clone")
        {
            entered = true;
            numClimbers++;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player" || other.tag == "Clone")
        {
            numClimbers--;
            if(numClimbers == 0)
            {
                entered = false;
            }
        }
    }

    public void OnClimbable(Collider2D other)
    {
        if (entered)
        {
            other.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        }
    }

    public void OffClimbable(Collider2D other)
    {
        if (entered)
        {
            other.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1.5F;
        }
    }
}
