using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Gameplay : MonoBehaviour {

    [SerializeField]
    private Transform panel;
    [SerializeField]
    private GameObject button;
    List<Sprite> backgrounds;
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
    // Use this for initialization
    private void Awake()
    {
        Sprite[] _backgrounds = Resources.LoadAll<Sprite>("Images/game_asset");
        backgrounds = new List<Sprite>();
        for (int i = 6; i < 13; i++)
            backgrounds.Add(_backgrounds[i]);
        main_bg = _backgrounds[4];
    }

    void Start () {
        stages = new int[] {4, 8};
        game_sprites = new List<Sprite>();
        firstGuess = false;
        secondGuess = false;
        correctCount = 0;
        LoadStage(current_stage);
        
    }

    void LoadStage(int stage)
    {
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
        int length = backgrounds.Count;
        if (length > btn_list.Length / 2 && length % 2 != 0)
            length--;
        for (int i=0; i<btn_list.Length; i++)
        {
            if (i == btn_list.Length/2||index>=length)
            {
                index = 0;
            }
            Debug.Log("index " + index);
            game_sprites.Add(backgrounds[index]);
            Debug.Log("game_sprites length " + game_sprites.Count);
            index++;
        }
        //Debug.Log("game_sprites size: " + game_sprites.Count);
    }
	
	// Update is called once per frame
	void Update () {
        
        
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
            btn_list[firstIndex].GetComponent<Image>().color = Color.clear;
            btn_list[secondIndex].GetComponent<Image>().color = Color.clear;
            btn_list[firstIndex].GetComponent<Button>().interactable = false;
            btn_list[secondIndex].GetComponent<Button>().interactable = false;
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
            if (current_stage < 2)
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
