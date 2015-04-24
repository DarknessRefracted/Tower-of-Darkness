using UnityEngine;
using System.Collections;

public class SeekerAStar : MonoBehaviour {

	public float moveSpeed;
	//public float turnSpeed;
	private Vector3 moveDirection;
	//public Rigidbody2D rigidbody2d;
	//private GameObject player;
	public Transform targetNode;
	private Vector3 currentPosition;
	private Vector3 moveToward;
	public bool seekDone = true;
	public Radar radarScript;
	void Start () {
		radarScript = this.GetComponentInParent<Radar>();
		targetNode = this.transform;
		//Debug.Log ("Player position: " + player.transform.position + ".");
		seekDone = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (!seekDone) {
						foreach(Transform n in this.GetComponentInParent<Radar>().path)
						{
							Debug.Log ("Inside foreach");
							currentPosition = transform.position;
							//Vector3 moveToward = Camera.main.ScreenToWorldPoint( Input.mousePosition );
							moveToward = n.position;
				Debug.Log ("Moving towards" + moveToward);

							moveDirection = moveToward - currentPosition;
							moveDirection.z = 0; 
							moveDirection.Normalize ();
							Vector3 target = moveDirection * moveSpeed + currentPosition;
							transform.position = Vector3.Lerp (currentPosition, target, Time.deltaTime);
						}
				}
		seekDone = true;
	}
}
