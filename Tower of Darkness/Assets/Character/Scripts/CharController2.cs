using UnityEngine;
using System.Collections;

public class CharController2 : MonoBehaviour {
	private bool grounded = true;
	private bool candoublejump = true;
	public float jumpForce= 50f;
	public Rigidbody2D rigidbody2d;
	public float jumpCooldown = 0.21f;
	public float speed = 5f;
	private float move;
	void Start(){
		//rigidbody2D = GetComponent<Rigidbody2D>();
	}

	void Update(){
				if (!grounded) {
						jumpCooldown -= Time.deltaTime;
				} 
		}
	void FixedUpdate(){
		move = Input.GetAxis ("Horizontal");
		rigidbody2D.velocity = new Vector2 (move * speed, rigidbody2D.velocity.y);
				if (Input.GetKeyUp (KeyCode.W)) {
						if (grounded) {
								rigidbody2d.velocity = new Vector2 (rigidbody2D.velocity.x, 0);
								rigidbody2d.AddForce (new Vector2 (0, jumpForce));
								grounded = false;
								candoublejump = true;
								jumpCooldown = 0.2f;
						} else if( jumpCooldown < 0) {
								if (candoublejump) {
										candoublejump = false;
										rigidbody2d.velocity = new Vector2 (rigidbody2D.velocity.x, 0);
										rigidbody2D.AddForce (new Vector2 (0, jumpForce));
								}
						}
				}
		}
	void OnTriggerEnter2D(Collider2D other) {
		grounded = true;
	}
}