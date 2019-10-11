using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    public static MeshGenerator instance;
    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;

    void Start() {
        if(instance == null){
            instance = this;
        }

        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        createShape();

    }

    void createShape(){
        updateMesh();
    }

    void updateMesh(){
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
    }

    public void setPoints(List<Vector2> a){
        vertices = new Vector3[a.Count];
        int counter = 0;
        int i = 0;
        foreach (Vector2 item in a)
        {
            //vertices[i] = new Vector3(a[i].x );
            counter += 6;
            i++;
        }
    }

    // void OnDrawGizmos()
    // {
    //     if(vertices == null){
    //         return;
    //     }

    //     for (int i = 0; i < vertices.Length; i++)
    //     {
    //         Gizmos.DrawSphere(vertices[i], .1f);
    //     }
    // }
}
