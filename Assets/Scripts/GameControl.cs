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
   public GameUIController uiController;

   public bool gameIsPaused = false;
   
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
      SetScoreText();
      RunIntroCutscene();
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
   
   //probably should have cutscene files or something, but here goes
   void RunIntroCutscene() {
      uiController.TogglePause(true);
      uiController.WriteThought("", "Lorem ipsum dolor sit amet,\n"
         + "consectetur adipiscing\n"
         + "elit. Integer interdum varius\n"
         + "pretium.\n"
         + "Aliquam erat volutpat.\n"
         + "Vivamus vitae convallis sapien,\n"
         + "in tristique dolor.\n"
         + "Nunc eget erat tempor,\n"
         + "aliquet justo vel, auctor libero.\n"
         + "Morbi mattis lacus felis, non iaculis\n"
         + "lorem dictum sed. Morbi mi nisi, placerat at\n"
         + "commodo ac, varius et odio. Nulla a magna odio.\n"
         + "Nam in justo nec nulla bibendum auctor at ac sapien.", GameUIController.OUR_TEXT_COLOR, true);
      uiController.WriteThought("", "Hello?", GameUIController.OUR_TEXT_COLOR, false);
      uiController.WriteThought("", "World?", GameUIController.OUR_TEXT_COLOR, false);
      uiController.TogglePause(false);
   }
}
