using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AdjAgentSensor : MonoBehaviour {

	Vector3 heading;
	Transform subjectTransform;
	Transform agentTransform;
	//bool[] inRange = new bool[10];
	//int agentCount = 0;
	public List<Transform> agentList= new List<Transform>();
	// Use this for initialization
	void Start () {
		subjectTransform = this.transform;
	}
	
	// Update is called once per frame
	void Update () {
		//int i = 0;
		/*foreach (bool agentPresent in inRange)
		{
			if (agentPresent) {
				Debug.Log ("Agent " + i + " Inside Collider");
				// Gets a vector that points from the player's position to the target's.
				heading = agentTransform[i].position - subjectTransform.position;
				Debug.Log ("Heading is " + heading);
			}
			i++;
		}*/
		int agentPie1 = 0;
		int agentPie2 = 0;
		int agentPie3 = 0;
		int agentPie4 = 0;
		foreach (Transform agent in agentList) {
			agent.GetComponent<Animator> ().SetBool ("Change", true);
			heading = agent.position - subjectTransform.position;
			Vector3 front = subjectTransform.right;
			float angle = Vector3.Angle (heading, front);
			Vector3 cross = Vector3.Cross(heading, front);
			if (cross.z < 0) angle = -angle;
			if(angle < 0 && angle > -90)
			{
				agentPie1++;
			}
			if(angle > 0 && angle < 90)
			{
				agentPie2++;
			}
			if(angle > 90 && angle < 180)
			{
				agentPie3++;
			}
			if(angle < -90 && angle > -180)
			{
				agentPie4++;
			}

			Debug.Log ("Target name: " + agent.name + ". Distance: " + heading.magnitude + ". Heading: " + heading + ". Angle: " + angle);
			Debug.Log ("Pie Slices: " + agentPie1 + ", " + agentPie2 + ", " + agentPie3 + ", " + agentPie4 + ".");
		
		}



	}

	void OnTriggerEnter2D(Collider2D other)
	{

		if(other.CompareTag("Agent")){
			agentList.Add (other.transform);
		}
		/*if(other.CompareTag("Agent")) {
			//inRange = true;

			inRange [agentCount] = true;
			agentTransform[agentCount] = other.transform;
			agentCount++;
			Update ();
		}*/
	}

	void OnTriggerExit2D(Collider2D other) {

		/*if(other.CompareTag("Agent")) {
			//inRange = false;
			inRange [agentCount] = false;
			Update ();			
		}*/
		//agentCount--;
		//agentList.Remove (other.transform.parent.gameObject);
			if(other.CompareTag("Agent")){
				agentList.Remove (other.transform);
				other.transform.GetComponent<Animator> ().SetBool ("Change", false);
			}
	}

	void OnTriggerStay2D(Collider2D other) {

		//transform.positionother.transform.parent.transform.position;
		//agentTransform  = other.transform;
		//Debug.Log ("Agent " + i + " Inside Collider");
		// Gets a vector that points from the player's position to the target's.
		//heading = agentTransform.position - subjectTransform.position;
		//Debug.Log ("Heading is " + heading);
	}
}
