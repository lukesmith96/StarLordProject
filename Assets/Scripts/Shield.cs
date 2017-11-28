using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour {
	public const float shieldHealth = 50f;
	public float curShieldHealth = 50f;

	private Rigidbody2D rb2d;

	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other) {
      if (other.gameObject.CompareTag ("Enemy") || other.gameObject.CompareTag ("Enemy2")
         || other.gameObject.CompareTag ("Enemy3")) {
			other.gameObject.SetActive (false);
			transform.localScale -= new Vector3 (0.1f, 0.1f, 1);
			curShieldHealth = curShieldHealth - 10f;
		}
		if (curShieldHealth <= 0f) {
			gameObject.SetActive (false);
		}
	}
}
