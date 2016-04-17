using UnityEngine;
using System.Collections;

public class MiniFade : MonoBehaviour {

    [SerializeField]
    private float fadePerSecond = 2.5f;

    private void Update()
    {
        if ( transform.root.GetComponent<BoxClick>().Clicked )
        {
            Material material = GetComponent<Renderer>().material;
            Color color = material.color;

            material.color = new Color(color.r, color.g, color.b, color.a - (fadePerSecond * Time.deltaTime));
            if (material.color.a < 0.05) Destroy(gameObject);
        }
    }
}
