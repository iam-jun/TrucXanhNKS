﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Gameplay : MonoBehaviour {

    [SerializeField]
    private Transform panel;
    [SerializeField]
    private GameObject button;
    Sprite[] backgrounds;
    Sprite main_bg;
    private int[] stages;
    [SerializeField]
    int current_stage;
    private GameObject btn;
    private GameObject[] btn_list;
    public List<Sprite> game_sprites;
    public bool firstGuess, secondGuess;
    string firstName, secondName;
    int firstIndex, secondIndex, correctCount;
    Text timeText;
    // Use this for initialization
    private void Awake()
    {
        backgrounds = Resources.LoadAll<Sprite>("Images/Puzzle");
        main_bg = Resources.Load<Sprite>("Images/question_mark");
    }

    void Start () {
        stages = new int[] {4, 8, 18 };
        game_sprites = new List<Sprite>();
        firstGuess = false;
        secondGuess = false;
        correctCount = 0;
        timeText = GameObject.Find("TimeText").GetComponent<Text>();
        LoadStage(current_stage);
        
    }

    void LoadStage(int stage)
    {
        timeText.text = "30";
        game_sprites.Clear();
        for (int i = 0; i < stages[stage]; i++)
        {
            btn = Instantiate(button);
            btn.name = "" + i;
            btn.transform.SetParent(panel, false);
        }
        AddSprite();
        Shuffle(game_sprites);
        AddButtonEvent();
    }

    void AddSprite()
    {
        btn_list = GameObject.FindGameObjectsWithTag("Field");
        int index = 0;
        for(int i=0; i<btn_list.Length; i++)
        {
            if (i == btn_list.Length/2)
            {
                index = 0;
            }
            game_sprites.Add(backgrounds[index]);
            Debug.Log("game_sprites length " + game_sprites.Count);
            index++;
        }
        //Debug.Log("game_sprites size: " + game_sprites.Count);
    }
	
	// Update is called once per frame
	void Update () {
        StartCoroutine(reduceTime());
        int time = int.Parse(timeText.text);
        if (time <= 0)
        {
            SceneManager.LoadScene("game_over");
        }
    }

    IEnumerator reduceTime()
    {
        int time = int.Parse(timeText.text);
        yield return new WaitForSeconds(1);
        time--;
        timeText.text = time.ToString();
    }

    private void AddButtonEvent()
    {
        foreach(GameObject btn in btn_list)
        {
            btn.GetComponent<Button>().onClick.AddListener(() => buttonClick());
        }
    }

    public void buttonClick()
    {
        
        Debug.Log(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);
        Debug.Log("firstGuess " + firstGuess + " secondGuess " + secondGuess);
        
        if (!firstGuess)
        {
            firstGuess = true;
            firstIndex = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);
            Debug.Log("firstIndex " + firstIndex);
            Debug.Log("game_sprites length " + game_sprites.Count);
            firstName = game_sprites[firstIndex].name;
            Debug.Log("1st index: " + firstIndex + " 1st name: " + firstName);
            btn_list[firstIndex].GetComponent<Image>().sprite = game_sprites[firstIndex];
            
        }
        else if(!secondGuess)
        {
            secondGuess = true;
            secondIndex = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);
            Debug.Log("secondIndex " + secondIndex);
            secondName = game_sprites[secondIndex].name;
            Debug.Log("2nd index: " + secondIndex + " 2nd name: " + secondName);
            btn_list[secondIndex].GetComponent<Image>().sprite = game_sprites[secondIndex];
            StartCoroutine(CheckPuzzleMatched());
        }
    }

    IEnumerator CheckPuzzleMatched()
    {
        yield return new WaitForSeconds(1);
        if(firstName == secondName && firstIndex != secondIndex)
        {
            GameObject.Destroy(btn_list[firstIndex]);
            GameObject.Destroy(btn_list[secondIndex]);
            correctCount++;
            CheckIfFinishedPuzzle();
        }
        else
        {
            btn_list[firstIndex].GetComponent<Image>().sprite = main_bg;
            btn_list[secondIndex].GetComponent<Image>().sprite = main_bg;
        }
        firstGuess = secondGuess = false;
    }

    void Shuffle(List<Sprite> list)
    {
        Sprite temp;
        for(int i=0; i<list.Count; i++)
        {
            temp = list[i];
            int random = Random.Range(0, list.Count);
            list[i] = list[random];
            list[random] = temp;
        }
    }
    
    void CheckIfFinishedPuzzle()
    {
        if(correctCount == stages[current_stage] / 2)
        {
            current_stage++;
            Debug.Log("Next stage: " + current_stage);
            if (current_stage < 3)
            {
                SceneManager.LoadScene("game_stage_" + (current_stage+1));
            }
            else
            {
                SceneManager.LoadScene("winner_screen");
            }
        }
    }
}
