using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitingTurretController : TurretController {
   public float radius = 10;
   public int orbitalSpeed = 50;

   private float angle;

	void Start () {
      base.Start();

      // Place turret at random point on radius of player
      Vector2 startPos = GetRandPoint(player.transform.position, radius, out angle);
      transform.position = startPos;

      isInvincible = false;
	}
	
	// Update is called once per frame
	void Update () {
      

      // Orbit around player
      angle += orbitalSpeed * Time.deltaTime;
      transform.position = GetPoint (player.transform.position, radius, angle);

      base.Update ();

      AutoFire ();
	}

   private Vector2 GetRandPoint (Vector2 origin, float radius, out float angle) {
      angle = Random.Range (0, 360);
       
      float x = origin.x + radius * Mathf.Cos (Mathf.Deg2Rad * angle);
      float y = origin.y + radius * Mathf.Sin (Mathf.Deg2Rad * angle);

      return new Vector2 (x, y);
   }

   private Vector2 GetPoint (Vector2 origin, float radius, float angle) {
      float x = origin.x + radius * Mathf.Cos (Mathf.Deg2Rad * angle);
      float y = origin.y + radius * Mathf.Sin (Mathf.Deg2Rad * angle);

      return new Vector2 (x, y);
   }
}
