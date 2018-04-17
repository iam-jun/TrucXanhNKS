using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour {

    Button btn_play;
    Dropdown drop_district;
    InputField input_name, input_phone, input_add;
    Text error_text;
    GameObject loading;

	// Use this for initialization
	void Start () {
        btn_play = GameObject.Find("btnPlay").GetComponent<Button>();
        input_name = GameObject.Find("InputName").GetComponent<InputField>();
        input_phone = GameObject.Find("InputPhone").GetComponent<InputField>();
        input_add = GameObject.Find("InputAdd").GetComponent<InputField>();
        error_text = GameObject.Find("ErrorText").GetComponent<Text>();
        loading = GameObject.Find("Loading");
        loading.SetActive(false);
        btn_play.onClick.AddListener(() => PlayGame());
        drop_district = GameObject.Find("Dropdown").GetComponent<Dropdown>();
        setDropdown();
    }

    void setDropdown()
    {
        string[] array = new string[] {"Quận 1", "Quận 2", "Quận 3","Quận 4", "Quận 5", "Quận 6","Quận 7","Quận 8","Quận 9",
                              "Quận 10","Quận 11","Quận 12","Quận Gò Vấp","Quận Bình Thạnh","Quận Tân Bình","Quận Tân Phú",
                              "Quận Phú Nhuận","Quận Bình Tân","Quận Thủ Đức","Huyện Củ Chi","Huyện Hóc Môn","Huyện Bình Chánh",
                              "Huyện Nhà Bè","Huyện Cần Giờ"};
        List<string> options = new List<string>();
        foreach (var option in array)
        {
            options.Add(option); // Or whatever you want for a label
        }
        drop_district.ClearOptions();
        drop_district.AddOptions(options);
    }

    void validateData(string name, string phone, string address)
    {
        if (name.Trim().Equals(""))
        {
            error_text.text = "*Bạn phải nhập tên";
        }
        else if (phone.Trim().Equals(""))
        {
            error_text.text = "*Bạn phải nhập số điện thoại";
        }
        else if (address.Trim().Equals(""))
        {
            error_text.text = "*Bạn phải nhập địa chỉ";
        }
        else
        {
            error_text.text = "";
            StartCoroutine( sendData(name, phone, address) );
        }
    }

     IEnumerator sendData(string name, string phone, string address)
    {
        name = name.Replace(" ", "%20");
        phone = phone.Replace(" ", "%20");
        address = address.Replace(" ", "%20");
        string current_date = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        current_date = current_date.Replace(" ", "%20");
        string district = drop_district.options[drop_district.value].text.Replace(" ", "%20");
        string getUrl = "http://api-booking.nkscom.webstarterz.com/newEntry.php?tmpFullname=aName&tmpPhone=aPhone&tmpAddress=aAddress&tmpDistrict=aDistrict&tmpDate=aDate";
        
        getUrl = getUrl.Replace("aName", name);
        getUrl = getUrl.Replace("aPhone", phone);
        getUrl = getUrl.Replace("aAddress", address);
        getUrl = getUrl.Replace("aDistrict", district);
        getUrl = getUrl.Replace("aDate", current_date);
        Debug.Log(getUrl);
        WWW getStatus = new WWW(getUrl);
        loading.SetActive(true);
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
            string address = input_add.text;
            validateData(name, phone, address);
            // SceneManager.LoadScene("game_stage_1");
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
