using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//William
public class CloneScript : MonoBehaviour {

    Dictionary<int, Vector3[]> rewindDict = new Dictionary<int, Vector3[]>();
    public GameObject player;
    int currentTime;
    private Rigidbody2D rb; // Rigidbody for this object
    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update () {
        currentTime = player.GetComponent<Controller2D>().getTime();
        if (Input.GetKey("e")) //hold down e to rewind
        {
            if (rewindDict.ContainsKey(currentTime)) //setting position if this clone is in the dictionary
            {
                //print("Rewinding");
                Vector3 rewindingPos = rewindDict[currentTime][0];
                Vector2 rewindingVelo = rewindDict[currentTime][1];
                //print("Rewinding to this place " + rewindingPos);
                Quaternion empty = new Quaternion();
                GetComponent<SpriteRenderer>().enabled = true;
                transform.position = (rewindingPos);
            }
            else //while rewinding and this clone is not supposed to be displayed right now
            {
                GetComponent<SpriteRenderer>().enabled = false;
            }
        }
        else //going forward
        {
            if(rewindDict.ContainsKey(currentTime)) 
            {
                GetComponent<SpriteRenderer>().enabled = true;
                Vector3 rewindingPos = rewindDict[currentTime][0];
                Vector2 rewindingVelo = rewindDict[currentTime][1];
                //print(rewindingVelo);
                //print("Rewinding to this place " + rewindingPos);
                Quaternion empty = new Quaternion();
                //transform.SetPositionAndRotation(rewindingPos, empty);
                rb.velocity = rewindingVelo;
                if(!transform.position.Equals(rewindingPos))
                {
                    print("Paradox");
                }
            }
            else // this clone isn't in the game at this time so disappear 
            {
                GetComponent<SpriteRenderer>().enabled = false;
            }
        }
        
    }
    


    public void updateDictionary(Dictionary<int, Vector3[]> temp)
    {
        rewindDict = temp;
        print("Got a new dictionary");
        currentTime = player.GetComponent<Controller2D>().getTime();
    }

}
