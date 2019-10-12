using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{   
    public static UIManager instance;
    public GameObject drawArea;
    public GameObject endScreen;

    // Start is called before the first frame update
    void Start()
    {   
        if(instance == null){
            instance = this;
        }
    }

    public void endGame(){
        endScreen.SetActive(true);
        drawArea.SetActive(false);
        
    }

    public void reloadLevel(){
        SceneManager.LoadScene(0);
    }

}
