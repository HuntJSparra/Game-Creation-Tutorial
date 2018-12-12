using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour {

	// Use this for initialization
	void Start () {
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("DontDestroy")) {
            if (go.name == gameObject.name && go != gameObject)
                Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
	}
}
