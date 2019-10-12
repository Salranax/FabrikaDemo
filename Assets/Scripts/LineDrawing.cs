using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LineDrawing : MonoBehaviour
{
	public GameObject linePrefab;
	[HideInInspector]
	public Line currentLine;
	public Transform lineParent;
	public static LineDrawing instance;
	public bool isStarted;
    private bool isMouseOver = false;

	void Awake ()
	{
		if (instance == null) {
			instance = this;
		} else {
			Destroy (gameObject);
		}
	}

	void Start ()
	{
		if (lineParent == null) {
			lineParent = GameObject.Find ("Lines").transform;
		}
	}
	
	void Update ()
	{
		if (!isStarted) {
			return;
		}

		if(Input.GetMouseButtonDown(0)){
			Debug.Log(isMouseOverDrawTable());
		}
		if (Input.GetMouseButtonDown (0) && isMouseOverDrawTable()) {
			CreateNewLine ();
		}
        else if(!isMouseOverDrawTable() && currentLine != null){
            EnableLine();
        } 
        else if (Input.GetMouseButtonUp (0) && isMouseOverDrawTable()) {
			EnableLine();
		}

		if (currentLine != null) {
			//currentLine.AddPoint (Camera.main.ScreenToWorldPoint (new Vector3(Input.mousePosition.x,Input.mousePosition.y - 200, 0)));
			currentLine.AddPoint (GetWorldPositionOnPlane(Input.mousePosition, 0));

			if (currentLine.ReachedPointsLimit ()) {
				EnableLine();
			}
		}
	}

	 public Vector3 GetWorldPositionOnPlane(Vector3 screenPosition, float z) {
		Ray ray = Camera.main.ScreenPointToRay(screenPosition);
		Plane xy = new Plane(Vector3.forward, new Vector3(0, 0, z));
		float distance;
		xy.Raycast(ray, out distance);
		return ray.GetPoint(distance);
 	}
    private bool isMouseOverDrawTable(){
        return EventSystem.current.IsPointerOverGameObject(0);
    }

    //Line Generation
	private void CreateNewLine ()
	{
		currentLine = (Instantiate (linePrefab, Vector3.zero, Quaternion.identity) as GameObject).GetComponent<Line> ();
		currentLine.name = "Line";
		currentLine.transform.SetParent (lineParent);

	}

    //Enables Line functions
	private void EnableLine ()
	{  	 
        CameraFollow.instance.setTarget(currentLine.gameObject);
		currentLine.generateCar();

        currentLine = null;
	}

}
