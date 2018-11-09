using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Jordan
public class ControllerJordan : MonoBehaviour
{

    private Rigidbody2D rb; // Rigidbody for this object
    private const float velMod = 5; // how fast we scale movement left and right
    private const float jumpForce = 300; // how much of a force we're applying upwards when we jump
    private Dictionary<int, Vector3[]> rewindDict = new Dictionary<int, Vector3[]>(); // The array at [0] is position and at [1] is velocity
    private bool rewindingStartedLastFrame = true; // used for instantiating clone
    private int currentTime; // current time that we are on (is subtracted while we're going back in time)
    public GameObject guy; // the starter clone
    private int spawnFrame = 1; // how far back we copy the dictionary too
    private Vector2 directionFacing; //direction the player character is facing, either [1,0] or [-1,0] (or [0,0] if the player hasn't moved yet)
    private BoxCollider2D bc;
    private bool entered;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        entered = false;
    }

    private void Update()
    {
        if (Input.GetKey("e")) // if rewinding
        {
            // setting up a clone
            if (rewindingStartedLastFrame)
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

            //have to write down what direction the player is facing so that the clones know when they're doing checking
            writeDownDirection();

            // adds current location, current velocity, and direction the player is facing to rewinding dictionary
            //print("Direction facing" + directionFacing);
            rewindDict[currentTime] = new Vector3[] { transform.position, rb.velocity, directionFacing };

            if (entered)
            {
                climb();
            }
            else
            {
                jump();
            }

            // turns the players movement back on if they just stopped rewinding, in use so the character stops while rewinding
            if (rewindingStartedLastFrame == false)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
            }

            // used for initializing clones
            rewindingStartedLastFrame = true;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
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

    //called by the clones so they can get the current time
    public int getTime()
    {
        return currentTime;
    }

    public void createClone()
    {
        Vector3 currentPos = transform.position;
        Quaternion empty = new Quaternion();
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
        LayerMask mask = LayerMask.GetMask("Platform");

        Vector2 jump = new Vector2(0.0f, jumpForce);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Physics2D.Raycast(rb.position, Vector2.down, 1.0f, mask))
            {
                rb.AddForce(jump);
            }
            entered = false;
        }
    }

    public void writeDownDirection()
    {
        if (Input.GetAxis("Horizontal") != 0)
        {
            if (Input.GetAxis("Horizontal") > 0)
                directionFacing = Vector2.right;
            else
                directionFacing = Vector2.left;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Climbable")
        {
            entered = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Climbable")
        {
            entered = false;
        }
    }

    public void climb()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            rb.velocity = new Vector2(rb.velocity.x, 2);
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            rb.velocity = new Vector2(rb.velocity.x, -2);
        }
        else
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }
    }
}