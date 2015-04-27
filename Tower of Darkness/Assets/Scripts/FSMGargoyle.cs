using UnityEngine;
using System.Collections;

public class FSMGargoyle : MonoBehaviour
{
		GameObject player;
		public RadarForPlayer scriptRadar;
		public float speed;
		private bool inRange;
		bool leftOfPlayer;
		//bool rightOfPlayer;
		//bool abovePlayer;
		//bool belowPlayer;
		float leftChargeCooldown;
		float rightChargeCooldown;
		public Vector3[] patrol;
		private int Currentpoint;
		public float moveSpeed;
		public CharController scriptController;

		void Start ()
		{
				player = GameObject.FindWithTag ("Player");
				scriptRadar = transform.GetComponentInChildren<RadarForPlayer> ();
				patrol = new Vector3[5];
				patrol [0] = transform.position + new Vector3 (0, 4, 0);
				patrol [1] = transform.position + new Vector3 (0, 0, 0);
				patrol [2] = transform.position + new Vector3 (4, 0, 0);
				patrol [3] = transform.position + new Vector3 (0, -4, 0);
				patrol [4] = transform.position + new Vector3 (0, 0, 0);
				leftChargeCooldown = Time.time;
				rightChargeCooldown = Time.time;
				scriptController = player.GetComponent<CharController> ();
		}

		void Update ()
		{
				if (!scriptController.freezeMovement) {
						inRange = scriptRadar.playerInRange;
						//Debug.Log ("Inrange is " + inRange);
						Vector3 directionToTarget = transform.position - player.transform.position;
						if (directionToTarget.x > 0) {
								leftOfPlayer = true;
								//rightOfPlayer = false;
						} else {
								//	rightOfPlayer = true;
								leftOfPlayer = false;
						}
			/*
		if(directionToTarget.y > 0)
		{
			abovePlayer = true;
			belowPlayer = false;
		} else {
			abovePlayer = false;
			belowPlayer = true;
		}*/

						if (inRange && leftChargeCooldown - Time.time < 0 && rightChargeCooldown - Time.time < 0) {
								if (leftOfPlayer == true && leftChargeCooldown - Time.time < 0) {
										StartCoroutine ("chargeLeft");
								} else if (rightChargeCooldown - Time.time < 0) {
										StartCoroutine ("chargeRight");
								}
						} else {

								if (Vector3.Distance (transform.position, patrol [Currentpoint]) < 0.5f) {
										Currentpoint++;
								}
				
								if (Currentpoint >= patrol.Length) {
										Currentpoint = 0;
								}

								transform.position = Vector3.MoveTowards (transform.position, patrol [Currentpoint], speed * Time.deltaTime);    

						}

				}


		}

		IEnumerator chargeLeft ()
		{

				float startCharge = Time.time;
				float finishTime = startCharge + 1.5f;
				while (Time.time < finishTime) {
						if (transform.position.x > -4.4) {
								transform.Translate (-Vector3.right * speed / 10 * Time.deltaTime);
						}
						yield return null; //will yield for update of frames
				}
				leftChargeCooldown = Time.time + 5f;
		}

		IEnumerator chargeRight ()
		{
				float startCharge = Time.time;
				float finishTime = startCharge + 1.5f; 
		

				while (Time.time < finishTime) {
						if (transform.position.x < 4.4) {
								transform.Translate (Vector3.right * speed / 10 * Time.deltaTime);

						}
						yield return null; //will yield for update of frames
				}

				rightChargeCooldown = Time.time + 5f;
		}

}
