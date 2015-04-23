using UnityEngine;
using System.Collections;

public class SeekerAStar : MonoBehaviour {

	public float moveSpeed;
	//public float turnSpeed;
	private Vector3 moveDirection;
	//public Rigidbody2D rigidbody2d;
	private GameObject player;
	public Transform targetNode;
	private Vector3 currentPosition;
	private Vector3 moveToward;

	void Start () {
		targetNode = this.transform;
		//Debug.Log ("Player position: " + player.transform.position + ".");
	}
	
	// Update is called once per frame
	void Update () {
		currentPosition = transform.position;
		// 2

		// 3
		//Vector3 moveToward = Camera.main.ScreenToWorldPoint( Input.mousePosition );

		moveToward = targetNode.position;
		// 4
		moveDirection = moveToward - currentPosition;
		moveDirection.z = 0; 
		moveDirection.Normalize();
		
		
		Vector3 target = moveDirection * moveSpeed + currentPosition;
		transform.position = Vector3.Lerp( currentPosition, target, Time.deltaTime );

	}
}
