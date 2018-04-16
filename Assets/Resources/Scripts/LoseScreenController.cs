using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseScreenController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(GoIntroScreen());
	}

    IEnumerator GoIntroScreen()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene(0);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
