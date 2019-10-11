using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow instance;
	// Distance in the x axis the target can move before the camera follows.
	public float xMargin = 1f;
	// Distance in the y axis the target can move before the camera follows.
	public float yMargin = 1f;
	// How smoothly the camera catches up with it's target movement in the x axis.
	public float xSmooth = 8f;
	// How smoothly the camera catches up with it's target movement in the y axis.
	public float ySmooth = 8f;
	// Reference to the target's transform.
	public Transform target;
	// Reference to the target's x postion.
	private float targetX;
	// Reference to the target's y postion.
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
		
		transform.position = new Vector3 (targetX, transform.position.y, transform.position.z);
	}

    public void setTarget(GameObject t){
        target = t.transform;
    }
}
