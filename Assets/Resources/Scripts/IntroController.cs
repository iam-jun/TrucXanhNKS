using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroController : MonoBehaviour {


    // Use this for initialization
    void Start()
    {
        StartCoroutine(GoStartMenu());
        GameObject slogan = GameObject.Find("Slogan");
        Vector3 screenPosition = new Vector3(1, 1, 1);
        slogan.transform.position = Camera.main.GetComponent<Camera>().ScreenToWorldPoint(screenPosition);
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
