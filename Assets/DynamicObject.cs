using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicObject : MonoBehaviour
{
    public float movementRange = 2.0f;
    public float movementSpeed = 0.2f;
    [HideInInspector]
    public Vector3 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = new Vector3(transform.position.x, transform.position.y - (movementSpeed * Time.deltaTime), transform.position.z);

        if(transform.position.y <= startPosition.y - movementRange || transform.position.y > startPosition.y + movementRange){
            movementSpeed *= -1;
        }
    }
}
