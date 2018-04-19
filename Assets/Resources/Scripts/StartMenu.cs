using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour {

    Button btn_play;
    Dropdown drop_district;
    InputField input_name, input_phone;
    Text error_text;
    GameObject loading;

    private void Awake()
    {
        #if UNITY_5 && UNITY_IOS
                ChangeXcodePlist.ChangeWifiPlist(UnityEditor.BuildTarget.iOS, Application.dataPath);
        #endif
    }///Năm sinh

    // Use this for initialization
    void Start () {
        btn_play = GameObject.Find("btnPlay").GetComponent<Button>();
        input_name = GameObject.Find("InputName").GetComponent<InputField>();
        input_phone = GameObject.Find("InputPhone").GetComponent<InputField>();
        error_text = GameObject.Find("ErrorText").GetComponent<Text>();
        GameObject form_wrapper = GameObject.Find("FormMove");
        Vector3 screenPosition = new Vector3(1, 1, 1);
        form_wrapper.transform.position = Camera.main.GetComponent<Camera>().ScreenToWorldPoint(screenPosition);
        loading = GameObject.Find("Loading");
        loading.SetActive(false);
        btn_play.onClick.AddListener(() => PlayGame());
        drop_district = GameObject.Find("Dropdown").GetComponent<Dropdown>();
        setDropdown();
    }

    void setDropdown()
    {
        List<string> options = new List<string>();
        for(int i=1990; i<2019; i++)
        {
            options.Add(i.ToString()); 
        }
        drop_district.ClearOptions();
        drop_district.AddOptions(options);
    }

    void validateData(string name, string phone)
    {
        if (name.Trim().Equals(""))
        {
            error_text.text = "*Bạn phải nhập tên";
        }
        else if (phone.Trim().Equals(""))
        {
            error_text.text = "*Bạn phải nhập số điện thoại";
        }
        else
        {
            error_text.text = "";
            StartCoroutine( sendData(name, phone) );
        }
    }

     IEnumerator sendData(string name, string phone)
    {
        name = name.Replace(" ", "%20");
        phone = phone.Replace(" ", "%20");
        string current_date = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        current_date = current_date.Replace(" ", "%20");
        string district = drop_district.options[drop_district.value].text.Replace(" ", "%20");
        string getUrl = "http://api-booking.nkscom.webstarterz.com/newEntry.php?tmpFullname=aName&tmpDob=aDob&tmpPhone=aPhone&tmpDate=aDate";
        
        getUrl = getUrl.Replace("aName", name);
        getUrl = getUrl.Replace("aPhone", phone);
        getUrl = getUrl.Replace("aDob", district);
        getUrl = getUrl.Replace("aDate", current_date);
        Debug.Log(getUrl);
        WWW getStatus = new WWW(getUrl);
        var sec = 0f;
        loading.SetActive(true);
        while (!getStatus.isDone && sec < 10f)
        {
            sec += Time.deltaTime;
            Debug.Log(getStatus.isDone + " Caching: " + Caching.ready);

            yield return getStatus;
            loading.SetActive(false);
            if (getStatus.text.Contains("Success"))
            {
                SceneManager.LoadScene("game_stage_1");
            }
            else
            {
                error_text.text = getStatus.text;
            }
            
        }
        if (sec >= 10f)
        {
            error_text.text = "Lỗi kết nối. Thử lại";
        }

        yield return getStatus;
        loading.SetActive(false);
        Debug.Log(getStatus.text);
        if (getStatus.text.Contains("Success"))
        {
            SceneManager.LoadScene("game_stage_1");
        }
        else
        {
            error_text.text = getStatus.text;
        }

    }

    void PlayGame()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            error_text.text = "Vui lòng kết nối internet và thử lại!";
        }
        else
        {
            string name = input_name.text;
            string phone = input_phone.text;
            validateData(name, phone);
            // SceneManager.LoadScene("game_stage_1");
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
