using UnityEngine;

public class LoadScene : MonoBehaviour {

    GameManagerScript gMScript;
    Animator fade;

    //Start
    void Start() {
        gMScript = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
        fade = GameObject.Find("Fade").GetComponent<Animator>();
    }

    //Load selected scene
    public void LoadSceneName(string scene)
    {
        if (gMScript.completeLevelUI != null && gMScript.completeLevelUI.activeSelf) //If all bonus coins are added
        {
            if (gMScript.addCoinsDone) gMScript.LoadLevel(scene);
        }
        else gMScript.LoadLevel(scene);
    }

    //Begin load selected scene async function
    public void BeginLoadSceneNameAsync(string scene)
    {
        if (scene != "")
        {
            StartCoroutine(gMScript.BeginAsyncLoadLevel(scene));
            fade.SetTrigger("FadeOutShortLoadScene");
        }
    }

    //Load selected scene Async
    public void LoadSceneNameAsync()
    {
        gMScript.AsyncLoadLevel();
    }

    //Begin Async Restart function
    public void BeginAsyncRestart()
    {
        //Load current scene
        StartCoroutine(gMScript.BeginAsyncRestart());
    }

    //Restart function
    public void AsyncRestart()
    {
        //Async restart
        gMScript.AsyncRestart();
    }
    //Restart function
    public void Restart()
    {
        if (gMScript.completeLevelUI != null && gMScript.completeLevelUI.activeSelf) //If level is complete
        {
            if (gMScript.addCoinsDone) gMScript.Restart(true); //Restart            
        } else gMScript.Restart(false); //Restart            
    }

    //Next level function
    public void NextLevel()
    {
        if (gMScript.addCoinsDone) //If all bonus coins are added
        {
            //Load next level
            gMScript.LoadNextLevel();
        }
    }
}
