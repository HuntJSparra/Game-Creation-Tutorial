using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.SceneManagement;


//add code for turning off box colider



//William
public class CloneScriptAnimated : MonoBehaviour
{

    Dictionary<int, Vector3[]> rewindDict = new Dictionary<int, Vector3[]>();
    public GameObject player;
    private int currentTime;
    private Rigidbody2D rb; // Rigidbody for this object
    private CapsuleCollider2D bc; // box collider 
    private float acceptableDifferenceInPosition = .5f; //this is how far it is ok to be away from the actual position of where the 
    //private bool onWall;                                //clone is now vs where the player was at that time
    private Climable climb;                             // Use this for initialization
    private SpriteRenderer sr;
    private Vector2 dir;

    //Sound FX related
    public AudioSource soundFXSource;
    public AudioClip restartLevel;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<CapsuleCollider2D>();
        //onWall = false;
        climb = new Climable();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        currentTime = player.GetComponent<Controller2DAnimated>().getTime();
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

                //direction facing
                dir = rewindDict[currentTime][3];
                if (dir == Vector2.right)
                    sr.flipX = false;
                else if(dir == Vector2.left)
                    sr.flipX = true;
                // paradox checking by velocity

                if (paradoxByVelocity(rewindingPos, transform.position))
                {
                    if (!soundFXSource.isPlaying)
                    {
                        soundFXSource.clip = restartLevel;
                        soundFXSource.Play();
                    }

                    if (!soundFXSource.isPlaying)
                    {
                        SceneManager.LoadScene("Hunt");
                    }
                    else //fade to black
                    {

                    }

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
        currentTime = player.GetComponent<Controller2DAnimated>().getTime();
    }

    public void goingBackwards()
    {
        Vector3 rewindingPos = rewindDict[currentTime][0];
        //Vector2 rewindingVelo = rewindDict[currentTime][1];
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
        rb.bodyType = RigidbodyType2D.Dynamic;
        if (rewindDict[currentTime][2] == Vector3.one)
        {
            climb.OnClimbable(bc);
        }
        else if (rewindDict[currentTime][2] == Vector3.zero)
        {
            climb.OffClimbable(bc);
        }
        rb.velocity = rewindingVelo;
        return rewindingPos;
    }

    public void paradoxBySeeing()
    {
        LayerMask mask = LayerMask.GetMask("Player");
        RaycastHit2D hit = Physics2D.Raycast(rb.position, rewindDict[currentTime][3], 500.0f, mask);
        Collider2D playerCollider = hit.collider;
        if (playerCollider != null)
        {
            //print("Is playerCollider on?" + playerCollider.enabled);
            if (playerCollider.enabled == true)
            {
                if (!soundFXSource.isPlaying)
                {
                    soundFXSource.clip = restartLevel;
                    soundFXSource.Play();
                }

                if (!(soundFXSource.isPlaying))
                {
                    SceneManager.LoadScene("Hunt");
                }
                else //fade to black
                {
                    
                }

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

}
