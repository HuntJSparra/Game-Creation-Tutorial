using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//William
public class Controller2D : MonoBehaviour {

    private Rigidbody2D rb; // Rigidbody for this object
    private float velMod; //how fast we scale movement left and right
    private float jumpForce; //how much of a force we're applying upwards when we jump
    private Dictionary<int, Vector3[]> rewindDict = new Dictionary<int, Vector3[]>();
    private bool rewindingStartedLastFrame = true;
    private int currentTime; // current time that we are on (is subtracted while we're going back in time)
    private Vector2 momentum; //current velocity of the player 
    public GameObject guy; //the starter clone
    private int spawnFrame = 1;
    //private RaycastHit2D hit;

	void Start () {
        rb = GetComponent<Rigidbody2D>();
        velMod = 5;
        jumpForce = 300;
	}
    
    private void Update()
    {
        if (Input.GetKey("e")) //hold down e to rewind
        {
            Quaternion empty = new Quaternion();
            //setting up a clone
            if(rewindingStartedLastFrame)
            {
                Vector3 currentPos = transform.position;
                GameObject clone =  Instantiate(guy, currentPos, empty);
                
                clone.GetComponent<SpriteRenderer>().enabled = true;
                //for deep copying
                Dictionary<int, Vector3[]> temp = new Dictionary<int, Vector3[]>();
                for(int i = spawnFrame; i < currentTime; i++)
                {
                    temp.Add(i, rewindDict[i]);
                }
                clone.GetComponent<CloneScript>().updateDictionary(temp);
                
                //print("spawning a guy");
                
                rewindingStartedLastFrame = false;
            }

            //removing the position from rewind dict
            //rewindDict.Remove(currentTime);
            if (currentTime > 1)
            {
                currentTime = currentTime - 1;
                spawnFrame = currentTime;
            }
        }
        else //playable
        {
            //The current time 
            currentTime = currentTime + 1;

            //adds current location to rewinding dictionary
            rewindDict[currentTime] = new Vector3[] { transform.position, rb.velocity };
            //print(" Adding " + Time.frameCount + " " + transform.position); //prints out where you are going next // Get rid of later

            //float dist = .51F;
            Vector2 jump = new Vector2(0.0f, jumpForce);
            //hit = Physics2D.Raycast(rb.transform.position, Vector2.down, dist);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.AddForce(jump);
            }

            if(rewindingStartedLastFrame == false)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
                //sets velocity of player character to momentum;
                //print("setting the velocity to momentum");
                rb.velocity = momentum;
            }


            //gets the velocity of the player character so after they rewind they are still moving in the direction before the rewind 
            momentum = rb.velocity;


            //used for initializing clones
            rewindingStartedLastFrame = true;
        }
    }

    // Update is called once per frame
    void FixedUpdate () {
        if (Input.GetKey("e")) //hold down e to rewind
        {
            rb.bodyType = RigidbodyType2D.Static;

        }
        else
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.velocity = new Vector2(Input.GetAxis("Horizontal") * velMod, rb.velocity.y);
        }
    }

    public int getTime()
    {
        return currentTime;
    }
}
