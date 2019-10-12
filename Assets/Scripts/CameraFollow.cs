using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow instance;
	public float xMargin = 1f;
	public float yMargin = 1f;
	public float xSmooth = 8f;
	public float ySmooth = 8f;
	public Transform target;
	private float targetX;
	private float targetY;

	void Start ()
	{
        if(instance == null){
            instance = this;
        }
	}

	void Update ()
	{
		trackTarget ();
	}

	void trackTarget ()
	{
		if (target == null) {
			return;
		}

		targetX = transform.position.x;
		targetY = transform.position.y;


		targetX = Mathf.Lerp (transform.position.x, target.position.x - xMargin, xSmooth * Time.deltaTime);
		targetY = Mathf.Lerp (transform.position.y + 0.3f, target.position.y + 0.3f - xMargin, xSmooth * Time.deltaTime);
		
		transform.position = new Vector3 (targetX, targetY, transform.position.z);
	}

    public void setTarget(GameObject t){
        target = t.transform;
    }
}
