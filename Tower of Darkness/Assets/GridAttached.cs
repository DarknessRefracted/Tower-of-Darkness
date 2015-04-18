using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridAttached : MonoBehaviour
{

		public List<Node> path;
		public Vector2 size;
		public Transform Node;
		public Transform Node_bad;
		public Transform Subject;
		public Transform Target;
	
		public GameObject[,] grid;
		public Node[,] nodeGrid;
		public List<GameObject> closestObjects = new List<GameObject> ();
		public int gridWidth, gridLength;
	
		GameObject[] tempGrid;
	
		int i = 0, j = 0;
	
	
		float x;
		float y;

		// Use this for initialization
		void Start ()
		{
				CreateGrid ();
		}
	
		// Update is called once per frame
		void Update ()
		{
				CheckGrid ();
		}
		void CreateGrid ()
		{
				gridWidth = (int)size.x * 2;
				gridLength = (int)size.y * 2;
				grid = new GameObject[gridWidth, gridLength];
				nodeGrid = new Node[gridWidth, gridLength];
				for (x = 0; x < size.x; x+=0.5F) {
						j = 0;
						for (y = 0; y < size.y; y+=0.5F) {
				
								Vector2 spawnPosition = new Vector2 ((float)(x), (float)(y));
								Vector2 spawnPosition2 = new Vector2 ((float)(x + 0.5), (float)(y + 0.5));
								bool checkResult = Physics2D.OverlapArea (spawnPosition, spawnPosition2);
								Vector2 spawn = new Vector2 ((float)(spawnPosition.x + 0.25) - 11, (float)((spawnPosition.y + 0.25) - 1.5));
								grid [i, j] = (GameObject)Instantiate (Resources.Load ("Node"), spawn, Quaternion.identity);
								grid [i, j].transform.parent = Camera.main.transform;
								nodeGrid [i, j] = new global::Node (checkResult, spawn, grid [i, j], i, j);
								closestObjects.Add (grid [i, j]);
								//Debug.Log ("closestObjects = " + closestObjects.Count);
								grid [i, j].SetActive (true);
								//Debug.Log ("Transform of grid[" + i + "][" + j + "] is " + grid [i, j].transform.position);
								j++;
						}
						i++;
				}	
		}
		void CheckGrid ()
		{
				for (i = 0; i < gridWidth * 2; i++) {
						for (j = 0; j < gridLength * 2; j++) {

						}
				}
		}
	
}
