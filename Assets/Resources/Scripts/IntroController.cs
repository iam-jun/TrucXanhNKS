using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroController : MonoBehaviour {


	// Use this for initialization
	void Start () {
        StartCoroutine(GoStartMenu());
	}

    IEnumerator GoStartMenu()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("start_menu");
    }

    // Update is called once per frame
    void Update () {
		
	}
}
