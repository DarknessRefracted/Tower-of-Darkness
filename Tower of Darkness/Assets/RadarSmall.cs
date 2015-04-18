using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RadarSmall : MonoBehaviour {
	public List<Transform> nodeList = new List<Transform>();
	public Transform playerNode;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log ("Contains " + nodeList.Count + " nodes.");
		playerNode = GetClosestNode (nodeList);
		//Debug.Log ("Closest node is " + playerNode.position);
	}
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag ("Node")) {
			nodeList.Add (other.transform);
		}
	}
	
	void OnTriggerExit2D(Collider2D other) {
		if(other.CompareTag("Node")){
			nodeList.Remove (other.transform);
			//other.transform.GetComponent<Animator> ().SetBool ("Change", false);
		}
	}
	Transform GetClosestNode (List<Transform> nodeList)
	{
		Transform bestTarget = null;
		float closestDistanceSqr = Mathf.Infinity;
		Vector3 currentPosition = transform.position;
		foreach(Transform potentialTarget in nodeList)
		{
			Vector3 directionToTarget = potentialTarget.position - currentPosition;
			float dSqrToTarget = directionToTarget.sqrMagnitude;
			if(dSqrToTarget < closestDistanceSqr)
			{
				closestDistanceSqr = dSqrToTarget;
				bestTarget = potentialTarget;
			}
		}
		
		return bestTarget;
	}
}
