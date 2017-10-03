using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour 
{
   public static GameControl instance;
   
   public bool gameOver = false;
   
   private bool gameIsPaused = false;
   private int score = 0;
   
   void Awake()
   {
      //If we don't currently have a game control...
      if (instance == null)
      {
         //...set this one to be it...
         instance = this;
      }
      else if (instance != this)
      {
         //...destroy this one because it is a duplicate.
         Destroy(gameObject);
      }
   }

   void Update()
   {
      
   }
   
   public void togglePauseGame(bool paused) {
      gameIsPaused = paused;
   }
}
