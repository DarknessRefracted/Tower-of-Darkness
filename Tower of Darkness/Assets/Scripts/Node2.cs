using UnityEngine;
using System.Collections;

public class Node2  {

	public bool walkable;
	public GameObject node;
	public GameObject parent;
	//public Node nody;
	public Node2 parentNode;
	public Vector2 worldPosition;
	public int g_cost; //distance to start
	public int h_cost; //distance to target
	public int f_cost; //total cost
	public int gridX;
	public int gridY;

	public Node2(bool _walkable, Vector2 _worldPos, GameObject _node, int x, int y){
		walkable = _walkable;
		worldPosition = _worldPos;
		node = _node;
		//nody = _nody;
		gridX = x;
		gridY = y;

	}
	public Node2()
	{

	}

	public int fCost {
				get {
						return g_cost + h_cost;
				}
	
		}
}
