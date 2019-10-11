using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingScript : MonoBehaviour
{
    public void testfindSmallest(){
        findSmallest(new Vector2(Random.Range(-10.0f,10.0f), Random.Range(-10.0f,10.0f)), new Vector2(Random.Range(-10.0f,10.0f), Random.Range(-10.0f,10.0f)));
    }
    
    private void findSmallest(Vector2 a, Vector2 b){
        Debug.Log("First One: " + a + " Second One: " + b + " Smallest One: " + (a.y > b.y ? b : a));
	}
}
