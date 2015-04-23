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
	private List<Transform> path = new List<Transform>();
	private float seekTimeNext = 0;



	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		seekTimeNext = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log ("Contains " + nodeList.Count + " nodes.");

		//Debug.Log ("Closest node is " + currentNode.position);

		if (Time.time >= seekTimeNext) {
						if (nodeList.Count > 0)
								//findPath ();
						seekTimeNext += 1f;
				}

	}
	void findPath () {
		//GetClosestNode (nodeList);
		nodeList.Sort (byDistanceStart);
		currentNode = GetClosestNode(nodeList);
		//Debug.Log ("Current node type: " + currentNode.GetType());
		targetNode = player.GetComponent<RadarSmall>().playerNode;
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
				if(neighbor.GetComponent<NodeAttached>().walkable || closed.Contains(neighbor))
				{
					//Debug.Log ("Continued");
					continue;
				}
				
				int newMovementCostToNeighbor = currentNode.GetComponent<NodeAttached>().g_cost + calculateCost(currentNode, neighbor);
				//Debug.Log ("New move some stuff idk = " + newMovementCostToNeighbor);
				if(newMovementCostToNeighbor < neighbor.GetComponent<NodeAttached>().g_cost || !openList.Contains(neighbor))
				{
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
	Transform GetClosestNode (List<Transform> nodeList)
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

}
