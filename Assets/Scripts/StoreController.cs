using UnityEngine;
using System.Collections;

public class StoreController : MonoBehaviour {

    public Camera MainCamera;
    public float speed = 15.0f;

    [Header("UI Elements to hide")]
    public GameObject UI_Element1;
    public GameObject UI_Element2;
    public GameObject UI_Element3;
    public GameObject UI_Element4;

    private int MovedRight = 0;

	void Start ()
    {

        PlayerPrefs.SetString("strSTORE_TOTAL_KEYS",  "Total Keys: "  + PlayerPrefs.GetInt("KEYS").ToString());
        PlayerPrefs.SetString("strSTORE_TOTAL_COINS", "Total Coins: " + PlayerPrefs.GetInt("COINS").ToString());
    }

    void Update()
    {   
        if ( MovedRight > 0 )
        {
            UI_Element1.SetActive(false);
            UI_Element2.SetActive(false);
            UI_Element3.SetActive(false);
            UI_Element4.SetActive(false);
        }
        else
        {
            UI_Element1.SetActive(true);
            UI_Element2.SetActive(true);
            UI_Element3.SetActive(true);
            UI_Element4.SetActive(true);
        }
    }


    void BuyKey()
    {
        if (PlayerPrefs.GetInt("COINS") > 10)
        {
            PlayerPrefs.SetInt("KEYS", PlayerPrefs.GetInt("KEYS") + 1);
            PlayerPrefs.SetString("strSTORE_TOTAL_KEYS", "Total Keys: " + PlayerPrefs.GetInt("KEYS").ToString());

            PlayerPrefs.SetInt("COINS", PlayerPrefs.GetInt("COINS") - 10);
            PlayerPrefs.SetString("strSTORE_TOTAL_COINS", "Total Coins: " + PlayerPrefs.GetInt("COINS").ToString());
        }

    }

    void MoveRight()
    {
        MovedRight++;
        float start_position = MainCamera.transform.position.x;
        float end_position   = start_position += 20;

        while (MainCamera.transform.position.x < end_position)
        {
            Strafe(-speed * Time.deltaTime);
        }
    }

    void MoveLeft()
    {
        if (MovedRight > 0)
        {
            MovedRight--;
            float start_position = MainCamera.transform.position.x;
            float end_position = start_position -= 20;

            while (MainCamera.transform.position.x > end_position)
            {
                Strafe(+speed * Time.deltaTime);
            }
        }
    }
    
    void Strafe(float dist)
    {
       MainCamera.transform.Translate(Vector3.left * dist);
    }


    void StartGame()
    {
        PlayerPrefs.SetString("strREWARD", "");
        Application.LoadLevel("Level_01");
    }


}
