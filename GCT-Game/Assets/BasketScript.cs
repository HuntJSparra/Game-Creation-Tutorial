using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketScript : MonoBehaviour{
    private class BasketSegment {
        public Vector3 originalPosition;
        public Transform segment;

        public BasketSegment(Vector3 originalPosition, Transform segment) {
            this.originalPosition = originalPosition;
            this.segment = segment;
        }
    }
    private Dictionary<int, Vector3> previousLocations = new Dictionary<int, Vector3>(); // The array at [0] is position
    private int currentTime; // current time that we are on (is subtracted while we're going back in time)
    public GameObject player;

    public float sinkSpeed;

    public float fallDir = 1;

    private BasketSegment basket;
    private BasketSegment rope;
    private BasketSegment gateSegment;

    private Transform lastSegment;
    public Transform gate;
    public Transform gateRope;

	// Use this for initialization
	void Start() {
        Transform basketTransform = transform.Find("Basket");
        basket = new BasketSegment(basketTransform.position, basketTransform);

        Transform ropeTransform = transform.Find("RopeSegment2");
        rope = new BasketSegment(ropeTransform.position, ropeTransform);

        lastSegment = basketTransform.GetChild(0);

        gateSegment = new BasketSegment(gate.transform.position, gate);
	}
	
	// Update is called once per frame
	void FixedUpdate() {
        currentTime = player.GetComponent<Controller2DAnimated>().getTime();
        if (Input.GetKey("e")) // hold down e to rewind
        {
            if (fallDir < 1 && fallDir > -1)
            {
                fallDir += 0.05f;
            }
            if (previousLocations.ContainsKey(currentTime))
            {
                GoBackwards();
                if (basket.segment.position.y <= 2.78)
                {
                    if (basket.segment.position.y > 1.62)
                    {
                        Vector3 newPos = rope.originalPosition;
                        newPos -= new Vector3(0, (float)(2.78 - basket.segment.position.y), 0);
                        rope.segment.position = newPos;

                        lastSegment.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                    }
                    else
                    {
                        lastSegment.gameObject.GetComponent<SpriteRenderer>().enabled = true;
                    }
                }
            }
        }
        else
        {
            if (fallDir < 1 && fallDir > -1)
            {
                fallDir += 0.05f;
            }

            if ((fallDir < 0 && basket.segment.position.y > 0) || (fallDir > 0 && basket.segment.position.y < 3.36))
            {
                basket.segment.position += new Vector3(0, fallDir * sinkSpeed, 0);
                if (basket.segment.position.y <= 2.78)
                {
                    if (basket.segment.position.y > 1.62)
                    {
                        Vector3 newPos = rope.originalPosition;
                        newPos -= new Vector3(0, (float)(2.78 - basket.segment.position.y), 0);
                        rope.segment.position = newPos;

                        lastSegment.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                    }
                    else
                    {
                        lastSegment.gameObject.GetComponent<SpriteRenderer>().enabled = true;
                    }
                }
            }
            WriteDownLocation();
        }
        gate.transform.position = gateSegment.originalPosition + (basket.originalPosition - basket.segment.position);
        gateRope.transform.position = new Vector3(gateRope.transform.position.x, 1.48f - 0.65f * (basket.segment.transform.position.y - basket.originalPosition.y) / (basket.originalPosition.y - 1.62f), gateRope.transform.position.z);
        gateRope.transform.localScale = new Vector3(gateRope.transform.localScale.x, 2.5f + 1.1f * (basket.segment.transform.position.y - basket.originalPosition.y) / (basket.originalPosition.y - 1.62f), gateRope.transform.localScale.z);
    }

    void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.tag == "Player") {
            fallDir = -1;
        }
    }

    void OnCollisionExit2D(Collision2D col) {
        if (col.gameObject.tag == "Player" && fallDir == -1)
            fallDir += 0.01f;
    }

    
    void WriteDownLocation()
    {
        //Write down location
        previousLocations[currentTime] = basket.segment.position;
    }

    
    void GoBackwards()
    {
        Vector3 rewindingPos = previousLocations[currentTime];
        basket.segment.position = rewindingPos;
    }
}
