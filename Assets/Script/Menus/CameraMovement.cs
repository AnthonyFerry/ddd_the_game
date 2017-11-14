using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    float _moveSpeed, _rotateSpeed;

    Transform _targetPosition;

	// Use this for initialization
	void Start () {
        _targetPosition = transform;
	}
	
	// Update is called once per frame
	void Update () {
        if (transform.position == _targetPosition.position && transform.rotation == _targetPosition.rotation)
            return;
        
        transform.position = Vector3.MoveTowards(transform.position, _targetPosition.position, _moveSpeed * Time.deltaTime);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetPosition.rotation, _rotateSpeed * Time.deltaTime);
    }

    public void MoveToPosition(Transform targetPosition, float time)
    {
        _moveSpeed = Vector3.Distance(transform.position, targetPosition.position) / time;
        _rotateSpeed = Quaternion.Angle(transform.rotation, targetPosition.rotation) / time; 

        _targetPosition = targetPosition;
    }
}
