using UnityEngine;
using System.Collections;

public class CollisionCheck : MonoBehaviour {

	// Use this for initialization
	public Transform n;
	void Start () {
		n = this.transform;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter2D(Collision2D other) {
		if(other.gameObject.tag != "Radar" && other.gameObject.tag != "Player")
			n.renderer.material.color = Color.red;
	}

	void OnCollisionStay2D(Collision2D other) {
		if(other.gameObject.tag != "Radar" && other.gameObject.tag != "Player")
			n.renderer.material.color = Color.red;
	}

	void OnCollisionExit2D(Collision2D other) {
		n.renderer.material.color = Color.blue;
	}

	void OnTriggerEnter2D(Collider2D other) {
		if(other.gameObject.tag != "Radar" && other.gameObject.tag != "Player")
			n.renderer.material.color = Color.red;
	}
	void OnTriggerStay2D(Collider2D other) {
		if(other.gameObject.tag != "Radar" && other.gameObject.tag != "Player")
			n.renderer.material.color = Color.red;
	}

	void OnTriggerExit2D(Collider2D other) {
		n.renderer.material.color = Color.blue;
	} 
}
