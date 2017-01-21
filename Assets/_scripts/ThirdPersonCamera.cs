using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour {

    private const float Y_ANGLE_MIN = 0.0f;
    private const float Y_ANGLE_MAX = 90.0f;

    public Transform lookAt;
    public Transform camTransform;

    private Camera cam;

    private float distance = 3.0f;
    private float currentX = 0.0f;
    private float currentY = 0.0f;
    private float sensitivityX = 40.0f;
    private float sensitivityY = 10.0f;

    void Start()
    {
        camTransform = transform;
        cam = GetComponent<Camera>();
    }
    void Update()
    {
        currentX += Input.GetAxisRaw("Mouse X")* sensitivityX;
        currentY += Input.GetAxisRaw("Mouse Y")* sensitivityY;
        camTransform.RotateAround(new Vector3(0, 0, 0), new Vector3(0,1,0), currentY);
        currentY = Mathf.Clamp(currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);
    }
    void LateUpdate()
    {
        Vector3 dir = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        //camTransform.RotateAround(new Vector3(0,0,0),y,)
        //camTransform.position = lookAt.position + rotation * dir;
        //camTransform.LookAt(lookAt.position);
    }
}
