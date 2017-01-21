using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveBehaviour : MonoBehaviour {

    private Mesh wobbleMesh;
    private Vector3[] wobbleVertices;
    
	// Use this for initialization
	void Start () {
        wobbleMesh = GetComponent<MeshFilter>().mesh;
        wobbleVertices = wobbleMesh.vertices;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
