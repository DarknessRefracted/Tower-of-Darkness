using UnityEngine;
using System.Collections;

public class SeekerAStar : MonoBehaviour {

	public float moveSpeed;
	//public float turnSpeed;
	private Vector3 moveDirection;
	//public Rigidbody2D rigidbody2d;
	private GameObject player;
	private Vector3 currentPosition;


	void Start () {
		player = GameObject.FindWithTag ("Player");
		//Debug.Log ("Player position: " + player.transform.position + ".");
		currentPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
