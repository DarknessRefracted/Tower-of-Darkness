using UnityEngine;
using System.Collections;

public class WallSensors : MonoBehaviour {

	public float senseRange;
	public RaycastHit2D[] sensors = new RaycastHit2D[3];
	public float[] distanceToWall = new float[3];
	private float noWallVal;
	public bool wallTouched;

	// Use this for initialization
	void Start () {
		noWallVal = senseRange + 10;
	}

	public float[] SenseWalls(Transform subTrans){
		//If there is an animation for wall detection, activate it
		//subTrans.GetComponent<Animator> ().SetBool ("Touch", false);

		//Reset the sensors. Set the LayerMask as 5 so it will ignore the UI layer (which is what the Radar is in; it was disrupting our
		//	sensors for the longest time).
		sensors[0] = Physics2D.Raycast(subTrans.position, 
		                               Quaternion.AngleAxis(-45, subTrans.forward) * subTrans.right,
		                               senseRange, 5);
		sensors [1] = Physics2D.Raycast (subTrans.position, subTrans.right, senseRange, 5);
		sensors[2] = Physics2D.Raycast(subTrans.position,
		                               Quaternion.AngleAxis(45, subTrans.forward) * subTrans.right,
		                               senseRange, 5);

		wallTouched = false;
		//Reset wall distances. They are set to a value far beyond the senseRange as such a value is not possible unless there are no
		//	walls within range.
		distanceToWall [0] = noWallVal;
		distanceToWall [1] = noWallVal;
		distanceToWall [2] = noWallVal;

		//Check if the sensors have hit anything
		//NOTE: For the vector additions made, normalizing is necessary as we are using sums for directions and for determining
			// if movement is possible

		if(sensors[0].collider != null && sensors[0].collider.CompareTag("Wall")){
			distanceToWall[0] = sensors[0].distance;
			Debug.Log("Sensor 1 distance to wall: "+distanceToWall[0]);
			Debug.DrawLine (subTrans.position, sensors[0].point, Color.red);
		}

		if(sensors[1].collider != null && sensors[1].collider.tag.Equals("Wall")){
			distanceToWall[1] = sensors[1].distance;
			Debug.Log("Sensor 2 distance to wall: "+distanceToWall[1]);
			Debug.DrawLine (subTrans.position, sensors[1].point, Color.green);
			//subTrans.GetComponent<Animator> ().SetBool ("Touch", true);
		}

		if(sensors[2].collider != null && sensors[2].collider.CompareTag("Wall")){
			distanceToWall[2] = sensors[2].distance;
			Debug.Log("Sensor 3 distance to wall: "+distanceToWall[2]);
			Debug.DrawLine (subTrans.position, sensors[2].point, Color.blue);
		}

		return distanceToWall;
	}

	//Function is used to get the distance to a wall by an outside function
	public float GetDistanceToWall(int sensorIndex){
		if(distanceToWall[sensorIndex] != noWallVal)
			return distanceToWall [sensorIndex];

		return noWallVal;
	}


}
