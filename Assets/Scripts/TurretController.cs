using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
/*
 * @author: Luke Smith, Ken Oshima
 * @since: 9.26.17
 * 
 * This class is the parent class to all Types of turrets.
 */
public class TurretController : MonoBehaviour {
   public GameObject bullet;
   
   public float speed = 100.0f;
   public float maxRange = 40f;
   public float minRange = 0f;
   public float reloadTime = 2.0f; //seconds
   public int numBullets = 1;
   public float spread = 0.0f; //inaccuracy

   protected CircleCollider2D collider;
   
   private GameObject firingArc;
   private float timeSinceFiring = 2.0f; //seconds
   private List<GameObject> pooledBullets = new List<GameObject>();

   private bool selected = false;
   
   public GameObject poolGameObject;
   private DynamicObjectPool dynamicPool;

   // Use this for initialization
   public void Start () {
      collider = GetComponent<CircleCollider2D> ();
      firingArc = transform.Find("TurretFiringArc").gameObject;

      dynamicPool = (DynamicObjectPool)poolGameObject.GetComponent(typeof(DynamicObjectPool));

      maxRange = bullet.gameObject.GetComponent<BulletController>().maxRange;
      minRange = bullet.gameObject.GetComponent<BulletController>().minRange;
      speed = bullet.gameObject.GetComponent<BulletController>().speed;
      
      float arcScale = (maxRange * 2f) / firingArc.GetComponent<SpriteRenderer>().bounds.size.x;
      firingArc.transform.localScale = new Vector3(arcScale, arcScale, 1);
   }
   
   // Update is called once per frame
   public void Update () {
      if (timeSinceFiring < reloadTime) {
         timeSinceFiring += Time.deltaTime;
      }
   }

   protected void FireBullet(Vector2 direction) {
      if (timeSinceFiring >= reloadTime) {
         timeSinceFiring = 0f;
         transform.up = direction;
         
         float theta = -spread / 2;
         float incTheta = numBullets > 1 ? spread / (numBullets - 1) : 0f;
         if (numBullets == 1) {
            theta = 0f;
         }
         
         GameObject clone;
         Rigidbody2D cloneRb2d;
         for (int i = 0; i < numBullets; ++i, theta += incTheta) {
            //Get instance of Bullet
            clone = dynamicPool.GetPooledObject(bullet);
            //if (clone == null) continue; //for graceful error
            clone.SetActive(true);
            cloneRb2d = clone.GetComponent<Rigidbody2D>();
            
            cloneRb2d.transform.position = transform.position;
            cloneRb2d.velocity = Vector2.zero;
            cloneRb2d.transform.up = transform.up;
            cloneRb2d.transform.Rotate(0, 0, theta);
            // Send bullet on its errand of destruction
            cloneRb2d.AddForce(clone.transform.up * speed);
         }
      }
   }
   
   private GameObject GetPooledObject(List<GameObject> collection) {
      for (int i = 0; i < collection.Count; i++) {
         if (!collection[i].activeInHierarchy) {
            return collection[i];
         }
      }
      GameObject obj = (GameObject)Instantiate(bullet, transform);
      obj.transform.parent = GameObject.Find("BulletHolder").transform;
      obj.SetActive(false);
      collection.Add(obj);
      return obj;
   }

   void OnTriggerEnter2D(Collider2D other) {
      if (other.gameObject.CompareTag("Enemy")) {
         gameObject.SetActive(false);
         other.gameObject.SetActive (false);

         //trigger explosion
         GameObject exe = EnemySpawner.GetPooledObject(EnemySpawner.instance.pooledExplosions);
         exe.transform.position = transform.position;
         exe.SetActive(true);
         exe.GetComponent<ParticleSystem>().Play();


      }
   }
}
