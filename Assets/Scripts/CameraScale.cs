using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScale : MonoBehaviour {
   public GameObject player;

   private float currScale;

	// Use this for initialization
	void Start () {
      //camera = GetComponent<Camera> ();
      currScale = Camera.main.transform.position.z;
   }
	
	// Update is called once per frame
	void Update () {
      float newScale = Mathf.Lerp (currScale, -player.transform.localScale.x * 100.0f, 0.1f * Time.deltaTime);

      //Camera.main.transform.position.z = newScale;
      Vector3 newPos = new Vector3 (Camera.main.transform.position.x, Camera.main.transform.position.y, newScale);
      //Camera.main.transform.position = newPos;
      currScale = newScale;
	}
}
