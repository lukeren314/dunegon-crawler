using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	private const float DEFAULT_CAMERA_SPEED = 0.125f;
	private const float DEFAULT_CAMERA_Z = -5f;
	private const float DEFAULT_CAMERA_SIZE = 2;
	private const float MIN_CAMERA_SIZE = 1.5f;
	private const float MAX_CAMERA_SIZE = 3.5f;
	private const float CAMERA_ZOOM_SPEED = 2.0f;
	private const float CAMERA_ZOOM_SMOOTH_SPEED = 0.125f;

	public Camera camera;
    public GameObject targetObject;

	private Vector3 velocity = Vector3.zero;
	private float smoothSpeed = DEFAULT_CAMERA_SPEED;
	private float cameraSize = DEFAULT_CAMERA_SIZE;
	public void SetTarget(GameObject newTarget)
    {
		targetObject = newTarget;
    }
    private void Update()
    {
		float scroll = Input.GetAxis("Mouse ScrollWheel");
		SetCameraSize(cameraSize - scroll * CAMERA_ZOOM_SPEED);
	}

    private void FixedUpdate()
	{
		MoveCamera();
	}

	private void MoveCamera()
    {
		Vector3 targetPosition = targetObject.transform.position;
		targetPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
		targetPosition.z = DEFAULT_CAMERA_Z;
		transform.position = targetPosition;
	}
	private void SetCameraSize(float cameraSize)
	{
		if (cameraSize < MIN_CAMERA_SIZE)
		{
			this.cameraSize = MIN_CAMERA_SIZE;
		}
		else if (cameraSize > MAX_CAMERA_SIZE)
		{
			this.cameraSize = MAX_CAMERA_SIZE;
		}
		else
		{
			this.cameraSize = cameraSize;
		}

		camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, this.cameraSize, CAMERA_ZOOM_SMOOTH_SPEED);
	}

}
