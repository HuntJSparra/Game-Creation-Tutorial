using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketScript : MonoBehaviour {
    private class BasketSegment {
        public Vector3 originalPosition;
        public Transform segment;

        public BasketSegment(Vector3 originalPosition, Transform segment) {
            this.originalPosition = originalPosition;
            this.segment = segment;
        }
    }

    public float sinkSpeed;

    public float fallDir = 1;

    private BasketSegment basket;
    private BasketSegment rope;

    private Transform lastSegment;

	// Use this for initialization
	void Start() {
        Transform basketTransform = transform.Find("Basket");
        basket = new BasketSegment(basketTransform.position, basketTransform);

        Transform ropeTransform = transform.Find("RopeSegment2");
        rope = new BasketSegment(ropeTransform.position, ropeTransform);

        lastSegment = basketTransform.GetChild(0);
	}
	
	// Update is called once per frame
	void Update() {
        if (fallDir < 1 && fallDir > -1) {
            fallDir += 0.01f;
        }

		if ((fallDir < 0 && basket.segment.position.y > 0) || (fallDir > 0 && basket.segment.position.y < 3.36)) {
            basket.segment.position += new Vector3(0, fallDir*sinkSpeed, 0);
            if (basket.segment.position.y <= 2.78) {
                if (basket.segment.position.y > 1.62) {
                    Vector3 newPos = rope.originalPosition;
                    newPos -= new Vector3(0, (float)(2.78-basket.segment.position.y), 0);
                    rope.segment.position = newPos;

                    lastSegment.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                } else {
                    lastSegment.gameObject.GetComponent<SpriteRenderer>().enabled = true;
                }
            }
        } else {
        }
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
}
