using UnityEngine;
using UnityEngine.SceneManagement;		// Requiered to switch scenes
using System.Collections;

public class LevelManager : MonoBehaviour {

    public static LevelManager levelManager;
    public int CurrentSceneIndex;

    // Use this for initialization
    void Awake()
    {
        if (levelManager == null)
        {
            DontDestroyOnLoad(gameObject);
            levelManager = this;
        }
        else if (levelManager != this)
        {
            Destroy(gameObject);
        }
    }

    public void LoadLevel(string name)
    {
        print("Loading " + name);
        SceneManager.LoadScene(name);
    }

    public void EndGame(){
		Debug.Log ("Quit requested");
		Application.Quit ();
	}

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void Update()
    {

    }
    public void LoadStart()
    {
        SceneManager.LoadScene(0);
    }
}
