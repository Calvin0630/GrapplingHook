using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Trail : MonoBehaviour {

    Mesh trail;
    List<Vector3> trailPoints;
    public float width;
    Vector3[] meshVertices;
    Vector2[] meshUVs;
    int[] meshTriangles;

    // Use this for initialization
    void Start() {
        trailPoints = new List<Vector3>();
        trail = new Mesh();
        GetComponent<MeshFilter>().mesh = trail;
        /*
        trail.Clear();
        meshVertices = new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 1, 0), new Vector3(1, 1, 0), new Vector3(1, 0, 0) };
        meshUVs = new Vector2[] { Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero };
        meshTriangles = new int[] { 1, 2, 3, 1, 3, 0 };
        */
        trailPoints.Add(new Vector3(-5, 1, 0));
        trailPoints.Add(new Vector3(-4, -1, 0));
        trailPoints.Add(new Vector3(-3, 1, 0));
        trailPoints.Add(new Vector3(-2, -1, 0));
        trailPoints.Add(new Vector3(-1, 1, 0));
        trailPoints.Add(new Vector3(0, -1, 0));
        CreateMesh();
        print("Vertices: " + meshVertices.Length + "  meshTris: " + meshTriangles.Length);
    }

    // Update is called once per frame
    void Update() {
        CreateMesh();
    }

    void FixedUpdate() {
        
        trailPoints.Add(Vector3.zero);
        for (int i=0;i<trailPoints.Count;i++) {
            trailPoints[i] = trailPoints[i] + ObstacleSpawner.worldVelocity * Time.deltaTime;
        }
        while (trailPoints.Count > 50) {
            trailPoints.RemoveAt(0);
        }
        
    }

    void CreateMesh() {
        if (trailPoints.Count > 1) {
            trail.Clear();
            GetVertices();
            GetTris();
            GetUVs();
            trail.vertices = meshVertices;
            trail.uv = meshUVs;
            trail.triangles = meshTriangles;
        }
    }

    void GetVertices() {
        meshVertices = new Vector3[2 * trailPoints.Count];
        //iterates through trailpoints, and finds vertices
        for (int i = 0; i < trailPoints.Count; i++) {
            Vector3 trailNormal;
            // if it's at the start of the trailpoints
            if (i == 0) {
                trailNormal = (trailPoints[i] - trailPoints[i + 1]).normalized;
            }
            //if its at the end of the trail points
            else if (i == trailPoints.Count - 1) {
                trailNormal = (trailPoints[i - 1] - trailPoints[i]).normalized;
            }
            //otherwise it's somewhere in the middle
            else {
                trailNormal = ((trailPoints[i - 1] - trailPoints[i]) + (trailPoints[i + 1] - trailPoints[i])).normalized;
            }
            meshVertices[i * 2] = trailPoints[i] + width / 2 * trailNormal;
            meshVertices[i * 2 + 1] = trailPoints[i] - width / 2 * trailNormal;
        }
    }


    Vector3 Get2DNormal(Vector3 v) {
        return new Vector3(-v.y, v.x, 0);
    }
    void GetTris() {
        //there should be an error here
        meshTriangles = new int[(meshVertices.Length - 2) * 3];
        for (int i = 0; i < meshTriangles.Length/6; i ++) {
            meshTriangles[i * 6]     = i * 2 + 1;
            meshTriangles[i * 6 + 1] = i * 2 + 2;
            meshTriangles[i * 6 + 2] = i * 2 + 0;
            meshTriangles[i * 6 + 3] = i * 2 + 1;
            meshTriangles[i * 6 + 4] = i * 2 + 3;
            meshTriangles[i * 6 + 5] = i * 2 + 2;

        }
    }

    void GetUVs() {
        meshUVs = new Vector2[meshVertices.Length];
        for (int i = 0; i < meshUVs.Length; i++) {
            meshUVs[i] = Vector2.zero;
        }
    }
    // v1 and v2 are position vectors representing the first line, and v3 and v4 represent the second line
    Vector3 GetVectorIntersection(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4) {
        float a, b, c, d, x, y;
        a = (v2.y - v1.y) / (v2.x - v1.x);
        b = v1.y - a * v1.x;
        c = (v4.y - v3.y) / (v4.x - v3.x);
        d = v3.y - c * v3.x;
        x = (d - b) / (a - c);
        y = a * x + b;
        return new Vector3(x, y);
    }
}
