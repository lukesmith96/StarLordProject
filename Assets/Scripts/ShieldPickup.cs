using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @author: Joshua King
 * @since: 11.5.17
 * 
 * Shield pickup that can be pulled in by the tractor beam, spawns a temporary
 * shield on top of the player that absorbs enemy attacks.
 */

public class ShieldPickup : MonoBehaviour {

	public GameObject Shield;

	public float thrust = 10f;

	private Rigidbody2D rb2d;

	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.CompareTag ("Player")) {
			gameObject.SetActive (false);
			GameObject newShield = Instantiate (Shield, new Vector3 (0, 0, 1), Quaternion.identity);
			newShield.transform.localScale = new Vector3 (0.1f, 0.1f, 0.1f);
			int count = 100;
			while (count > 0) {
				newShield.transform.localScale += new Vector3 (1f, 1f, 1f) * (Time.deltaTime);
				count--;
			}
			count = 100;
		}
	}

	void OnTriggerStay2D (Collider2D other){
		if (other.gameObject.CompareTag ("Beam") && other.gameObject.GetComponent<Renderer> ().enabled == true) {
			transform.up = (Vector3.zero - transform.position);
			transform.position += transform.up * Time.deltaTime * thrust;
		} else if (other.gameObject.GetComponent<Renderer> ().enabled == false && rb2d.velocity != Vector2.zero) {
			rb2d.velocity -= new Vector2 (0.1f, 0.1f);
		}
	}
}
