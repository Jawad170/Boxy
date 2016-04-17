using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UpdateResourceCount : MonoBehaviour {

    public string ResourceName = "NONE";

	// Use this for initialization
	void Start ()
    {
        GetComponent<Text>().text = PlayerPrefs.GetInt(ResourceName).ToString();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if ( ResourceName.StartsWith("str") )
            GetComponent<Text>().text = PlayerPrefs.GetString(ResourceName);
        else
            GetComponent<Text>().text = PlayerPrefs.GetInt(ResourceName).ToString();
    }
}
