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
        trail.Clear();
        meshVertices = new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 1, 0), new Vector3(1, 1, 0), new Vector3(1, 0, 0) };
        meshUVs = new Vector2[] { Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero };
        meshTriangles = new int[] { 1, 2, 3, 1, 3, 0 };
    }

    // Update is called once per frame
    void Update() {
        CreateMesh();
    }

    void FixedUpdate() {
        trailPoints.Add(transform.position);
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
        meshVertices = new Vector3[GetNumOfVertices()];
        //iterates through trailpoints, and finds vertices
        for (int i = 0; i < trailPoints.Count; i++) {
            Vector3 lengthOfRectangle;
            if (i == 0) lengthOfRectangle = trailPoints[i] - transform.position;
            else lengthOfRectangle = trailPoints[i - 1] - trailPoints[i];
            Vector3 prevPoint;
            if (trailPoints.Count == 1 || i==0) prevPoint = transform.position;
            else prevPoint = trailPoints[i - 1];
            //adds perpendicular points
             meshVertices[i * 4] = trailPoints[i] + (width / 2) * Get2DNormal(lengthOfRectangle).normalized;
            meshVertices[i * 4 + 1] = trailPoints[i] - (width / 2) * Get2DNormal(lengthOfRectangle).normalized;
            //adds points perpendicular to the next trailPoint if it's not out of bounds 
            if (i+1 < trailPoints.Count) {
                meshVertices[i * 4 + 2] = trailPoints[i] + (width / 2) * Get2DNormal(lengthOfRectangle).normalized;
                meshVertices[i * 4 + 3] = trailPoints[i] - (width / 2) * Get2DNormal(lengthOfRectangle).normalized;

            }
        }



    }

    int GetNumOfVertices() {
        int trailCount = trailPoints.Count;
        if (trailCount == 0) return 0;
        else if (trailCount == 1) return 2;
        else return (trailCount - 1) * 4;
    }

    Vector3 Get2DNormal(Vector3 v) {
        return new Vector3(-v.y, v.x, 0);
    }
    void GetTris() {
        meshTriangles = new int[(GetNumOfVertices() * 6) / 4];
        for (int i=0;i< trailPoints.Count-1;i+=2) {
            if (trailPoints.Count > 1) {
                meshTriangles[i * 12] =     i * 12 + 1;
                meshTriangles[i * 12 + 1] = i * 12 + 2;
                meshTriangles[i * 12 + 2] = i * 12 + 0;
                meshTriangles[i * 12 + 3] = i * 12 + 1;
                meshTriangles[i * 12 + 4] = i * 12 + 3;
                meshTriangles[i * 12 + 5] = i * 12 + 2;
                if (i<trailPoints.Count - 2) {
                    meshTriangles[i * 12 + 6] = 12 + 5;
                    meshTriangles[i * 12 + 7] = 12 + 4;
                    meshTriangles[i * 12 + 8] = 12 + 2;
                    meshTriangles[i * 12 + 9] = 12 + 5;
                    meshTriangles[i * 12 + 10] = 12 + 3;
                    meshTriangles[i * 12 + 11] = 12 + 4;
                }
            }
        }
    }

    void GetUVs() {
        meshUVs = new Vector2[meshVertices.Length];
        for (int i=0;i<meshUVs.Length;i++) {
            meshUVs[i] = Vector2.zero;
        }
    }
}
