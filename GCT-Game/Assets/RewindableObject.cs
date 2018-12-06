using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindableObject : MonoBehaviour {

    private Dictionary<int, Vector3> previousLocations = new Dictionary<int, Vector3>(); // The array at [0] is position
    private int currentTime; // current time that we are on (is subtracted while we're going back in time)
    public GameObject player;

    //Sound FX related
    public AudioSource soundFXSource;
    //how to make new sounds
    public AudioClip ExampleSound;


    private void FixedUpdate()
    {
        currentTime = player.GetComponent<Controller2DAnimated>().getTime();
        if (Input.GetKey("e")) // if rewinding
        {
            goBackwards();
        }
        else // playable
        {
            writeDownLocation();
            //how to play sounds
            /*
            if(someCondition)
            {
                //sets the clip for the sound object
                soundFXSource.clip = FakeSound;
                //plays the clip in the clip variable
                soundFXSource.Play();
            }
            */
        }
    }

    private void goBackwards()
    {
        Vector3 rewindingPos = previousLocations[currentTime];
        transform.position = rewindingPos;
    }

    private void writeDownLocation()
    {
        //Write down location
        previousLocations[currentTime] = transform.position;
    }
}
