using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Trail : MonoBehaviour {

    Mesh trail;
    public int length;
    List<Vector3> trailPoints;
    public float width;
    Vector3[] meshVertices;
    Vector2[] meshUVs;
    int[] meshTriangles;
    public GameObject cube;

    // Use this for initialization
    void Start() {
        trailPoints = new List<Vector3>();
        trail = new Mesh();
        GetComponent<MeshFilter>().mesh = trail;
        CreateMesh();
    }

    // Update is called once per frame
    void Update() {
    }

    void FixedUpdate() {
        for (int i = 0; i < trailPoints.Count; i++) {
            trailPoints[i] = trailPoints[i] + (ObstacleSpawner.worldVelocity - transform.parent.gameObject.GetComponent<Rigidbody>().velocity) * Time.deltaTime;
        }
        while (trailPoints.Count > length) {
            trailPoints.RemoveAt(0);
        }
        trailPoints.Add(Vector3.zero);
        CreateMesh();

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
                trailNormal = (trailPoints[i] - trailPoints[i + 1]);
            }
            //otherwise it's somewhere in the middle
            else {
                trailNormal = (trailPoints[i-1] - trailPoints[i]);
            }
            trailNormal = Get2DNormal(trailNormal).normalized;
            meshVertices[i * 2] = trailPoints[i] + width / 2 * trailNormal;
            meshVertices[i * 2 + 1] = trailPoints[i] - width / 2 * trailNormal;
        }
    }


    Vector3 Get2DNormal(Vector3 v) {
        return new Vector3(-v.y, v.x, 0);
    }
    void GetTris() {
        //there should be an error here
        meshTriangles = new int[(meshVertices.Length - 2) * 3 * 2];
        for (int i = 0; i < meshTriangles.Length / 12; i++) {
            meshTriangles[i * 12] = i * 2 + 1;
            meshTriangles[i * 12 + 1] = i * 2 + 0;
            meshTriangles[i * 12 + 2] = i * 2 + 2;
            meshTriangles[i * 12 + 3] = i * 2 + 1;
            meshTriangles[i * 12 + 4] = i * 2 + 2;
            meshTriangles[i * 12 + 5] = i * 2 + 0;


            meshTriangles[i * 12 + 6] = i * 2 + 1;
            meshTriangles[i * 12 + 7] = i * 2 + 2;
            meshTriangles[i * 12 + 8] = i * 2 + 3;
            meshTriangles[i * 12 + 9] = i * 2 + 1;
            meshTriangles[i * 12 + 10] = i * 2 + 3;
            meshTriangles[i * 12 + 11] = i * 2 + 2;

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
