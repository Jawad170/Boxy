using UnityEngine;
using System.Collections;

public class BoxyDetails : MonoBehaviour
{

    public int BoxyType = -1;

    //Call whenever a Boxy is created to set it's type and all other attributes likes materials.
    public void PaintJob(int x)
    {

        BoxyType = x;

        Renderer cubeBottom = GetComponentsInChildren<MeshRenderer>()[1];//.GetComponent<MeshRenderer>();
        Renderer cubeTop    = GetComponentsInChildren<Transform>()[0].GetComponentsInChildren<MeshRenderer>()[0];

        switch (BoxyType)
        {
            case 0: cubeBottom.material = Resources.Load(@"Art/Materials/FADE_ME")      as Material;
                    cubeTop.material    = Resources.Load(@"Art/Materials/FADE_ME")      as Material; break;
            case 1: cubeBottom.material = Resources.Load(@"Art/Materials/White_01")     as Material; 
                    cubeTop.material    = Resources.Load(@"Art/Materials/White_01")     as Material; break;
            case 2: cubeBottom.material = Resources.Load(@"Art/Materials/Gold_01")      as Material;
                    cubeTop.material    = Resources.Load(@"Art/Materials/Gold_01")      as Material; break;
            case 3: cubeBottom.material = Resources.Load(@"Art/Materials/Obsidian_01")  as Material;
                    cubeTop.material    = Resources.Load(@"Art/Materials/Obsidian_01")  as Material; break;

            case 101: cubeBottom.material   = Resources.Load(@"Art/Materials/White_01_Fade")     as Material;
                      cubeTop.material      = Resources.Load(@"Art/Materials/White_01_Fade")     as Material; break;
            case 102: cubeBottom.material   = Resources.Load(@"Art/Materials/Gold_01_Fade")      as Material;
                      cubeTop.material      = Resources.Load(@"Art/Materials/Gold_01_Fade")      as Material; break;
            case 103: cubeBottom.material   = Resources.Load(@"Art/Materials/Obsidian_01_Fade")  as Material;
                      cubeTop.material      = Resources.Load(@"Art/Materials/Obsidian_01_Fade")  as Material; break;

            default: Debug.Log("ERROR: Couldn't set Boxy material"); break;
        }
    }


    int Rewarded_Coins  = 0;

    //Call when box is clicked to generate a reward based on the Boxy type
    public void GenerateReward()
    {

        if (BoxyType == -1)
        {
            Debug.Log("GENERATE_REWARD: Failed Reward Generation. Boxy type NOT set.");
        }
        else if (BoxyType == 1)
        {
            Rewarded_Coins = 1; //Random.Range(1, 3);
        }
        else if (BoxyType == 2)
        {
            Rewarded_Coins = 5; //Random.Range(2, 6);
        }
        else if (BoxyType == 3)
        {
            Rewarded_Coins = 10; //Random.Range(3, 8);
        }
        else
        {
            Debug.Log("GENERATE_REWARD: Failed, no reward set for current Boxy Type [ " + BoxyType + " ]");
        }

        PropegateRewards();
    }

    void PropegateRewards()
    {
        PlayerPrefs.SetInt("COINS", PlayerPrefs.GetInt("COINS") + Rewarded_Coins)   ;

        string RewardText = " ";
        if (Rewarded_Coins > 0) RewardText += "+" + Rewarded_Coins;

        PlayerPrefs.SetString("strREWARD", RewardText);

        Rewarded_Coins  = 0;
    }
}
