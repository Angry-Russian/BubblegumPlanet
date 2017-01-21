using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityAttraction : MonoBehaviour {

    public float gravity = -10;
    public Vector3 gravityUp;
    public Vector3 Attract(Transform body)
    {
        gravityUp = (body.position - transform.position).normalized;
        Vector3 bodyUp = body.up;

        body.GetComponent<Rigidbody>().AddForce(gravityUp * gravity);
        Quaternion targetRotation = Quaternion.FromToRotation(bodyUp, gravityUp) * body.rotation;
        body.rotation = Quaternion.Slerp(body.rotation, targetRotation, 50 * Time.deltaTime);
        return gravityUp;
    }

    public void FixedUpdate()
    {
        MeshCollider collider = GetComponent<MeshCollider>();
        if(collider)
        {
            collider.sharedMesh = GetComponent<MeshFilter>().mesh;
        }
    }
}
