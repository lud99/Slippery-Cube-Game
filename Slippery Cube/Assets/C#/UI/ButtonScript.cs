using UnityEngine;

public class ButtonScript : MonoBehaviour {

    public GameObject[] EnableUI, DisableUI;
    public int levelBuildIndex;
    GameManagerScript gMScript;

    //Init some vars
    void Start()
    {
        gMScript = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
    }

    //Lock level
    void Update()
    {
        if (levelBuildIndex != 0 && !gMScript.LoadJson().levelDone[levelBuildIndex - 2])
        {
            this.gameObject.SetActive(false);
        }
    }

    //Exit Application
    public void QuitGame()
    {
        Application.Quit();
    }

    //Activate and de-activate UI
    public void UIToggle()
    {
        //Loop through all objects to enable
        for (int i = 0; i < EnableUI.Length; i++)
        {
            EnableUI[i].SetActive(true);
        }
        //Loop through all objects to disable
        for (int i = 0; i < DisableUI.Length; i++)
        {
            DisableUI[i].SetActive(false);
        }
    }
}
