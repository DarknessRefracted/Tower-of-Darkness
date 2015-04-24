using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridAttached : MonoBehaviour
{

		public List<Node2> path;
	private Vector2 size = new Vector2 (22f, 10f);
		public Transform Node;
		public Transform Node_bad;
		public Transform Subject;
		public Transform Target;
		public GameObject[,] grid;
		public Node2[,] nodeGrid;
		public List<GameObject> closestObjects = new List<GameObject> ();
		public int gridWidth, gridLength;
		GameObject[] tempGrid;
		int i = 0, j = 0;
		float x;
		float y;

		// Use this for initialization
		void Awake ()
		{
				CreateGrid ();
		}
	
		// Update is called once per frame
		void Update ()
		{
				
		}

		void CreateGrid ()
		{
				gridWidth = (int)size.x * 2;
				gridLength = (int)size.y * 2;
				grid = new GameObject[gridWidth, gridLength];
				nodeGrid = new Node2[gridWidth, gridLength];
				for (x = 0; x < size.x; x+=0.5F) {
						j = 0;
						for (y = 0; y < size.y; y+=0.5F) {
				
								Vector2 spawnPosition = new Vector2 ((float)(x), (float)(y));
								Vector2 spawnPosition2 = new Vector2 ((float)(x + 0.5), (float)(y + 0.5));
								bool checkResult = Physics2D.OverlapArea (spawnPosition, spawnPosition2);
								Vector2 spawn = new Vector2 ((float)(spawnPosition.x + 0.25) - 11, (float)((spawnPosition.y + 0.25) - 1.5));
								grid [i, j] = (GameObject)Instantiate (Resources.Load ("Node"), spawn, Quaternion.identity);
								grid [i, j].transform.parent = Camera.main.transform;
								grid [i, j].SetActive (true);
								
								//nodeGrid [i, j] = new global::Node2 (checkResult, spawn, grid [i, j], i, j);
								//closestObjects.Add (grid [i, j]);
								//Debug.Log ("closestObjects = " + closestObjects.Count);
								
								//Debug.Log ("Transform of grid[" + i + "][" + j + "] is " + grid [i, j].transform.position);
								j++;
						}
						i++;
				}	
				gridWidth = i;
				gridLength = j;
				//Debug.Log ("i = " + i + ", j = " + j);
				
				for (i = 0; i < gridWidth; i++) {
						for (j = 0; j < gridLength; j++) {
								grid [i, j].GetComponent<NodeAttached>().neighbors = GetNeighbors(i, j);
						}
				}
		}

		public List<Transform> GetNeighbors (int i, int j)
		{
				List<Transform> neighbors = new List<Transform> ();
		
				for (int x = -1; x <=1; x++) {
						for (int y = -1; y <=1; y++) {
								if (x == 0 && y == 0) {
										continue;
								}
								int checkX = i + x;
								int checkY = j + y;
								//Debug.Log ("CheckX = " + checkX + "Check Y = " + checkY);
								//NodeStruct neighbor = new NodeStruct(null,null, 0, 0, 0);
								if (checkX >= 0 && checkX < gridWidth && checkY >= 0 && checkY < gridLength) {
										neighbors.Add (grid [checkX, checkY].GetComponent<Transform>());
										//grid[checkX,checkY].SetActive(true);
										//Debug.Log ("grid["+checkX+","+checkY+"] is on");
								}
				
						}
				}
				//Debug.Log ("Node[" + i + ", "+ j + "] neighborCount " + neighbors.Count);
				return neighbors;
		}
}
