using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject spawnPoint; 
    public Line activeLine;

    // Start is called before the first frame update
    void Start()
    {
        if(instance == null){
            instance = this;
        }
    }

    public void setSpawnPoint(GameObject obj){
        spawnPoint = obj;
    }

    public void setLine(Line crt){
        if(activeLine != null){
            activeLine.destroyGameObject();
        }
        activeLine = crt;
    }

    public Line getLine(){
        return activeLine;
    }

    public bool getActiveLineStatus(){
        if(activeLine == null){
            return false;
        }
        else{
            return true;
        }
    }
}
