using UnityEngine;
using System.Collections;

public class SmoothFollow : MonoBehaviour {
	public Transform target;
	public float distance = 10.0f;
	public float height = 0f;
	public float damping = 2.0f;
	public bool smoothRotation = false;
	public float rotationDamping = 10.0f;
	public bool lockRotation = true;
	
	void Update () {
		Vector3 wantedPosition = target.TransformPoint(0, height, -distance);
		transform.position = Vector3.Lerp (transform.position, wantedPosition, Time.deltaTime * damping);
		
		if (smoothRotation) {
			Quaternion wantedRotation = Quaternion.LookRotation(target.position - transform.position, target.up);
			transform.rotation = Quaternion.Slerp (transform.rotation, wantedRotation, Time.deltaTime * rotationDamping);
		}
		
		else transform.LookAt (target, target.up);

		if (lockRotation){
			transform.localRotation = Quaternion.Euler(0,0,0);
		}
	}
}