using UnityEngine;
using System.Collections;

public class BoxyMovement : MonoBehaviour {

    public float speed = 1.0f;  
	

	void Update () 
    {
        transform.Translate(new Vector3(0.0f, speed, 0.0f) * Time.deltaTime);
	}
}
