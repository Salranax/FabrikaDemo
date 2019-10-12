using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBooster : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        other.GetComponent<Line>().setBoost();
        Destroy(this.gameObject);
    }
}
