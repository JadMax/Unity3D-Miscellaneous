using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstpersonCameraController : MonoBehaviour {
    [SerializeField] public float mouseSensitivity = 2.0f;
    public Camera camera;
    public bool useMianCamera = true;
    [SerializeField] public bool verticalFlip = false;
    public Transform camraBindingTarget;

    private Vector3 cameraEulers;

	void Start () {

	}
	
	void Update () {
		if (useMianCamera) { camera = Camera.main; } else { camera = this.GetComponent<Camera>(); }
        UpdateCamera();
	}

    void UpdateCamera()
    {
        if (camraBindingTarget != null) { camera.transform.position = camraBindingTarget.position; }
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        cameraEulers.y += mouseX;
        if (verticalFlip)
        {
            if (cameraEulers.x > -90 && mouseY < 0) { cameraEulers.x += mouseY; }
            if (cameraEulers.x < 90 && mouseY > 0) { cameraEulers.x += mouseY; }
        }
        else
        {
            if (cameraEulers.x < 90 && mouseY < 0) { cameraEulers.x -= mouseY; }
            if (cameraEulers.x > -90 && mouseY > 0) { cameraEulers.x -= mouseY; }
        }
        camera.transform.eulerAngles = cameraEulers;
    }
}
