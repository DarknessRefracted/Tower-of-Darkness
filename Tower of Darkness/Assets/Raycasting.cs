using UnityEngine;
using System.Collections;

public class Raycasting : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//public static RaycastHit2D Raycast(Vector2 origin, Vector2 direction, float distance = Mathf.Infinity, int layerMask = DefaultRaycastLayers, float minDepth = -Mathf.Infinity, float maxDepth = Mathf.Infinity); 
		RaycastHit2D hitDown = Physics2D.Raycast(transform.position, -Vector2.up, 2);
		RaycastHit2D hitUp = Physics2D.Raycast(transform.position, Vector2.up, 2);
		RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.right, 2);
		RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, -Vector2.right, 2);

		if(hitDown.collider != null|| hitUp.collider != null || hitRight.collider != null || hitLeft.collider != null)
		{


		}
	}
}
