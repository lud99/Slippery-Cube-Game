using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    AsyncOperation asyncLevelPreview, asyncLevel;
    GameManagerScript gameManager, lvlGameManager;
    GameObject canvas, lvlCanvas;
    PlayerMovement player;

    public int levelToLoad = 2;
    
    delegate void Delegate();

    //Start
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerScript>();
        canvas = GameObject.Find("Canvas");

        bool[] levelsDone = gameManager.LoadJson().levelDone;

        for (int i = 0; i < levelsDone.Length; i++)
        {
            if (levelsDone[15])
            {
                levelToLoad = 15 + 2;
                break;
            }
            if (!levelsDone[i]) { //If found a level that isn't completed
                levelToLoad = 2 + i;
                Debug.Log(i);
                break;
            }
            if (levelsDone[levelsDone.Length - 1]) //If all levels are completed
            {
                levelToLoad = levelsDone.Length + 1;
            }
        }
        StartCoroutine(BeginAsyncLoading(levelToLoad));
    }

    //Play level
    public void Play()
    {
        Debug.Log("Play!");
        asyncLevel.allowSceneActivation = true;
    }

    //Load selected scene Async
    IEnumerator BeginAsyncLoading(int sceneIndex, LoadSceneMode mode = LoadSceneMode.Additive, bool allowSceneActivation = true, bool callback = true)
    {
        //Load scene by build index
        if (mode == LoadSceneMode.Additive) asyncLevelPreview = SceneManager.LoadSceneAsync(sceneIndex, mode);
        else asyncLevel = SceneManager.LoadSceneAsync(sceneIndex, mode); 
        (mode == LoadSceneMode.Additive ? asyncLevelPreview : asyncLevel).allowSceneActivation = allowSceneActivation;
        Debug.Log("Loading level " + sceneIndex);

        while ((mode == LoadSceneMode.Additive ? asyncLevelPreview : asyncLevel).progress <= 1f)
        {
            if ((mode == LoadSceneMode.Additive ? asyncLevelPreview : asyncLevel).isDone)
            {
                Debug.Log("Finished loading level");
                if (callback) OnLoadingComplete();
                break;
            }
            yield return null;
        }
    }

    //When the level is loaded in
    void OnLoadingComplete()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        
        lvlGameManager = GameObject.FindGameObjectsWithTag("GameManager")[1].GetComponent<GameManagerScript>();
        lvlCanvas = GameObject.FindGameObjectWithTag("Level Canvas");

        player.enabled = false;
        player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
        player.transform.position = player.originalPosition;

        lvlCanvas.SetActive(false);
        gameManager.transform.GetChild(0).gameObject.GetComponent<FollowPlayer>().Awake(); //Make trail follow player
        lvlGameManager.transform.GetChild(0).gameObject.SetActive(false);
        lvlGameManager.checkEsc = false;
        lvlGameManager.checkSpace = false;
        lvlGameManager.allowPracticeMode = false;
        Destroy(lvlGameManager.practiceMode[0].GetComponent<PracticeMode>().checkpointObjects[0]);
        lvlGameManager.PracticeMode();

        StartCoroutine(BeginAsyncLoading(levelToLoad, LoadSceneMode.Single, false, false));
    }
}
