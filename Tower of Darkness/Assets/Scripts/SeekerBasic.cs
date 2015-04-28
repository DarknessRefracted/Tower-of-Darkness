using UnityEngine;
using System.Collections;

public class SeekerBasic : MonoBehaviour {


	private float moveSpeed = 1f;
	//public float turnSpeed;
	private Vector3 moveDirection;
	//public Rigidbody2D rigidbody2d;
	private GameObject player;
	public CharController scriptController;
	public LevelGenerator scriptLevelGen;
	private bool speedIncreased = false;
	GameObject tower;
	void Start () {
		tower = GameObject.FindWithTag("Tower");
		player = GameObject.FindWithTag ("Player");
		scriptController = player.GetComponent<CharController>();
		scriptLevelGen = tower.GetComponent<LevelGenerator>();
	}
	

	void Update () {
		// 1
		if(!scriptController.freezeMovement){
			if(scriptLevelGen.levelCounter % 5 == 0 && !speedIncreased)
			{
				moveSpeed += 0.2f;
				speedIncreased = true;
			}else if (scriptLevelGen.levelCounter % 5 != 0){
				speedIncreased = false;
			}


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
		}
	}
}
