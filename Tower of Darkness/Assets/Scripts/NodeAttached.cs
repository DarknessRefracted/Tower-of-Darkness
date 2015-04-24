using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class NodeAttached : MonoBehaviour {
	public bool walkable;
	public GameObject node;
	public GameObject parent;
	//public Node nody;
	public Transform parentNode;
	public Vector2 worldPosition;
	public int g_cost; //distance to start (past cost)
	public int h_cost; //distance to target(heuristic cost)
	public int f_cost; //total cost
	public int gridX;
	public int gridY;
	private GameObject target;
	public List<Transform> neighbors;
	float nextTimeToSearch = 0;


	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (target == null) {
			FindPlayer ();
			return;
		}
		if (neighbors != null) {
			//Debug.Log ("Node neighbors " + neighbors); 		
		}

	}

	void FindPlayer () {
		if (nextTimeToSearch <= Time.time) {
			GameObject searchResult = GameObject.FindGameObjectWithTag ("Player");
			if (searchResult != null)
				target = searchResult;
			nextTimeToSearch = Time.time + 0.5f;
		}
	}

	void GetG(Transform startPosition)
	{
		//g_cost = start
	}
}
