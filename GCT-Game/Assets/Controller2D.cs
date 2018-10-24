using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//William
public class Controller2D : MonoBehaviour {

    private Rigidbody2D rb; // ridgid body for this object
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
            if (rewindDict.ContainsKey(currentTime)) // so we don't go back too far in time
            {
                Quaternion empty = new Quaternion();
                if (rewindingStartedLastFrame)
                {
                    Vector3 currentPos = transform.position;
                    GameObject clone = Instantiate(guy,currentPos,empty);
                    //myObject.GetComponent<MyScript>().MyFunction();
                    
                    clone.GetComponent<CloneScript>().updateDictionary(rewindDict);
                    print("spawning a guy");

                    rewindingStartedLastFrame = false;
                }
                //print("Rewinding");
                //Vector3 rewindingPos = rewindDict[rewindFrame];
                //print("Rewinding to this place " + rewindingPos);
                GetComponent<Rigidbody2D>().simulated = false;
                //transform.SetPositionAndRotation(rewindingPos, empty);
                currentTime = currentTime - 1;
                
            }
        }
        else //playable
        {
            if (GetComponent<Rigidbody2D>().simulated == false)
            {
                GetComponent<Rigidbody2D>().simulated = true;
            }
            //adds current location to rewinding dictionary
            rewindDict.Add(currentTime, transform.position);
            print(" Adding " + Time.frameCount + " " + transform.position); //prints out where you are going next // Get rid of later

            //float dist = .51F;
            Vector2 jump = new Vector2(0.0f, jumpForce);
            //hit = Physics2D.Raycast(rb.transform.position, Vector2.down, dist);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.AddForce(jump);
            }

            //The current time 
            currentTime = currentTime + 1;

            rewindingStartedLastFrame = true;
        }
    }

    // Update is called once per frame
    void FixedUpdate () {
        rb.velocity = new Vector2(Input.GetAxis("Horizontal")*velMod, rb.velocity.y);
    }

    public int getTime()
    {
        return currentTime;
    }
}
