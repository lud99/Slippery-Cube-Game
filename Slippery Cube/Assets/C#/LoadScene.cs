using UnityEngine;

public class LoadScene : MonoBehaviour {

    GameManagerScript gMScript;
    Animator fade;

    //Start
    void Start() {
        gMScript = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
        fade = GameObject.Find("Fade").GetComponent<Animator>();
    }

    //Load selected scene (not async)
    public void LoadSceneName(string scene)
    {
        if (gMScript.completeLevelUI != null && gMScript.completeLevelUI.activeSelf) //If levels is finished
        {
            if (gMScript.addCoinsDone) gMScript.LoadLevel(scene); //If all bonus coins are added
        }
        else gMScript.LoadLevel(scene);
    }

    //Begin loading selected scene async
    public void BeginAsyncLoading(string scene, int sceneIndex)
    {
        StartCoroutine(gMScript.BeginAsyncLoading(scene, sceneIndex)); //Begin loading selected scene async
    }

    //Load selected scene async without removing local deaths
    public void AsyncLoading()
    {
        gMScript.AsyncLoading(); //Switch to async loaded scene
    }

    //Load selected scene async whith removing local deaths
    public void AsyncLoadingRemoveDeaths()
    {
        gMScript.AsyncLoading(); //Switch to async loaded scene
        gMScript.SaveJson(gMScript.sceneBuildIndex, -1, -1, 0, gMScript.LoadJson().levelDone[gMScript.sceneBuildIndex], -1, -1, -1, -1, -1); //Remove local deaths
    }

    //Restart function
    public void Restart()
    {
        if (gMScript.completeLevelUI != null && gMScript.completeLevelUI.activeSelf) //If level is complete
        {
            if (gMScript.addCoinsDone) gMScript.Restart(true); //Restart            
        } else gMScript.Restart(false); //Restart
    }

    //Load Level function
    public void LevelSelect()
    {
        gMScript.EnterInput();
    }

    //Load Level select function
    public void Customization()
    {
        gMScript.EscapeInput();
    }
}
