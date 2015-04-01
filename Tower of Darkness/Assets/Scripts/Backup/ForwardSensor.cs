using UnityEngine;
using System.Collections;

public class ForwardSensor : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	
	
	void OnTriggerEnter2D(Collider2D other){
		RaycastHit2D sensor;
		Debug.Log("FUCK");
		
		if(other.CompareTag("Wall")){
			sensor =  Physics2D.Raycast(transform.position, other.transform.position);
			Debug.Log(sensor.distance);
		}
	}
}
