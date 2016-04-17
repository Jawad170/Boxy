using UnityEngine;
using System.Collections;

public class SpawnBoxy : MonoBehaviour {

    public GameObject BoxyPrefab;
    public int GenerationFrequency = 2;

    public bool SpawnOnlyOnce = true;

    void Start()
    {
        if (SpawnOnlyOnce) SpawnOne();
        else StartCoroutine(GenerateBoxy());
    }

    IEnumerator GenerateBoxy()
    {
        
        float PercentageGold     = 2.0f;
        float PercentageObsidian = 0.5f;
        GameObject newBoxy;

        newBoxy = Instantiate(BoxyPrefab, transform.position, transform.rotation) as GameObject;

        int TypeRoll = Random.Range(0, 1000);

        int GoldAdj = 1000 - ( (int)PercentageGold     * 10 );
        int ObsiAdj = 1000 - ( (int)PercentageObsidian * 10 );

        if (TypeRoll < GoldAdj)
        {
            newBoxy.GetComponent<BoxyDetails>().PaintJob(1);
        }
        else if (TypeRoll < ObsiAdj)
        {
            newBoxy.GetComponent<BoxyDetails>().PaintJob(2);
        }
        else
        {
            newBoxy.GetComponent<BoxyDetails>().PaintJob(3);
        }
            
        yield return new WaitForSeconds(GenerationFrequency);
        StartCoroutine(GenerateBoxy());
    }

    void SpawnOne()
    {
        GameObject newBoxy;

        newBoxy = Instantiate(BoxyPrefab, transform.position, transform.rotation) as GameObject;

        int TypeRoll = Random.Range(0, 100);

        if (TypeRoll < 97)
        {
            newBoxy.GetComponent<BoxyDetails>().PaintJob(1);
        }
        else if (TypeRoll < 99)
        {
            newBoxy.GetComponent<BoxyDetails>().PaintJob(2);
        }
        else
        {
            newBoxy.GetComponent<BoxyDetails>().PaintJob(3);
        }
    }
}
