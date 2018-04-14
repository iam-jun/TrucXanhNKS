using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour {

    Button btn_play;
	// Use this for initialization
	void Start () {
        btn_play = GameObject.Find("btnPlay").GetComponent<Button>();
        btn_play.onClick.AddListener(() => PlayGame());
	}

    void PlayGame()
    {
        SceneManager.LoadScene("game_stage_1");
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
