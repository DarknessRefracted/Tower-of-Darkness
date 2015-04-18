using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawner : MonoBehaviour {

	public Grid gridScript;// = GetComponent<Grid>;
	public gameStart game;// = GetComponent<gameStart>;

	GameObject[,] grid;
	List<GameObject> closestObjects;// = new List<GameObject> ();
	public GameObject subject;
	public GameObject target;
	public GameObject startNode;
	public GameObject endNode;
	public Node2 beginNode;
	public Node2 finishNode;
	GameObject[] tempGrid;
	Vector3 spawnClick;

	int i = 0, j = 0;
	int subjectIndexX, subjectIndexY;
	int targetIndexX, targetIndexY;
	

	void Awake()
	{
	
	}

	// Use this for initialization
	void Start () {

	
	}
	
	// Update is called once per frame

		void Update ()
		{
			closestObjects = gridScript.closestObjects;
			grid = gridScript.grid;
			
			if (!GameObject.Find ("Subject(Clone)") && Input.GetMouseButtonDown (0)) {
			spawnClick = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 10));
			//var checkResult = Physics2D.OverlapArea(Input.mousePosition.x, spawnPosition2);
			closestObjects.Sort (ByDistanceSubject);
			startNode = closestObjects[0];//closestObjects [0];
			Debug.Log ("startNode " + startNode.transform.position);
			
			for (i = 0; i < gridScript.size.x*2; ++i) {
				for (j = 0; j < gridScript.size.y*2; ++j) {
					if (grid [i, j].transform.position == startNode.transform.position) {
						//Debug.Log ("Subject " + grid[i, j].transform.position);
						//Debug.Log ("StartNode " + startNode.transform.position);
						subjectIndexX = i;
						subjectIndexY = j;
						beginNode = gridScript.nodeGrid[subjectIndexX,subjectIndexY];
					}
				}
			}

			subject = (GameObject)Instantiate (Resources.Load ("Subject"), 
			                                   grid[ subjectIndexX, subjectIndexY].transform.position, 
			                                   Quaternion.identity);
			//***************************************May have to adjust this to the nearest node.

			} else if (GameObject.Find ("Subject(Clone)") && GameObject.Find ("Target(Clone)")) {
				
				if (Input.GetMouseButtonDown (0)) {
					Destroy (GameObject.Find ("Subject(Clone)"));
					Destroy (GameObject.Find ("Target(Clone)"));
					if (gridScript.path != null) {
						foreach(Node2 n in gridScript.path)
						{
							gridScript.grid[n.gridX,n.gridY].SetActive(false);
							
							
							//Debug.Log ("Deactivate:  gridX = "+ n.gridX + ", gridY = " + n.gridY);
						//grid[n.gridX,n.gridY].SetActive(true);
							//Debug.Log ("SET THEM ACTIVE TO FALSE");
						}
					gridScript.path.Clear();
					}
					//gridScript.path = null;
					//game. = null;
				} 
			} else if (GameObject.Find ("Subject(Clone)") && Input.GetMouseButtonDown (0)) {
				
				spawnClick = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 10));
				
				
				//target.renderer.sortingLayerName = "Object";
				
				closestObjects.Sort (ByDistanceTarget);
				endNode = closestObjects [0];
				for (i = 0; i < gridScript.size.x*2; ++i) {
					for (j = 0; j < gridScript.size.y*2; ++j) {
						if (grid [i, j].transform.position == endNode.transform.position) {
							//Debug.Log ("Target " + grid [i, j].transform.position);
							//Debug.Log ("endNode " + endNode.transform.position);
							targetIndexX = i;
							targetIndexY = j;
							finishNode = gridScript.nodeGrid[targetIndexX,targetIndexY];
						}
					}
				}
				target = (GameObject)Instantiate (Resources.Load ("Target"), 
			                                  grid [targetIndexX, targetIndexY].transform.position, 
			                                  Quaternion.identity);
			//	Debug.Log ("Target is at index (" + targetIndexX + ", " + targetIndexY + ")");

			//game.Invoke("findPath", 100);
				
			}
		}
		
		
		
		int ByDistanceSubject (GameObject a, GameObject b)
		{
			//var dstToA = Vector3.Distance (subject.transform.position, a.transform.position);
			//var dstToB = Vector3.Distance (subject.transform.position, b.transform.position);
			var dstToA = Vector3.Distance (spawnClick, a.transform.position);
			var dstToB = Vector3.Distance (spawnClick, b.transform.position);
			return dstToA.CompareTo (dstToB);
		}
		int ByDistanceTarget (GameObject a, GameObject b)
		{
			var dstToA = Vector3.Distance (spawnClick, a.transform.position);
			var dstToB = Vector3.Distance (spawnClick, b.transform.position);
			return dstToA.CompareTo (dstToB);
		}

}
