using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//add code for turning off box colider



//William
public class CloneScript : MonoBehaviour {

    Dictionary<int, Vector3[]> rewindDict = new Dictionary<int, Vector3[]>();
    public GameObject player;
    private int currentTime;
    private Rigidbody2D rb; // Rigidbody for this object
    private BoxCollider2D bc; // box collider 
    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
    }
	
	// Update is called once per frame
	void Update () {
        currentTime = player.GetComponent<Controller2D>().getTime();
        if (Input.GetKey("e")) // hold down e to rewind
        {
            // setting position if this clone is in the dictionary
            if (rewindDict.ContainsKey(currentTime)) 
            {
                goingBackwards();
            }
            else // while rewinding and this clone is not supposed to be displayed right now
            {
                GetComponent<SpriteRenderer>().enabled = false;
                bc.enabled = false;
            }
        }
        else // going forward
        {
            if(rewindDict.ContainsKey(currentTime)) 
            {
                Vector3 rewindingPos = goingForwards();

                // paradox checking by velocity
                if (!transform.position.Equals(rewindingPos))
                {
                    print("Paradox by Velocity");
                }
            }
            else // this clone isn't in the game at this time so disappear 
            {
                GetComponent<SpriteRenderer>().enabled = false;
                bc.enabled = false;
            }
        }
    }
    
    public void updateDictionary(Dictionary<int, Vector3[]> temp)
    {
        rewindDict = temp;
        currentTime = player.GetComponent<Controller2D>().getTime();
    }

    public void goingBackwards()
    {
        Vector3 rewindingPos = rewindDict[currentTime][0];
        Vector2 rewindingVelo = rewindDict[currentTime][1];
        //print("Rewinding to this place " + rewindingPos);
        GetComponent<SpriteRenderer>().enabled = true;
        transform.position = rewindingPos;
    }

    public Vector3 goingForwards()
    {
        GetComponent<SpriteRenderer>().enabled = true;
        Vector3 rewindingPos = rewindDict[currentTime][0];
        Vector2 rewindingVelo = rewindDict[currentTime][1];

        //Quaternion empty = new Quaternion();
        //transform.SetPositionAndRotation(rewindingPos, empty);
        // turns the bounding box back on 
        bc.enabled = true;
        rb.velocity = rewindingVelo;
        return rewindingPos;
    }
}
