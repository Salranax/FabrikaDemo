using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject spawnPoint; 

    // Start is called before the first frame update
    void Start()
    {
        if(instance == null){
            instance = this;
        }

    }


}
