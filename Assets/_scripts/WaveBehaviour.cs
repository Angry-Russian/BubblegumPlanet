using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveBehaviour : MonoBehaviour {

    public Transform gravityCenter;

    private Mesh wobbleMesh;
    private Vector3[] wobbleVertices;
    private int[] wobblyTriangles;
    private List<int>[] wobblyAssociations;
    private Vector3[] wobbleVertexSpeeds;
    private Vector3[] wobbleVertexAccelerations;
    private Vector3[] wobbleVertexAxes;
    private int n = 0;

    [Range(1, 20)]
    public float radius = 1;
    private float r; // because the drawn radius isn't the same as the one the vertices converge to, for some reason.
    [Range(0.00005f, 0.75f)]
    public float k = 0.04f;
    [Range(0.00005f, 0.5f)]
    public float friction = 0.1f;
    [Range(1, 6)]
    public int propagation = 3;

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, transform.localScale.magnitude * radius);
    }

    // Use this for initialization
    void Start () {

        wobbleMesh = GetComponent<MeshFilter>().mesh;
        wobbleVertices = wobbleMesh.vertices;
        n = wobbleVertices.Length;

        wobbleVertexAxes = new Vector3[n];
        wobbleVertexSpeeds = new Vector3[n];
        wobblyAssociations = new List<int>[n];
        wobbleVertexAccelerations = new Vector3[n];

        wobblyTriangles = wobbleMesh.triangles;

        for (var i = 0; i<n; i++)   
        {
            wobbleVertexAccelerations[i] = (wobbleVertices[i] - gravityCenter.position).normalized * 9.8f;
            wobbleVertexSpeeds[i] = new Vector3(0, 0, 0);
            wobbleVertexAxes[i] = (wobbleVertices[i] - gravityCenter.position).normalized;
        }

        for(var i = 0; i<wobblyTriangles.Length; i += 3) {
            wobblyAssociations[wobblyTriangles[i]] = wobblyAssociations[wobblyTriangles[i]] ?? new List<int>();
            for (var j = 1; j<3; j++)
            {
                wobblyAssociations[wobblyTriangles[i]].Add(wobblyTriangles[i + j]);
                wobblyAssociations[wobblyTriangles[i + j]] = wobblyAssociations[wobblyTriangles[i + j]] ?? new List<int>();   
                wobblyAssociations[wobblyTriangles[i + j]].Add(wobblyTriangles[i]);
            }
        }


        wobbleVertices[0] = (radius + 2) * wobbleVertexAxes[0];
    }

	// Update is called once per frame
	void FixedUpdate () {
        r = radius / 0.6f;

        wobbleVertices[0] = (Mathf.Cos(Time.realtimeSinceStartup*Mathf.PI)*radius*0.125f + radius) * wobbleVertexAxes[0];


        for (var i = 1; i < n; i++) {
            Vector3 p = wobbleVertices[i];
            Vector3 horizon = p - gravityCenter.position;
            Vector3 f = -k * (horizon.magnitude - r) * wobbleVertexAxes[i]; // starts by going back to its radius

            List<int> adjascents = wobblyAssociations[i];
            int n = adjascents.Count;
            foreach(var j in adjascents) {
                Vector3 partialHorizon = wobbleVertices[j] - gravityCenter.position;
                Vector3 partialForce = k * (partialHorizon.magnitude - r) * wobbleVertexAxes[i];
                f += partialForce / (n);
            }

            wobbleVertexAccelerations[i] = f - friction * wobbleVertexSpeeds[i];
            wobbleVertexSpeeds[i] += wobbleVertexAccelerations[i];
            wobbleVertices[i] += wobbleVertexSpeeds[i];
        }

        wobbleMesh.vertices = wobbleVertices;
    }

    public Vector3 NearestVertexTo(Vector3 point) {
        // referenced code : http://answers.unity3d.com/questions/7788/closest-point-on-mesh-collider.html
        // convert point to local space
        point = transform.InverseTransformPoint(point);


        Mesh mesh = GetComponent<MeshFilter>().mesh;
        float minDistanceSqr = Mathf.Infinity;
        Vector3 nearestVertex = Vector3.zero;
        // scan all vertices to find nearest
        foreach (Vector3 vertex in mesh.vertices) {
            Vector3 diff = point - vertex;
            float distSqr = diff.sqrMagnitude;
            if (distSqr < minDistanceSqr){
                minDistanceSqr = distSqr;
                nearestVertex = vertex;
            }
        }
        // convert nearest vertex back to world space
        return transform.TransformPoint(nearestVertex);

    }
}
