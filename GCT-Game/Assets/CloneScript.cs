using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneScript : MonoBehaviour {

    Dictionary<int, Vector3> rewindDict = null;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        /*
        if (Input.GetKey("e")) //hold down e to rewind
        {
            if (rewindDict.ContainsKey(rewindFrame)) // so we don't go back too far in time
            {
                print("Rewinding");
                Vector3 rewindingPos = rewindDict[rewindFrame];
                print("Rewinding to this place " + rewindingPos);
                Quaternion empty = new Quaternion();
                transform.SetPositionAndRotation(rewindingPos, empty);
                rewindFrame = rewindFrame - 1;
            }
        }
        */
    }
    

    void updateDictionary(Dictionary<int, Vector3> temp)
    {
        rewindDict = temp;
    }
}
