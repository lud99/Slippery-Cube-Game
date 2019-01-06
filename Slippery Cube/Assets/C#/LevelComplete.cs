using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelComplete : MonoBehaviour {

    //Load next level
    public void LoadNextLevel()
    {
        System.GC.Collect();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
