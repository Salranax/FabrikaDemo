using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        GameManager.instance.setSpawnPoint(this.gameObject);
    }
}
