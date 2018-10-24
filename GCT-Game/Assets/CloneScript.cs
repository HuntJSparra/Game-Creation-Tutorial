using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//William
public class CloneScript : MonoBehaviour {

    Dictionary<int, Vector3> rewindDict = new Dictionary<int, Vector3>();
    public GameObject player;
    int currentTime;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        currentTime = player.GetComponent<Controller2D>().getTime();
        if (Input.GetKey("e")) //hold down e to rewind
        {
            if (rewindDict.ContainsKey(currentTime)) // so we don't go back too far in time
            {
                //print("Rewinding");
                Vector3 rewindingPos = rewindDict[currentTime];
                //print("Rewinding to this place " + rewindingPos);
                Quaternion empty = new Quaternion();
                transform.SetPositionAndRotation(rewindingPos, empty);
            }
        }
        else //not rewinding
        {
            if(rewindDict.ContainsKey(currentTime))
            {
                GetComponent<SpriteRenderer>().enabled = true;
                Vector3 rewindingPos = rewindDict[currentTime];
                //print("Rewinding to this place " + rewindingPos);
                Quaternion empty = new Quaternion();
                transform.SetPositionAndRotation(rewindingPos, empty);
            }
            else // this clone isn't in the game at this time so disappear 
            {
                GetComponent<SpriteRenderer>().enabled = false;
            }
        }
        
    }
    

    public void updateDictionary(Dictionary<int, Vector3> temp)
    {
        rewindDict = temp;
        print("Got a new dictionary");
        currentTime = player.GetComponent<Controller2D>().getTime();
    }

}
