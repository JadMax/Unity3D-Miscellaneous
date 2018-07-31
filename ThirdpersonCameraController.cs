// You need to make the camera's tag be "MainCamera" first.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdpersonCameraController : MonoBehaviour {
    public float mouseSensitivity = 2;
    public Transform targetObject;

    public Transform cameraAxle;
    Vector3 thirdPCamEuler;

    Camera thirdPCam;
    public float camForwardMaxDistance = 9;
    float camForwardDistance;
    float camBackDistance;

	void Start () {
        thirdPCamEuler = cameraAxle.localEulerAngles;

        thirdPCam = Camera.main;
	}
	
	void Update () {
        cameraAxle.position = targetObject.position;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        thirdPCamEuler.y += mouseX;
        if (thirdPCamEuler.z < 90 && mouseY > 0) { thirdPCamEuler.z += mouseY; }
        if (thirdPCamEuler.z > -90 && mouseY < 0) { thirdPCamEuler.z += mouseY; }
        cameraAxle.localEulerAngles = thirdPCamEuler;

        Ray camForwardRay = new Ray(thirdPCam.transform.position, thirdPCam.transform.forward);
        RaycastHit forwardRayHit;
        if (Physics.Raycast(camForwardRay, out forwardRayHit))
        {
            camForwardDistance = Vector3.Distance(thirdPCam.transform.position, forwardRayHit.point);
        }

        Vector3 dir = thirdPCam.transform.position - targetObject.position;;
        dir = dir.normalized;
        Ray ray = new Ray(targetObject.position, dir);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.green);
            thirdPCam.transform.position = hit.point;
        }
        else
        {
            if (camForwardDistance < camForwardMaxDistance)
            {
                thirdPCam.transform.Translate(Vector3.back * Time.deltaTime * 5);
            }
        }
    }
}
