﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 2D Polygon Line Collider Package
 *
 * @license		    Unity Asset Store EULA https://unity3d.com/legal/as_terms
 * @author		    Indie Studio - Baraa Nasser
 * @Website		    https://indiestd.com
 * @Asset Store     https://www.assetstore.unity3d.com/en/#!/publisher/9268
 * @Unity Connect   https://connect.unity.com/u/5822191d090915001dbaf653/column
 * @email		    info@indiestd.com
 *
 */


[RequireComponent(typeof(LineRenderer))]
public class Line : MonoBehaviour
{	
	public GameObject wheelPrefab;
	[HideInInspector]
	Mesh mesh;

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
	//

	[Range(0,5000)]
	public float maxPoints = Mathf.Infinity;

	void Awake ()
	{
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

	public void generateCar(){
		//MeshGenerator.instance.setPoints(points);

		placeWheels();
		RespawnCoordinate = findMidPoint();

		transform.position = new Vector2(GameManager.instance.spawnPoint.transform.position.x - RespawnCoordinate.x, GameManager.instance.spawnPoint.transform.position.y - RespawnCoordinate.y);

		mesh = new Mesh();
		lineRenderer.BakeMesh(mesh, true);
		GetComponent<MeshCollider>().sharedMesh = mesh;
		GetComponent<MeshFilter>().mesh = mesh;

		rigidBody.isKinematic = false;
		GetComponent<MeshCollider>().convex = true;
		//TODO: Add Wheels, spawn the car
        //Find Its mid point
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
		Debug.Log(tmp[0] + " " + tmp[1]);
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

		// if (autoAddColliderPoint) {
		// 	//Add the point to the collider of the line
		// 	AddPointToCollider (points.Count - 1);
		// }
	}

	public bool ReachedPointsLimit(){
		return points.Count >= maxPoints;
	}

	// public void AddPointToCollider (int index)
	// {
	// 	direction = points [index] - points [index + 1 < points.Count ? index + 1 : (index - 1 >= 0 ? index - 1 : index)];
	// 	angle = Mathf.Atan2 (direction.x, -direction.y);

	// 	tempVector = points [index];
	// 	tempVector.x = tempVector.x + halfWidth * Mathf.Cos (angle);
	// 	tempVector.y = tempVector.y + halfWidth * Mathf.Sin (angle);
	// 	polygon2DPoints.Insert (polygon2DPoints.Count, tempVector);

	// 	tempVector = points [index];
	// 	tempVector.x = tempVector.x - halfWidth * Mathf.Cos (angle);
	// 	tempVector.y = tempVector.y - halfWidth * Mathf.Sin (angle);
	// 	polygon2DPoints.Insert (0, tempVector);

	// 	polygonCollider2D.points = polygon2DPoints.ToArray ();
	// }
}