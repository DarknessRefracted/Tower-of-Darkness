using UnityEngine;
using System.Collections;

public class MazePilotController : MonoBehaviour {

	public float movementSpeed;
	private float move;
	public Rigidbody2D rb2d;

	public MazeSolver solverScript;

	private Vector3 temp;

	// Use this for initialization
	void Start () {
		if(movementSpeed >= 1)
			movementSpeed *= 0.25f;

		solverScript = (MazeSolver)GameObject.FindWithTag ("Tower").GetComponent ("MazeSolver");
		rb2d = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetKey(KeyCode.W)){
			rb2d.AddRelativeForce(transform.up * movementSpeed);
		}
		else if(Input.GetKey(KeyCode.A)){
			rb2d.AddRelativeForce((transform.right * -1) * movementSpeed);
		}
		else if(Input.GetKey(KeyCode.S)){
			rb2d.AddRelativeForce(transform.up * -1 * movementSpeed);
		}
		else if(Input.GetKey(KeyCode.D)){
			rb2d.AddRelativeForce(transform.right * movementSpeed);
		}
	}

	void OnCollisionEnter2D(Collision2D other) {
		if(other.gameObject.CompareTag("MazeFinish")){
			solverScript.playerFinished = true;
		}
	}
}
