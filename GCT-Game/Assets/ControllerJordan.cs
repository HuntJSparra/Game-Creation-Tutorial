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
    private Collider2D bc;
    private bool entered;
    private bool onWall;
    private Climable climbing;
    Vector3 passOnWall;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<Collider2D>();
        passOnWall = Vector3.zero;
        entered = false;
        onWall = false;
        climbing = new Climable();
    }

    private void FixedUpdate()
    {
        if (Input.GetKey("e")) // if rewinding
        {
            rb.bodyType = RigidbodyType2D.Static;
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
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.velocity = new Vector2(Input.GetAxis("Horizontal") * velMod, rb.velocity.y);
            climb();
            jump();
            // The current time 
            currentTime = currentTime + 1;

            //have to write down what direction the player is facing so that the clones know when they're doing checking
            writeDownDirection();

            // adds current location, current velocity, and direction the player is facing to rewinding dictionary
            //print("Direction facing" + directionFacing);

            if (onWall)
            {
                passOnWall = Vector3.one;
            }
            else
            {
                passOnWall = Vector3.zero;
            }

            rewindDict[currentTime] = new Vector3[] { transform.position, rb.velocity, directionFacing, passOnWall };
            
            // turns the players movement back on if they just stopped rewinding, in use so the character stops while rewinding
            if (rewindingStartedLastFrame == false)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
            }

            // used for initializing clones
            rewindingStartedLastFrame = true;
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
        clone.GetComponent<JordanClones>().updateDictionary(temp);

        rewindingStartedLastFrame = false;
    }

    public void jump()
    {
        LayerMask mask = LayerMask.GetMask("Platform");
        LayerMask climbable = LayerMask.GetMask("TransparentFX");

        Vector2 jump = new Vector2(0.0f, jumpForce);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Physics2D.Raycast(rb.position, Vector2.down, 1.0f, mask))
            {
                rb.AddForce(jump);
            }
            else  if (Physics2D.Raycast(rb.position, Vector2.down, 1.0f, climbable)&onWall)
            {
                onWall = false;
                climbing.OffClimbable(bc);
                rb.AddForce(jump);
            }
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
            climbing = collision.GetComponent<Climable>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Climbable")
        {
            entered = false;
            onWall = false;
            climbing.OffClimbable(bc);
            climbing = new Climable();
        }
    }

    public void climb()
    {
        if (entered & !onWall)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                onWall = true;
                climbing.OnClimbable(bc);
            }
        }
        if (onWall)
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
}