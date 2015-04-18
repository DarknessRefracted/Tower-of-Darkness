using UnityEngine;
using System.Collections;
using System.Collections.Generic;//to use List<GameObject>

public class Grid : MonoBehaviour
{
		public List<Node2> path;
		public Vector2 size;
		public Transform Node;
		public Transform Node_bad;
		public Transform Subject;
		public Transform Target;

		public GameObject[,] grid;
		public Node2 [,] nodeGrid;
		public List<GameObject> closestObjects = new List<GameObject> ();

		GameObject[] tempGrid;

		int i = 0, j = 0;


		float x;
		float y;


		void Awake ()
		{
				CreateGrid ();
		}

		// grid
		void Update(){
			 
			if(path.Count > 0) {
				foreach(Node2 n in path)
				{
					grid[n.gridX,n.gridY].SetActive(true);
					
					//Debug.Log ("Activate:  gridX = "+ n.gridX + ", gridY = " + n.gridY);
					//Debug.Log ("In GRIDSCRIPT");
				}

			}
		}
		
		void CreateGrid ()
		{
				grid = new GameObject[(int)size.x * 2, (int)size.y * 2];
				nodeGrid =  new Node2[(int)size.x * 2, (int)size.y * 2];
				for (x = 0; x < size.x; x+=0.5F) {
						j = 0;
						for (y = 0; y < size.y; y+=0.5F) {
			
								Vector2 spawnPosition = new Vector2 ((float)(x), (float)(y));
								Vector2 spawnPosition2 = new Vector2 ((float)(x + 0.5), (float)(y + 0.5));
								bool checkResult = Physics2D.OverlapArea (spawnPosition, spawnPosition2);
								Vector2 spawn = new Vector2 ((float)(spawnPosition.x + 0.25), (float)(spawnPosition.y + 0.25));
								if (checkResult == false) {
										//grid[i,j] = (GameObject)(Instantiate(Node, new Vector2((float)(spawnPosition.x+0.25),(float)(spawnPosition.y+0.25)), Quaternion.identity));//as GameObject;
										
										grid [i, j] = (GameObject)Instantiate (Resources.Load ("Node"), spawn, Quaternion.identity);
										nodeGrid[i,j] = new global::Node2(checkResult,spawn, grid[i,j], i, j);
										closestObjects.Add (grid [i, j]);
										//Debug.Log ("closestObjects = " + closestObjects.Count);
										grid [i, j].SetActive (false);
								} else {
										//grid[i,j] = (GameObject)(Instantiate(Node_bad, new Vector2((float)(spawnPosition.x+0.25),(float)(spawnPosition.y+0.25)), Quaternion.identity));//as GameObject;
										grid [i, j] = (GameObject)Instantiate (Resources.Load ("Node Bad"), spawn, Quaternion.identity);
										nodeGrid[i,j] = new global::Node2(checkResult,spawn, grid[i,j], i, j);
										//closestObjects.Add (grid [i, j]);
										//Debug.Log ("closestObjects = " + closestObjects.Count);
										grid [i, j].SetActive (false);
								}



								//Debug.Log ("Transform of grid[" + i + "][" + j + "] is " + grid [i, j].transform.position);
								j++;
						}
						i++;

				}

		}
	public List<Node2> GetNeightbors(Node2 node)
	{
		List<Node2> neighbors = new List<Node2>();

		for (int x = -1; x <=1; x++) 
		{
			for (int y = -1; y <=1; y++) 
			{
				if(x == 0 && y ==0)
				{
					continue;
				}
				int checkX = node.gridX + x;
				int checkY = node.gridY + y;
				//Debug.Log ("CheckX = " + checkX + "Check Y = " + checkY);
				//NodeStruct neighbor = new NodeStruct(null,null, 0, 0, 0);
				if(checkX >= 0 && checkX < size.x*2 && checkY >= 0 && checkY < size.y*2)
				{
					neighbors.Add (nodeGrid[checkX,checkY]);
					//grid[checkX,checkY].SetActive(true);
					//Debug.Log ("grid["+checkX+","+checkY+"] is on");
				}
				
			}
		}
		//Debug.Log ("Neighbor Count is = " + neighbors.Count);
		return neighbors;
	}

}
