using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//William
public class Controller2D : MonoBehaviour {

    private Rigidbody2D rb; // rigid body for this object
    private float velMod; //how fast we scale movement left and right
    private float jumpForce; //how much of a force we're applying upwards when we jump
    private Dictionary<int, Vector3> rewindDict = new Dictionary<int, Vector3>();
    private bool rewindingStartedLastFrame = true;
    private int currentTime;
    public GameObject guy;
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
            Debug.Log("Has the key!");
            Quaternion empty = new Quaternion();
            //setting up a clone
            if (rewindingStartedLastFrame)
            {
                Vector3 currentPos = transform.position;
                GameObject clone = Instantiate(guy, currentPos, empty);

                clone.GetComponent<SpriteRenderer>().enabled = true;
                Dictionary<int, Vector3> temp = new Dictionary<int, Vector3>();
                for(int i = 1; i < currentTime; i++)
                {
                    temp.Add(i, rewindDict[i]);
                }
                clone.GetComponent<CloneScript>().updateDictionary(temp);
                print("spawning a guy");
                
                rewindingStartedLastFrame = false;
            }

            //removing the position 
            rewindDict.Remove(currentTime);
            currentTime = currentTime - 1;
        }
        else //playable
        {
            //The current time 
            currentTime = currentTime + 1;

            //adds current location to rewinding dictionary
            rewindDict.Add(currentTime, transform.position);
            //print(" Adding " + Time.frameCount + " " + transform.position); //prints out where you are going next // Get rid of later

            //float dist = .51F;
            Vector2 jump = new Vector2(0.0f, jumpForce);
            //hit = Physics2D.Raycast(rb.transform.position, Vector2.down, dist);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.AddForce(jump);
            }

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
