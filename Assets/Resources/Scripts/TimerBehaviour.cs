using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TimerBehaviour : MonoBehaviour {

    Slider timer_bar;
    float time;
    float div;
	// Use this for initialization
	void Start () {
        timer_bar = transform.GetChild(0).GetComponent<Slider>();
        time = timer_bar.value;
        //div = timer_bar.uvRect.width/time;
        StartCoroutine(reduceTime());
    }
	
	// Update is called once per frame
	void Update () {
        if (time <= 0)
        {
            SceneManager.LoadScene("game_over");
        }
    }

    IEnumerator reduceTime()
    {
        yield return new WaitForSeconds(1);
        time--;
        timer_bar.value = time;
        
        StartCoroutine(reduceTime());
    }
}
