using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

    public GameObject BoxyPrefab;
    public int GenerationFrequency = 2;

    void Start()
    {
        StartCoroutine(GenerateBoxy());
    }

    IEnumerator GenerateBoxy()
    {
        GameObject newBoxy;

        float x_pos = -11f;
        float z_pos = 5.0f;

        for (int i = 0; i < 10; i++)
        {
            //newBoxy = Instantiate(BoxyPrefab, new Vector3(x_pos, 0.0f, z_pos), Quaternion.identity) as GameObject;
            x_pos += 1.2f;
            z_pos += 0.7f;
        }

        yield return new WaitForSeconds(GenerationFrequency);
        StartCoroutine(GenerateBoxy());
    }
}
