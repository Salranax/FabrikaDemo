using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Line : MonoBehaviour
{	
	public GameObject wheelPrefab;
	public GameObject WheelModelPrefab;
	[HideInInspector]
	Mesh mesh;
	public bool isObjectActivated = false;
	public List<Vector2> points;
	private List<Vector2> polygon2DPoints;
	private LineRenderer lineRenderer;
	private PolygonCollider2D polygonCollider2D;
	private Rigidbody rigidBody;
	public Material lineMaterial;
	public float pointZPosition = -3;
	private float pointMinOffset = 0.05f;
	private static Vector2 tempVector;
	private static Vector2 direction;
	private static float angle;
	private static float halfWidth;
	public bool autoAddColliderPoint = true;
	
	//Point Management
	private Vector2[] wheelPoints = new Vector2[2];
	private Vector2 RespawnCoordinate;
	WheelCollider wheelColliderOne;
	WheelCollider wheelColliderTwo;
	GameObject wheelObjectOne;
	GameObject wheelObjectTwo;

	//Booster Variables
	private float boosterTime;
	private bool isBoostActive = false;

	private GameManager _gm;
	//

	[Range(0,5000)]
	public float maxPoints = Mathf.Infinity;

	void Awake ()
	{
		_gm = GameManager.instance;

		points = new List<Vector2> ();
		polygon2DPoints = new List<Vector2> ();
		lineRenderer = GetComponent<LineRenderer> ();
		polygonCollider2D = GetComponent<PolygonCollider2D> ();
		rigidBody = GetComponent<Rigidbody> ();

		if (lineMaterial == null) {
			//Create the material of the line
			lineMaterial = new Material (Shader.Find ("Sprites/Default"));
		}

		lineRenderer.material = lineMaterial;
		halfWidth = lineRenderer.endWidth / 2.0f;
	}

	void FixedUpdate() {
		if(wheelObjectTwo != null){
			UpdateWheelPoses();
		}
	}

	private void Update() {
		if(isObjectActivated && transform.localPosition.y < -5){
			teleportToSpawnPoint();
		}

		if(isBoostActive){
			boosterTime -= Time.deltaTime;
			
			if(boosterTime <= 0){
				isBoostActive = false;
				wheelColliderOne.motorTorque -= 10;
				wheelColliderTwo.motorTorque -= 10;
				boosterTime = 0;
			}
		}
	}

	private void UpdateWheelPoses(){
		UpdateWheelPose(wheelColliderOne, wheelObjectOne.transform);
		UpdateWheelPose(wheelColliderTwo, wheelObjectTwo.transform);
	}

	private void UpdateWheelPose(WheelCollider _collider, Transform _transform)
	{
		Vector3 _pos = _transform.position;
		Quaternion _quat = _transform.rotation;

		_collider.GetWorldPose(out _pos, out _quat);

		_transform.position = _pos;
		_transform.rotation = _quat;
	}

	public void generateCar(){
		transformPointsToZero();
		mesh = new Mesh();
		lineRenderer.BakeMesh(mesh, true);
		GetComponent<MeshCollider>().sharedMesh = mesh;
		GetComponent<MeshFilter>().mesh = mesh;

		placeWheels();
		RespawnCoordinate = findMidPoint();

		newDrawnSpawn();

		GetComponent<MeshCollider>().convex = true;

		MeshCollider colMesh = gameObject.AddComponent<MeshCollider>();
		colMesh.sharedMesh = mesh;
		colMesh.convex = true;
		colMesh.isTrigger = true;

		_gm.setLine(this);

		rigidBody.isKinematic = false;
		
		isObjectActivated = true;
	}

	public Vector2[] findWheelPoints(){

		Vector2 pointOne = new Vector2();
		Vector2 pointTwo = new Vector2();
		int count = 0;

		foreach (Vector2 item in points)
		{		
			//For first two iterations
			if(count == 0){
				pointOne = item;
			}
			else if(count == 1){
				pointTwo = item;
			}
			else if(pointOne.y > pointTwo.y){
				pointOne = findBiggest(pointOne,item);
			}
			else if(pointOne.y == pointTwo.y){ //if equality happens
				pointOne = findBiggest(pointOne,item);
			}

			count ++;
		}

		Vector2[] tmp = new Vector2[2]{pointOne, pointTwo};
		return tmp;
	}

	private void placeWheels(){
		wheelPoints = findWheelPoints();

		//Wheels İnstantiated
		GameObject wheelOne = Instantiate(wheelPrefab) as GameObject;
		GameObject wheelTwo = Instantiate(wheelPrefab) as GameObject;

		wheelOne.transform.SetParent(this.transform);
		wheelTwo.transform.SetParent(this.transform);

		wheelOne.transform.position = points[0];
		wheelTwo.transform.position = points[points.Count-1];

		wheelColliderOne = wheelOne.transform.GetChild(0).GetComponent<WheelCollider>();
		wheelColliderTwo = wheelTwo.transform.GetChild(0).GetComponent<WheelCollider>();

		wheelColliderOne.steerAngle = 90;
		wheelColliderTwo.steerAngle = 90;
		wheelColliderOne.motorTorque = 10;
		wheelColliderTwo.motorTorque = 10;

		wheelObjectOne = Instantiate(WheelModelPrefab) as GameObject;
		wheelObjectTwo = Instantiate(WheelModelPrefab) as GameObject;

		wheelObjectOne.transform.SetParent(this.transform);
		wheelObjectTwo.transform.SetParent(this.transform);

		wheelObjectOne.transform.position = points[0];
		wheelObjectTwo.transform.position = points[points.Count-1];

	}

	public Vector2 findMidPoint(){
		float midX;
		float midY;

		if(wheelPoints != null){
			midX = (wheelPoints[0].x + wheelPoints[0].x) / 2f;
			midY = (wheelPoints[0].y + wheelPoints[0].y) / 2f;
		}
		else{ // Just a backup
			midX = points[points.Count - 1].x;
			midY = points[points.Count - 1].y;
		}

		return new Vector2(midX, midY);
	}

	private Vector2 findBiggest(Vector2 a, Vector2 b){
		return a.y > b.y ? a : b;
	}

	public void AddPoint (Vector3 point)
	{
		//If the given point already exists ,then skip it
		if (points.Contains (point)) {
			return;
		}

		if (points.Count > 1) {
			if (Vector2.Distance (point, points [points.Count - 1]) < pointMinOffset) {
				return;//skip the point
			}
		}

		//z-position of the point
		point.z = pointZPosition;

		//Add the point to the points list
		points.Add (point);
		lineRenderer.positionCount++;
		lineRenderer.SetPosition (lineRenderer.positionCount - 1, point);
	}

	public bool ReachedPointsLimit(){
		return points.Count >= maxPoints;
	}

	public void teleportToSpawnPoint(){
		transform.position = new Vector2(_gm.spawnPoint.transform.position.x, (_gm.spawnPoint.transform.position.y < 2 ? 2.7f : _gm.spawnPoint.transform.position.y));
		GetComponent<Rigidbody>().velocity = Vector3.zero;
		GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
		transform.eulerAngles = new Vector3(0,0,0);
		Debug.Log(transform.position);
	}

	public void newDrawnSpawn(){
		GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
		if(!_gm.getActiveLineStatus()){
			transform.position = new Vector2(_gm.spawnPoint.transform.position.x, (_gm.spawnPoint.transform.position.y < 2 ? 2.7f : _gm.spawnPoint.transform.position.y));
		}
		else{
			transform.position = _gm.getLine().getSpawnCoordinate();
		}
		Debug.Log(transform.position);
	}

	public Vector3 getSpawnCoordinate(){
		return new Vector3(transform.position.x, transform.position.y < 2 ? 2.7f : transform.position.y, transform.position.z);
	}

	public void destroyGameObject(){
		Destroy(this.gameObject);
	}

	public void setBoost(){
		wheelColliderOne.motorTorque += 10;
		wheelColliderTwo.motorTorque += 10;

		isBoostActive = true;
		boosterTime = 2f;

	}

	void OnTriggerEnter(Collider other)
	{
		Debug.Log(other);
		if(other.gameObject.name.Contains("Booster")){
			Debug.Log("Collided");
		}
	}

	public void endTheGame(){
		rigidBody.isKinematic = true;
		UIManager.instance.endGame();
	}

	private void transformPointsToZero(){
		List<Vector2> tmp = new List<Vector2>(points.Count);
		Vector2 vectrTmp = new Vector2(Mathf.Floor(points[points.Count - 1].x), Mathf.Floor(points[points.Count - 1].y));

		int count = 0;

		foreach (Vector2 item in points)
		{
			if(points[count].x > 0){
				if(points[count].y > 0){
					lineRenderer.SetPosition(count,new Vector2(points[count].x - Mathf.Abs(vectrTmp.x), points[count].y - Mathf.Abs(vectrTmp.y)));
					tmp.Add(new Vector2(points[count].x - Mathf.Abs(vectrTmp.x), points[count].y - Mathf.Abs(vectrTmp.y)));
				}
				else{
					lineRenderer.SetPosition(count,new Vector2(points[count].x - Mathf.Abs(vectrTmp.x), points[count].y + Mathf.Abs(vectrTmp.y)));
					tmp.Add(new Vector2(points[count].x - Mathf.Abs(vectrTmp.x), points[count].y + Mathf.Abs(vectrTmp.y)));
				}

			}
			else{
				lineRenderer.SetPosition(count,new Vector2(points[count].x + Mathf.Abs(vectrTmp.x), points[count].y + Mathf.Abs(vectrTmp.y)));
				tmp.Add(new Vector2(points[count].x + Mathf.Abs(vectrTmp.x), points[count].y + Mathf.Abs(vectrTmp.y)));
			}
			count++;
		}

		points = tmp;
	}
}
