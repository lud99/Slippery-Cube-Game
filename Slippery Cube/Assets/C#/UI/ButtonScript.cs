using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonScript : MonoBehaviour {

    public GameObject[] EnableUI, DisableUI;
    public GameObject fade, viewport;
    //public bool setSelectedGameObjectOnStart;
    //public int levelBuildIndex;
    GameManagerScript gMScript;
    FollowPlayer followPlayer;

    //Get components
    void Start()
    {
        gMScript = GameObject.Find("GameManager").GetComponent<GameManagerScript>(); //Get game manager
        followPlayer = Camera.main.GetComponent<FollowPlayer>(); //Get camera 'follow player' component
        //if (PlayerPrefs.HasKey("ObjectToSelectOnStart") && setSelectedGameObjectOnStart) SetSelectedGameObjectString(PlayerPrefs.GetString("ObjectToSelectOnStart")); //Set saved object to be selected on start
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
        Debug.Log("Quiting");
        Application.Quit();
    }

    //Set currently selected object (event system)
    public void SetSelectedGameObject(GameObject objectToSelect)
    {
        EventSystem.current.SetSelectedGameObject(objectToSelect); //Set specified object to be selected
    }

    //Set currently selected object (event system) but find the object
    public void SetSelectedGameObjectString(string objectName)
    {
        EventSystem.current.SetSelectedGameObject(GameObject.Find(objectName)); //Set specified object to be selected
    }

    //Delete PlayerPrefs key
    public void DeletePlayerPrefsKey(string key)
    {
        PlayerPrefs.DeleteKey(key); //Delete key
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
