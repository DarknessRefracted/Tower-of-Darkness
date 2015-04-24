using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Radar : MonoBehaviour {

	public List<Transform> nodeList = new List<Transform>();
	public Transform currentNode;
	// Use this for initialization
	public Transform targetNode;
	public Transform startNode;
	private GameObject player;
	private List<Transform> openList = new List<Transform>();
	private List<Transform> closed = new List<Transform> ();
	public List<Transform> path = new List<Transform>();
	private float seekTimeNext = 0;
	public SeekerAStar seekerScript;


	void Awake () {
		player = GameObject.FindGameObjectWithTag ("Player");
		seekerScript = this.GetComponentInParent<SeekerAStar>();//gets the seeker script
		//seekerScript.enabled = false;//disables it

		seekTimeNext = Time.time+0.5f;
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log ("Contains " + nodeList.Count + " nodes.");
	
		//Debug.Log ("Closest node is " + currentNode.position);

		if (Time.time >= seekTimeNext)
		{
			if (nodeList.Count > 0 && seekerScript.seekDone)
			{
				findPath ();
				Debug.Log ("Path.count = " + path.Count);
				if(path.Count > 0)
				{
					Debug.Log ("seekDone is false");
					seekerScript.seekDone = false;
				}
				if(seekerScript.seekDone)
				{
					path = new List<Transform>();
				}
			}
						seekTimeNext = Time.time + 1f;
		}

	}
	void findPath () {
		//GetClosestNode (nodeList);
		path = new List<Transform>();
		nodeList.Sort (byDistanceStart);
		currentNode = GetClosestNodeEnemy(nodeList);
		Debug.Log ("Eneme Node " + currentNode.position);
		//Debug.Log ("Current node type: " + currentNode.GetType());
		targetNode = GetClosestNodePlayer(nodeList);
		Debug.Log ("Player " + targetNode.position);
		//this sets the target for the movement
		this.GetComponentInParent<SeekerAStar> ().targetNode = targetNode;
		openList.Add (currentNode);
		startNode = currentNode;
		while (openList.Count > 0) 
		{
			openList.Sort(byCost);
			currentNode = openList[0];
			closed.Add (currentNode);
			if(openList.Contains (currentNode))
			{
				openList.Remove(currentNode);
				//Debug.Log ("Removed current");
			}
			if(currentNode.position ==  targetNode.position)
			{
				//Debug.Log ("Target reached");
				RetracePath (startNode, targetNode);
				//path = null;
				openList = new List<Transform>();
				closed = new List<Transform>();
				return;
			}
			
			foreach(Transform neighbor in currentNode.GetComponent<NodeAttached>().neighbors)
			{
				//Debug.Log ("In foreach");
				if(neighbor.GetComponent<NodeAttached>().walkable || closed.Contains(neighbor) || !nodeList.Contains (neighbor))
				{
					//Debug.Log ("Continued");
					continue;
				}
				
				int newMovementCostToNeighbor = currentNode.GetComponent<NodeAttached>().g_cost + calculateCost(currentNode, neighbor);
				//Debug.Log ("New move some stuff idk = " + newMovementCostToNeighbor);
				if(newMovementCostToNeighbor < neighbor.GetComponent<NodeAttached>().g_cost || !openList.Contains(neighbor))
				{
					/*
					 * 
					 * 
					 * 
					 * MAKE SURE YOU LIMIT THE NEIGHBOR SEARCH ONLY TO THE RADAR SIZE/WALKABLE/ENDPATH AS CLOSEST NODE TO PLAYER
					 * 
					 * 
					 * 
					 * 
					 * 
					 * 
					 * 
					 * 
					 * 
					 * 
					 */
					//Debug.Log("In if statement to add neighbors");
					neighbor.GetComponent<NodeAttached>().g_cost = newMovementCostToNeighbor;
					neighbor.GetComponent<NodeAttached>().h_cost = calculateCost(neighbor,targetNode);
					neighbor.GetComponent<NodeAttached>().parentNode = currentNode;
					
					if(!openList.Contains(neighbor))
					{
						//Debug.Log ("Added neighbor");
						openList.Add(neighbor);
					}
				}
			}
		
		}//end while
	}//end findPath()

	int byDistanceStart (Transform a, Transform b)
	{
		//var dstToA = Vector3.Distance (subject.transform.position, a.transform.position);
		//var dstToB = Vector3.Distance (subject.transform.position, b.transform.position);
		var dstToA = Vector3.Distance (transform.position, a.transform.position);
		var dstToB = Vector3.Distance (transform.position, b.transform.position);
		return dstToA.CompareTo (dstToB);
	}

	void RetracePath(Transform startNode, Transform endNode)
	{
		//List<Node> path = new List<Node> ();
		Transform currentNode = endNode;
		
		while (currentNode != startNode) {
			path.Add (currentNode);
			currentNode = currentNode.GetComponent<NodeAttached>().parentNode;
			//Debug.Log ("Path node " + currentNode.gridX +", "+ currentNode.gridY);
		}

		path.Reverse ();
		//path.Clear ();
		foreach(Transform n in path)
		{
			Debug.Log ("This transform is " + transform.position);
		}
		Debug.Log ("Path made");
	}

	int calculateCost(Transform a, Transform b)
	{
		int dstX = Mathf.Abs (a.GetComponent<NodeAttached>().gridX - b.GetComponent<NodeAttached>().gridX);
		int dstY = Mathf.Abs (a.GetComponent<NodeAttached>().gridY - b.GetComponent<NodeAttached>().gridY);
		if (dstX > dstY) {
			return 14 * dstY + 10 *(dstX - dstY);	
			//return 1;
		}
		return 14 * dstX + 10 * (dstX - dstY);
		
	}

	int byCost(Transform a, Transform b)
	{
		int comparator = a.GetComponent<NodeAttached>().f_cost.CompareTo (b.GetComponent<NodeAttached>().f_cost);
		if(comparator == 0)
		{
			return a.GetComponent<NodeAttached>().h_cost.CompareTo (b.GetComponent<NodeAttached>().h_cost);
		}
		return comparator;
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

	//optimized closest node position found at http://forum.unity3d.com/threads/clean-est-way-to-find-nearest-object-of-many-c.44315/
	//Transform GetClosestNode (List<Transform> nodeList)
	Transform GetClosestNodeEnemy (List<Transform> nodeList)
	{

		float closestDistanceSqr = Mathf.Infinity;
		Vector3 currentPosition = transform.position;
		foreach(Transform potentialTarget in nodeList)
		{
			Vector3 directionToTarget = potentialTarget.position - currentPosition;
			float dSqrToTarget = directionToTarget.sqrMagnitude;
			if(dSqrToTarget < closestDistanceSqr)
			{
				closestDistanceSqr = dSqrToTarget;
				currentNode = potentialTarget;
			}
		}
		
		return currentNode;
	}

	Transform GetClosestNodePlayer (List<Transform> nodeList)
	{
		
		float closestDistanceSqr = Mathf.Infinity;
		Vector3 playerPosition = player.transform.position;
		foreach(Transform potentialTarget in nodeList)
		{
			Vector3 directionToTarget = potentialTarget.position - playerPosition;
			float dSqrToTarget = directionToTarget.sqrMagnitude;
			if(dSqrToTarget < closestDistanceSqr)
			{
				closestDistanceSqr = dSqrToTarget;
				currentNode = potentialTarget;
			}
		}
		
		return currentNode;
	}
}
