using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	public float scaleValue = 2F;
	public static PlayerController instance;
	public float rotationTime = .5f;

   private bool isColliding;
	private bool rotatePU;
	private float currRotation;
   private Vector3 currScale;
   private Vector3 newScale;

	// Use this for initialization
	void Start () {
		rotatePU = false;
		instance = this;
      currScale = newScale = transform.localScale;
	}

	// Update is called once per frame
	void Update () {
		// Joshua King
		// Pressing '=' increases the scale of the player
		if (Input.GetKeyDown("="))
		{
			transform.localScale += (new Vector3 (scaleValue, scaleValue, 0) * Time.deltaTime);
		}
		//Joshua King
		// Pressing '-' decreases the scale of the player
		if (Input.GetKeyDown("-"))
		{
			transform.localScale -= (new Vector3 (scaleValue, scaleValue, 0) * Time.deltaTime);
		}

		if (rotatePU && currRotation < (Time.time - rotationTime))
		{
			rotatePU = false;
		}
		if (Input.GetKey(KeyCode.UpArrow) && rotatePU)
		{
			transform.Rotate(new Vector3(0, 0, 45) * (Time.deltaTime * transform.localScale.x));
		}
		if (Input.GetKey(KeyCode.DownArrow) && rotatePU)
		{
			transform.Rotate(new Vector3(0, 0, -45) * (Time.deltaTime * transform.localScale.x));
		}
		isColliding = false;

      // Interpolate to newScale
      Vector3 actualScale = Vector3.Lerp (currScale, newScale, 0.1f);
      transform.localScale = actualScale;
      currScale = actualScale;
	}

	void FixedUpdate() {
	}

	void OnTriggerEnter2D(Collider2D other) {
		// Joshua King
		// If an enemy collides with the player, the player loses score
		if (other.gameObject.CompareTag ("Enemy")) {
			other.gameObject.SetActive (false);
			if (isColliding)
				return;
			isColliding = true;
			//transform.localScale -= (new Vector3 (scaleValue, scaleValue, 0) * Time.deltaTime);

			GameControl.instance.score -= 10;
			GameControl.instance.SetScoreText ();

		} else if (other.gameObject.CompareTag ("Asteroid")) {
			// Joshua King
			// If an asteroid comes in contact with the player, the player gains mass
			other.gameObject.SetActive (false);
			addMass ();
		}
	}

	public void addMass()
	{
      newScale += (new Vector3(scaleValue, scaleValue, 0) * Time.deltaTime);
	}

	public void startRotationPU()
	{
		rotatePU = true;
		currRotation = Time.time;
	}

}