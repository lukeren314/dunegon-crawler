using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject targetObject;
    public float smoothSpeed = 0.125f;
    private Vector3 velocity = Vector3.zero;
	private float cameraZ = -10f;
	public void SetTarget(GameObject newTarget)
    {
		targetObject = newTarget;
    }

	private void FixedUpdate()
	{
		MoveCamera();
	}

	private void MoveCamera()
    {
		Vector3 targetPosition = targetObject.transform.position;
		targetPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
		targetPosition.z = cameraZ;
		transform.position = targetPosition;
	}

}
