using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour 
{
   public static GameControl instance;

   public int score = 0;
   public Text scoreText;
   public bool gameOver = false;

   private bool gameIsPaused = false;
   
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

   void Start() {
      SetScoreText ();
   }

   void Update()
   {
      
   }

   public void SetScoreText()
   {
      scoreText.text = "Score: " + GameControl.instance.score.ToString ();
   }
     

   public void togglePauseGame(bool paused) {
      gameIsPaused = paused;
   }
}
