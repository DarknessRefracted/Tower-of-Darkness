using UnityEngine;
using System.Collections;

public class SeekerBasic : MonoBehaviour {


	public float moveSpeed;
	//public float turnSpeed;
	private Vector3 moveDirection;
	//public Rigidbody2D rigidbody2d;
	private GameObject player;
	void Start () {
		player = GameObject.FindWithTag ("Player");
		Debug.Log ("Player position: " + player.transform.position + ".");
	}
	

	void Update () {
		// 1
		Vector3 currentPosition = transform.position;
		// 2

			// 3
			//Vector3 moveToward = Camera.main.ScreenToWorldPoint( Input.mousePosition );
			Vector3 moveToward = player.transform.position;
			// 4
			moveDirection = moveToward - currentPosition;
			moveDirection.z = 0; 
			moveDirection.Normalize();

		
		Vector3 target = moveDirection * moveSpeed + currentPosition;
		transform.position = Vector3.Lerp( currentPosition, target, Time.deltaTime );
		Debug.Log ("Current " + transform.position + ". Moving to " + target + ".");
		//float targetAngle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
		//transform.rotation = 
			//Quaternion.Slerp( transform.rotation, 
			               //  Quaternion.Euler( 0, 0, targetAngle ), 
			                // turnSpeed * Time.deltaTime );
	}
	
}
