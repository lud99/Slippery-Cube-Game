using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour {

    //Load selected scene
    public void LoadSceneName(string scene)
    {
        int sceneBuildIndex;
        if (SceneManager.GetActiveScene().buildIndex > 1) sceneBuildIndex = SceneManager.GetActiveScene().buildIndex - 2;
        else sceneBuildIndex = SceneManager.GetActiveScene().buildIndex;
        GameManagerScript gMScript = GameObject.Find("GameManager").GetComponent<GameManagerScript>();

        //Remove local deaths
        gMScript.SaveJson(sceneBuildIndex, gMScript.LoadJson().levelCoins[sceneBuildIndex], gMScript.LoadJson().levelDeaths[sceneBuildIndex], 0, gMScript.LoadJson().levelDone[sceneBuildIndex], -1, -1, -1); //Save

        SceneManager.LoadScene(scene);
    }

    //Restart function
    public void Restart()
    {
        //Load current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
