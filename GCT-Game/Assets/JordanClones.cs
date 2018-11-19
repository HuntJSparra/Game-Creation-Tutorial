using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//add code for turning off box colider



//Jordan
public class JordanClones : MonoBehaviour
{

    Dictionary<int, Vector3[]> rewindDict = new Dictionary<int, Vector3[]>();
    public GameObject player;
    private int currentTime;
    private Rigidbody2D rb; // Rigidbody for this object
    private CapsuleCollider2D bc; // box collider 
    private float acceptableDifferenceInPosition = .5f; //this is how far it is ok to be away from the actual position of where the 
    private bool onWall;                                //clone is now vs where the player was at that time
    private Climable climb;                             // Use this for initialization
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<CapsuleCollider2D>();
        onWall = false;
        climb = new Climable();
    }

    // Update is called once per frame
    void Update()
    {
        currentTime = player.GetComponent<ControllerJordan>().getTime();
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
                rb.bodyType = RigidbodyType2D.Static;
            }
        }
        else // going forward
        {
            if (rewindDict.ContainsKey(currentTime))
            {
                Vector3 rewindingPos = goingForwards();

                // paradox checking by velocity

                if (paradoxByVelocity(rewindingPos, transform.position))
                {
                    print("Paradox by Velocity");
                }

                paradoxBySeeing();
            }
            else // this clone isn't in the game at this time so disappear 
            {
                GetComponent<SpriteRenderer>().enabled = false;
                bc.enabled = false;
                rb.bodyType = RigidbodyType2D.Static;
            }
        }
    }

    public void updateDictionary(Dictionary<int, Vector3[]> temp)
    {
        rewindDict = temp;
        currentTime = player.GetComponent<ControllerJordan>().getTime();
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
        bc.enabled = true;
        rb.bodyType = RigidbodyType2D.Dynamic;
        GetComponent<SpriteRenderer>().enabled = true;
        Vector3 rewindingPos = rewindDict[currentTime][0];
        Vector2 rewindingVelo = rewindDict[currentTime][1];
        if(rewindDict[currentTime][3] == Vector3.one)
        {
            climb.OnClimbable(bc);
        }
        else if(rewindDict[currentTime][3] == Vector3.zero)
        {
            climb.OffClimbable(bc);
        }

        //Quaternion empty = new Quaternion();
        //transform.SetPositionAndRotation(rewindingPos, empty);
        // turns the bounding box back on 
        
        rb.velocity = rewindingVelo;
        return rewindingPos;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Climbable")
        {
            climb = collision.GetComponent<Climable>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Climbable")
        {
            climb.OffClimbable(bc);
            climb = new Climable();
        }
    }

    public void paradoxBySeeing()
    {
        LayerMask mask = LayerMask.GetMask("Player");

        //print("Clone is facing" + rewindDict[currentTime][2]);
        RaycastHit2D hit = Physics2D.Raycast(rb.position, rewindDict[currentTime][2], 500.0f, mask);
        Collider2D playerCollider = hit.collider;
        if (playerCollider != null)
        {
            //print("Is playerCollider on?" + playerCollider.enabled);
            if (playerCollider.enabled == true)
            {
                print("Paradox where a clone saw you");
            }
        }
    }

    public bool paradoxByVelocity(Vector2 positionInDictionary, Vector2 currentPosition)
    {
        if (Math.Abs(positionInDictionary.x - currentPosition.x) >= acceptableDifferenceInPosition)
            return true;
        if (Math.Abs(positionInDictionary.y - currentPosition.y) >= acceptableDifferenceInPosition)
            return true;

        return false;
    }

}
