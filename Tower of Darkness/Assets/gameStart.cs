﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class gameStart : MonoBehaviour {

	public Spawner spawnerScript;
	public Grid gridScript;
	List<Node2> closed = new List<Node2>();
	List<Node2> open = new List<Node2>();
	List<Node2> path = new List<Node2> ();
	//GameObject current;
	Node2 neighbor = new Node2();
	Node2 current;
	Node2 target;
	Node2 start;
	int maxX;
	int maxY;
	// Use this for initialization
	void Start () {
		
	}
	void Update(){

			//if(path == null)
				findPath ();

	}
	// Update is called once per frame
	void findPath () {

		if (GameObject.Find ("Subject(Clone)") && GameObject.Find ("Target(Clone)") ) {
						start = spawnerScript.beginNode;
						current = spawnerScript.beginNode;
						target = spawnerScript.finishNode;
						open.Add (current);
						//Debug.Log ("Added current to open");
						Debug.Log ("Added a node to open");
				} else {
						Debug.Log ("DIDN'T ADD");
				}

		while (open.Count > 0) 
		{
			//Debug.Log ("Open Count = " + open.Count);
			open.Sort(byCost);
			current = open[0];
			closed.Add (current);
			if(open.Contains (current))
			{
				open.Remove(current);
				//Debug.Log ("Removed current");
			}
			//closed.Add (current);
			//Debug.Log ("In while");
			if(current.worldPosition ==  target.worldPosition)
			{
				Debug.Log ("Target reached");
				RetracePath (start, target);
				//path = null;
				open= new List<Node2>();
				closed = new List<Node2>();
				return;
			}

			foreach(Node2 neighbor in gridScript.GetNeightbors(current))
			{
				//Debug.Log ("In foreach");
				if(neighbor.walkable || closed.Contains(neighbor))
				{
					//Debug.Log ("Continued");
					continue;
				}

				int newMovementCostToNeighbor = current.g_cost + calculateCost(current, neighbor);
				//Debug.Log ("New move some stuff idk = " + newMovementCostToNeighbor);
				if(newMovementCostToNeighbor < neighbor.g_cost || !open.Contains(neighbor))
				{
					//Debug.Log("In if statement to add neighbors");
					neighbor.g_cost = newMovementCostToNeighbor;
					neighbor.h_cost = calculateCost(neighbor,target);
					neighbor.parentNode = current;

					if(!open.Contains(neighbor))
					{
						//Debug.Log ("Added neighbor");
						open.Add(neighbor);
					}
				}
			}
		}
		//Debug.Log ("LEFT LOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOP!!");


	}
	
	int calculateCost(Node2 a, Node2 b)
	{
		int dstX = Mathf.Abs (a.gridX - b.gridX);
		int dstY = Mathf.Abs (a.gridY - b.gridY);
		if (dstX > dstY) {
			return 14 * dstY + 10 *(dstX - dstY);	
			//return 1;
		}
		return 14 * dstX + 10 * (dstX - dstY);
		
		}
	void RetracePath(Node2 startNode, Node2 endNode)
	{
		//List<Node> path = new List<Node> ();
		Node2 currentNode = endNode;

		while (currentNode != startNode) {
						path.Add (currentNode);
						currentNode = currentNode.parentNode;
			//Debug.Log ("Path node " + currentNode.gridX +", "+ currentNode.gridY);
				}
		path.Reverse ();
		gridScript.path = path;
		//path.Clear ();
		Debug.Log ("Path made");
	}


	int byCost(Node2 a, Node2 b)
	{
			int comparator = a.f_cost.CompareTo (b.f_cost);
			if(comparator == 0)
			{
				return a.h_cost.CompareTo (b.h_cost);
			}
			return comparator;
	}

}