using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SubjectController3 : MonoBehaviour {
	
	public float movementSpeed = 5.0f;
	public float clockwise = 1000f;
	//public float counterClockwise = -100.0f;
	GameObject subject;
		
	void Start () {
		//Grid gridScript = GetComponent<Grid>();
		//subject = gridScript.subject;
	}
		
	void FixedUpdate () {
	// transform.position += transform.forward * Time.deltaTime * movementSpeed
		if(Input.GetKey(KeyCode.E)) {
			transform.position += transform.forward * Time.deltaTime * movementSpeed;
		}
		else if(Input.GetKey(KeyCode.Q)) {
			transform.position -= transform.forward * Time.deltaTime * movementSpeed;
		}
		else if(Input.GetKey(KeyCode.S)) {
			transform.position -= transform.right * Time.deltaTime * movementSpeed;
		}
		else if(Input.GetKey(KeyCode.W)) {
			transform.position += transform.right * Time.deltaTime * movementSpeed;
		}
		if(Input.GetKey(KeyCode.A)) {
			transform.Rotate(0, 0, (Time.deltaTime * clockwise)/3);
		}
		else if(Input.GetKey(KeyCode.D)) {
			transform.Rotate(0, 0, -(Time.deltaTime * clockwise)/3);
		}
	}
}

