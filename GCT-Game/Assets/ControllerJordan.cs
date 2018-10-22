using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerJordan : MonoBehaviour {

    private Rigidbody2D rb; // ridgid body for this object
    private float velMod; //how fast we scale movement left and right
    private float jumpForce; //how much of a force we're applying upwards when we jump
    private Stack<Vector3> rewindStack = new Stack<Vector3>();
    private Stack<Vector2> moveStack = new Stack<Vector2>();
    private Stack<string> pressedKey = new Stack<string>();
    private int CurFrame; //current frame that you're on
    private bool CurRewind;
    private int rewindDist;
    private Object ClonePrefab;

    //private RaycastHit2D hit;

	void Start () {
        rb = GetComponent<Rigidbody2D>();
        velMod = 5;
        jumpForce = 300;
        CurRewind = false;
        ClonePrefab = Resources.Load("Clone.prefab");
    }
    
    private void Update(){

        if (Input.GetKey("e")) /*hold down e to rewind*/{
            if (CurRewind == false)
            {
                CurRewind = true;
                rewindDist = 0;
                pressedKey.Push("e");
                GameObject newClone = (GameObject)Instantiate(ClonePrefab, transform.position, new Quaternion());
                CloneController control = newClone.GetComponent<CloneController>();
                control = new CloneController(rewindStack, moveStack, pressedKey);

            }
            if (rewindDist<rewindStack.Count) /* so we don't go back too far in time*/{
                print("Rewinding");
                rewindDist = rewindDist + 1;
            }
        }
        else /*playable*/{
            //adds current location to rewinding dictionary
            rewindStack.Push(transform.position);
            moveStack.Push(rb.velocity);
            print(" Adding " + Time.frameCount + " " + transform.position); //prints out where you are going next // Get rid of later
            //float dist = .51F;
            Vector2 jump = new Vector2(0.0f, jumpForce);
            //hit = Physics2D.Raycast(rb.transform.position, Vector2.down, dist);
            if (Input.GetKeyDown(KeyCode.Space)){
                rb.AddForce(jump);
            }

            //when we rewind we need a frame to start from
            CurFrame = Time.frameCount;
            CurRewind = false;
            pressedKey.Push(rewindDist.ToString());
        }
        
    }

    // Update is called once per frame
    void FixedUpdate () {
        if (Input.GetKey("e"))
        {
            float grav = .2943F;
            rb.velocity = new Vector2(0, grav);
        }
        else
        {
            rb.velocity = new Vector2(Input.GetAxis("Horizontal") * velMod, rb.velocity.y);
        }
    }
}
