using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoyPosition : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Vector3 screenPosition = new Vector3(0, 0, 20);
        transform.position = Camera.main.GetComponent<Camera>().ScreenToWorldPoint(screenPosition);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
