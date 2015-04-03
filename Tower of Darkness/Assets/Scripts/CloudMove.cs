using UnityEngine;
using System.Collections;

public class CloudMove : MonoBehaviour {

	public bool moveLeft;
	Transform targetLeft, targetRight;

	void Start(){
		targetLeft = GameObject.FindGameObjectWithTag ("CloudLeft").transform;
		targetRight = GameObject.FindGameObjectWithTag ("CloudRight").transform;
	}
	
	// Update is called once per frame
	void Update () {
		if(moveLeft){
			transform.position = Vector3.MoveTowards(transform.position, targetLeft.position, 1 * Time.deltaTime);
		}
		else{
			transform.position = Vector3.MoveTowards(transform.position, targetRight.position, 1 * Time.deltaTime);
		}
	}
}
