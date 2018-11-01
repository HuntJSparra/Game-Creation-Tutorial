using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//William
public class Controller2D : MonoBehaviour {

    private Rigidbody2D rb; // Rigidbody for this object
    private const float velMod = 5; // how fast we scale movement left and right
    private const float jumpForce = 300; // how much of a force we're applying upwards when we jump
    private Dictionary<int, Vector3[]> rewindDict = new Dictionary<int, Vector3[]>(); // The array at [0] is position and at [1] is velocity
    private bool rewindingStartedLastFrame = true; // used for instantiating clone
    private int currentTime; // current time that we are on (is subtracted while we're going back in time)
    public GameObject guy; // the starter clone
    private int spawnFrame = 1; // how far back we copy the dictionary too

	void Start () {
        rb = GetComponent<Rigidbody2D>();
	}
    
    private void Update()
    {
        if (Input.GetKey("e")) // if rewinding
        {
            // setting up a clone
            if(rewindingStartedLastFrame)
                createClone();

            // Sets the current time back and sets spawn frame to the right value;
            if (currentTime > 1)
            {
                currentTime = currentTime - 1;
                spawnFrame = currentTime;
            }
        }
        else // playable
        {
            // The current time 
            currentTime = currentTime + 1;

            // adds current location to rewinding dictionary
            rewindDict[currentTime] = new Vector3[] { transform.position, rb.velocity };

            jump();

            // turns the players movement back on if they just stopped rewinding, in use so the character stops while rewinding
            if(rewindingStartedLastFrame == false)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
            }

            // used for initializing clones
            rewindingStartedLastFrame = true;
        }
    }

    // Update is called once per frame
    void FixedUpdate () {
        if (Input.GetKey("e")) // hold down e to rewind
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

    public void createClone()
    {
        Quaternion empty = new Quaternion();
        Vector3 currentPos = transform.position;
        GameObject clone = Instantiate(guy, currentPos, empty);

        clone.GetComponent<SpriteRenderer>().enabled = true;
        // for deep copying
        Dictionary<int, Vector3[]> temp = new Dictionary<int, Vector3[]>();
        for (int i = spawnFrame; i < currentTime; i++)
        {
            temp.Add(i, rewindDict[i]);
        }
        clone.GetComponent<CloneScript>().updateDictionary(temp);

        rewindingStartedLastFrame = false;
    }

    public void jump()
    {
        Vector2 jump = new Vector2(0.0f, jumpForce);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(jump);
        }
    }
}
