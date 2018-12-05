using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//William
public class Controller2DAnimated : MonoBehaviour {

    private Rigidbody2D rb; // Rigidbody for this object
    private const float velMod = 5; // how fast we scale movement left and right
    private const float jumpForce = 300; // how much of a force we're applying upwards when we jump
    private Dictionary<int, Vector3[]> rewindDict = new Dictionary<int, Vector3[]>(); // The array at [0] is position and at [1] is velocity
    private bool rewindingStartedLastFrame = true; // used for instantiating clone
    private int currentTime; // current time that we are on (is subtracted while we're going back in time)
    public GameObject guy; // the starter clone
    private int spawnFrame = 1; // how far back we copy the dictionary too
    private bool entered;

    // Wall-Related
    private Collider2D bc;
    private bool onWall;
    private Climable climbing;
    Vector3 passOnWall;

    // Animation-Related
    private Animator animator;
    private SpriteRenderer sr;

    void Start () {
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<Collider2D>();

        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        passOnWall = Vector3.zero;
        entered = false;
        onWall = false;
        climbing = new Climable();
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

            // turns the players movement back on if they just stopped rewinding, in use so the character stops while rewinding
            if(rewindingStartedLastFrame == false)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
            }

            // used for initializing clones
            rewindingStartedLastFrame = true;

            // wall stuff
            if (onWall)
            {
                passOnWall = Vector3.one;
            }
            else
            {
                passOnWall = Vector3.zero;
            }

            jump();
            climb();
        }
    }

    // Update is called once per frame
    void FixedUpdate () {
        if (Input.GetKey("e")) // hold down e to rewind
        {
            rb.bodyType = RigidbodyType2D.Static;

            // Animation-related
            animator.SetBool("Running", false);
        }
        else
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.velocity = new Vector2(Input.GetAxis("Horizontal") * velMod, rb.velocity.y);

            // Animation-related
            if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.3)
                animator.SetBool("Running", true);
            else
                animator.SetBool("Running", false);

            // Face the correct direction
            if (Input.GetAxis("Horizontal") > 0)
                sr.flipX = false;
            else if (Input.GetAxis("Horizontal") < 0)
                sr.flipX = true;
        }
    }

    public int getTime()
    {
        return currentTime;
    }

    public void createClone()
    {
        Vector3 currentPos = transform.position;
        GameObject clone = Instantiate(guy, currentPos, Quaternion.identity);

        clone.GetComponent<SpriteRenderer>().enabled = true;
        // for deep copying
        Dictionary<int, Vector3[]> temp = new Dictionary<int, Vector3[]>();
        for (int i = spawnFrame; i < currentTime; i++)
        {
            temp.Add(i, rewindDict[i]);
        }
        clone.GetComponent<CloneScriptAnimated>().updateDictionary(temp);

        rewindingStartedLastFrame = false;
    }

    public void jump()
    {
        LayerMask mask = LayerMask.GetMask("Platform");
        LayerMask climbable = LayerMask.GetMask("TransparentFX");

        Vector2 jump = new Vector2(0.0f, jumpForce);
        if (Physics2D.Raycast(rb.position, Vector2.down, 1.0f, mask))
        {
            animator.SetBool("Falling", false);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                animator.SetBool("Jumping", true);
                rb.AddForce(jump);
            }
        }
        else if (Physics2D.Raycast(rb.position, Vector2.down, 1.0f, climbable) & onWall)
        {
            animator.SetBool("Falling", false);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                animator.SetBool("Jumping", true);
                onWall = false;
                climbing.OffClimbable(bc);
                rb.AddForce(jump);
            }
        }
        else
        {
            animator.SetBool("Jumping", false);
                animator.SetBool("Falling", true); 
            }
    }

    public void climb()
    {
        if (entered & !onWall)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                onWall = true;
                animator.SetBool("Climbing", true);
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

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Climbable")
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
            animator.SetBool("Climbing", false);
            climbing.OffClimbable(bc);
            climbing = new Climable();
        }
    }
}
