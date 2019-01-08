using UnityEngine;

public class ButtonScript : MonoBehaviour {

    public GameObject[] EnableUI, DisableUI;
    public GameObject fade, viewport;
    //public int levelBuildIndex;
    GameManagerScript gMScript;
    FollowPlayer followPlayer;

    //Get components
    void Start()
    {
        gMScript = GameObject.Find("GameManager").GetComponent<GameManagerScript>(); //Get game manager
        followPlayer = Camera.main.GetComponent<FollowPlayer>(); //Get camera 'follow player' component
    }

    /*/Lock level
    void Update()
    {
        if (levelBuildIndex != 0 && !gMScript.LoadJson().levelDone[levelBuildIndex - 2])
        {
            this.gameObject.SetActive(false);
        }
    }*/

    //Activate short restart fade
    public void ShortRestartFade()
    {
        fade.GetComponent<Animator>().SetTrigger("FadeOutShort"); //Activate fade
    }

    //Move viewport
    public void MoveViewport(int speed)
    {
        RectTransform rectTrans = viewport.GetComponent<RectTransform>();

        viewport.GetComponent<RectTransform>().position += new Vector3(speed, 0, 0);
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
