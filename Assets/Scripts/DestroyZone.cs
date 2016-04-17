using UnityEngine;
using System.Collections;

public class DestroyZone : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);
    }
}
